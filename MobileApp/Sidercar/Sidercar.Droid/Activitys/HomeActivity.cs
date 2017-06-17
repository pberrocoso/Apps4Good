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

namespace Sidercar.Droid
{
    [Activity(Label = "Share The Road", Icon = "@drawable/icon72", Theme = "@style/MainTheme")]
    public class HomeActivity : Activity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Home);
            // Create your application here

            Button SoyciclistaButton = FindViewById<Button>(Resource.Id.soyciclista);
            SoyciclistaButton.Click += SoyciclistaButton_Click;

            Button SoyVehiculoButton = FindViewById<Button>(Resource.Id.soyconductor);
            SoyVehiculoButton.Click += SoyVehiculoButton_Click;
        }

        private void SoyciclistaButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(BicicleActivity));
        }

        private void SoyVehiculoButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(VehicleActivity));
        }
    }
}