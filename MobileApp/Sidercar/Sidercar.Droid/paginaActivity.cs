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


namespace Sidercar.Droid
{
    [Activity(Label = "paginaActivity")]
    public class paginaActivity : Activity
    {       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.pagina);
        }
    }
}