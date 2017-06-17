using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Sidercar.Droid.Services;
using Android.Content.PM;
using Sidercar.SidecarAPIModel;
using Sidercar.SideCarAPI;
using Android.Locations;
using Android.Util;
using Sidercar.Droid.Custom;
using Plugin.Vibrate;
using Android.Support.V4.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Threading;
using Sidercar.Data;
using Sidercar.Data.SQLite;

namespace Sidercar.Droid
{
    //[Activity(Label = "Sidercar", Icon = "@drawable/icon72", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
   [Activity(Label = "Sidercar", Icon = "@drawable/icon72", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class VehicleActivity : FragmentActivity, IOnMapReadyCallback
    {
        List<PosModel> Posiciones;
        private List<Marker> ListaMarkerMaps;
        private GoogleMap Map;
        private const int MAX_POSICONES_TEXT = 2;
        private int Distancia;
        private int Tiempo;
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Vehicle);

            //Leemos de SQLLite los parámetros de búsqueda en backend
            var configuracion = DatabaseHelper.Database.GetItemsAsync().Result;
            this.Tiempo = configuracion[0].Tiempo;
            this.Distancia = configuracion[0].Metros;

            //Activamos el Toolbar//
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.TituloLayoutVehicle);

            //Button HablaButton = FindViewById<Button>(Resource.Id.button1);
            //HablaButton.Click += HablaButton_Click;

            App.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) =>
            {
                App.Current.LocationService.LocationChanged += HandleLocationChanged;
                App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                App.Current.LocationService.StatusChanged += HandleStatusChanged;
            };

            // Start the location service:
            App.StartLocationService();

            //Inicializamos lista de marcadores
            ListaMarkerMaps = new List<Marker>();

            //Inicializamos el mapa
            var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
        }


        #region Android Location Service methods

        ///<summary>
        /// Updates UI with location data
        /// </summary>
        public async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Android.Locations.Location location = e.Location;
            Log.Debug("LocationChanged", string.Format("Latitude: {0}, Longitude:{1}", location.Latitude, location.Longitude));
            SidecarAPIClient client = new SidecarAPIClient();
            Posiciones = await client.getDatosPosCercanasAsync(location.Latitude.ToString(), location.Longitude.ToString(), this.Distancia.ToString(), this.Tiempo.ToString());

            IListAdapter ListAdapter = new PosicionesTableAdapter(this, Posiciones);
            ListView listaPosiciones = FindViewById<ListView>(Resource.Id.list);
            listaPosiciones.Adapter = ListAdapter;

            UpdatePosicionesMapa(Posiciones);
            PintarPosicionActualEnMapa(location.Latitude, location.Longitude);

            ////////
            ////////TODO: Comprobar si el aviso de esa posición ha sido ya y solo volver a repetirla si está a menos de 500 , 200, 100 metros, nunca si la distancia va en aumento
            ////////TODO: Pensar el algoritmo. ¿Dónde almacenamos temporalmente? Dictionary<posicion,aviso> Ordenar por aviso "S" y distancia y seleccionar x primeros
            ////////////////

            var Avisos = GestionAvisosPosiciones.GetAvisosPosicionesCercanas(Posiciones).Where(p => p.Notificar == true).OrderBy(p => p.Distancia);
            int Contador = (Avisos.Count() <= MAX_POSICONES_TEXT ? Avisos.Count() : 2);
            if (Contador > 0)
            {                
                var v = CrossVibrate.Current;
                v.Vibration(500);                

                for (int i = 0; i < Contador; i++)
                {
                    string sTipo = Avisos.ElementAt(i).Tipo;
                    string sVia = Avisos.ElementAt(i).Via;
                    string sMetros = Math.Round(Avisos.ElementAt(i).Distancia).ToString();
                    string texto = string.Format("Atención. {0} a {1} metros por {2}", sTipo, sMetros, sVia);

                    TextToSpeechService serviceSpeek = new Services.TextToSpeechService();
                    serviceSpeek.Speak(texto);
                }
            }
        }

        private void UpdatePosicionesMapa(List<PosModel> ListaPosiciones)
        {
            //Eliminamos los marcadores actuales y pintamos los nuevos.

            foreach (Marker marcador in ListaMarkerMaps)
            {
                marcador.Remove();
            }

            ListaMarkerMaps.Clear();


            foreach (PosModel pos in ListaPosiciones)
            {
                double dlatitude = double.Parse(pos.Latitud.Value.ToString().Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                double dlongitude = double.Parse(pos.Longitud.Value.ToString().Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                string textoMarcador = string.Format("{0} - {1} m", pos.Tipo, Math.Round(pos.Distancia));
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(dlatitude, dlongitude));
                markerOpt1.SetTitle(textoMarcador);
                markerOpt1.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.biciA));
                Marker mm = Map.AddMarker(markerOpt1);
                ListaMarkerMaps.Add(mm);
            }
        }

        private void PintarPosicionActualEnMapa(double Latitude, double Longitude)
        {
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(new LatLng(Latitude, Longitude));
            markerOpt1.SetTitle("Tu");
            markerOpt1.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.car));
            Marker mm = Map.AddMarker(markerOpt1);
            ListaMarkerMaps.Add(mm);

            //Centramos la cámara en la posición del vehículo
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(Latitude, Longitude));
            builder.Zoom(14);
            //  builder.Bearing(155);
            //builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            Map.AnimateCamera(cameraUpdate);

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
        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
        }

        #endregion

        #region calcularOrientacion

        private static double DegreeBearing(double lat1, double lon1, double lat2, double lon2)
        {
            var dLon = ToRad(lon2 - lon1);
            var dPhi = Math.Log(Math.Tan(ToRad(lat2) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat1) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }

        // verify against the website example
        //DegreeBearing(50.36389,-4.15694,42.35111,-71.04083);
        #endregion
    }
}