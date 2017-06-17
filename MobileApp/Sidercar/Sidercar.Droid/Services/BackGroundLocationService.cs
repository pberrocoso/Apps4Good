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
using Android.Util;
using Android.Locations;

namespace Sidercar.Droid.Services
{
    [Service]
    public class BackGroundLocationService : Service, ILocationListener
    {
        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
        public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
        public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
        public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

        IBinder binder;
        static readonly string TAG = typeof(BackGroundLocationService).FullName;
        protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService("location") as LocationManager;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;

        
        public override void OnCreate()
        {
            base.OnCreate();
        }
        
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // The bulk of the OnStartComand code as been omitted for clarity. 

            var notification = new Notification.Builder(this)
                .SetContentTitle(Resources.GetString(Resource.String.TituloNotificacionForeground))
                .SetContentText(Resources.GetString(Resource.String.MensajeNotificacionForeground))
                .SetSmallIcon(Resource.Drawable.ic_info_black)
                //.SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                //.AddAction()
                //.AddAction(BuildStopServiceAction())
                .Build();

            // Enlist this instance of the service as a foreground service
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            return StartCommandResult.Sticky;
        }


        public void StartLocationUpdates()
        {
            var locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.NoRequirement;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);            
            LocMgr.RequestLocationUpdates(locationProvider, 0, 10, this);            
        }

        public void StopServiceLocation()
        {
            this.StopForeground(true);
            this.StopSelf();
        }


        public override IBinder OnBind(Intent intent)
        {
            Log.Debug(TAG, "OnBind");
            binder = new LocationServiceBinder(this);
            return binder;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            // Stop getting updates from the location manager:
            //LocMgr.RemoveUpdates(this);
        }


        void ILocationListener.OnLocationChanged(Location location)
        {
            this.LocationChanged(this, new LocationChangedEventArgs(location));
        }

        void ILocationListener.OnProviderDisabled(string provider)
        {
            this.ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
        }

        void ILocationListener.OnProviderEnabled(string provider)
        {
            this.ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
        }

        void ILocationListener.OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            this.StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
        }
    }
}