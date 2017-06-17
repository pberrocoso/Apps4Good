using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Sidercar.Droid.Services;
using Android.Util;
using Android.Locations;
using System.Threading;
using System.Globalization;

namespace Sidercar.Droid
{
    [Activity(Label = "Sidercar", Icon = "@drawable/icon72", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class BicicleActivity : Activity
    {

        TextView TextLatitude;
        TextView TextLongitude;
        TextView TextVelocidad;
        TextView TextAltura;


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.info_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_info:
                    Toast.MakeText(this, "Has pulsado en: " + item.TitleFormatted,
                    ToastLength.Short).Show();
                    return true;

                default:
                    Toast.MakeText(this, "Has pulsado en: " + item.TitleFormatted,
                    ToastLength.Short).Show();
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Bicicle);

            //Activamos el Toolbar//
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.TituloLayoutBicicle);


            App.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) =>
            {
                App.Current.LocationService.LocationChanged += HandleLocationChanged;
                App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                App.Current.LocationService.StatusChanged += HandleStatusChanged;
            };

            TextLatitude = FindViewById<TextView>(Resource.Id.textMainLatitude);
            TextLongitude = FindViewById<TextView>(Resource.Id.textMainLongitude);
            TextVelocidad = FindViewById<TextView>(Resource.Id.textMainVelocidad);
            TextAltura = FindViewById<TextView>(Resource.Id.textMainAltitud);


            // Start the location service:
            App.StartLocationService();
            //global::Xamarin.Forms.Forms.Init(this, bundle);
            //LoadApplication(new App());
        }

        #region Android Location Service methods

        ///<summary>
        /// Updates UI with location data
        /// </summary>
        public async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Android.Locations.Location location = e.Location;
            Log.Debug("LocationChanged", string.Format("Latitude: {0}, Longitude:{1}", location.Latitude, location.Longitude));

            string velocidad = location.Speed.ToString();
            int iResult = 0;
            try
            {
                velocidad = velocidad.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                decimal dVelocidad = decimal.Parse(velocidad);

                dVelocidad = ((dVelocidad * 3600) / 1000);
                dVelocidad = Math.Truncate(dVelocidad);
                iResult = int.Parse(dVelocidad.ToString());
            }
            catch (Exception)
            {
                iResult = -1;
            }


            // these events are on a background thread, need to update on the UI thread
            RunOnUiThread(() =>
            {
                TextLatitude.Text = String.Format("Latitude: {0}", location.Latitude);
                TextLongitude.Text = String.Format("Longitude: {0}", location.Longitude);
                TextAltura.Text = String.Format("Altitude: {0}", location.Altitude);
                TextVelocidad.Text = String.Format("Speed: {0} Km/h", iResult.ToString());
            });


            SidecarAPIModel.InsModel datosInsert = new SidecarAPIModel.InsModel();
            datosInsert.idTipo = "1";
            datosInsert.Latitud = location.Latitude.ToString();
            datosInsert.Longitud = location.Longitude.ToString();
            datosInsert.Velocidad = location.Speed.ToString();
            datosInsert.Identificador = Android.OS.Build.Serial.ToString();

            SideCarAPI.SidecarAPIClient client = new SideCarAPI.SidecarAPIClient();
            await client.insertaDatosAPI(datosInsert);

        }

        public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
            Log.Debug("HandleProviderDisabled", "Location provider disabled event raised");
        }

        public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
            Log.Debug("HandleProviderEnabled", "Location provider enabled event raised");
        }

        public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Log.Debug("HandleStatusChanged", "Location status changed, event raised");
        }

        #endregion

    }
}

