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

namespace Sidercar.Droid.Services
{
    public class LocationServiceBinder:Binder
    {
        protected BackGroundLocationService service;

        public BackGroundLocationService Service
        {
            get { return this.service; }
        }

        public bool IsBound { get; set; }

        // constructor
        public LocationServiceBinder(BackGroundLocationService service)
        {
            this.service = service;
        }
    }
}