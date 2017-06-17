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
using Xamarin.Forms;
using Sidercar.Droid;

[assembly: Dependency(typeof(ImplementaAbrirPage))]
namespace Sidercar.Droid
{
    public class ImplementaAbrirPage : IAbrirPage
    {
        public void AbrirPaginaCiclista()
        {
            Forms.Context.StartActivity(typeof(BicicleActivity));
        }

        public void AbrirPaginaConductor()
        {
            Forms.Context.StartActivity(typeof(VehicleActivity));
        }

       
    }
}