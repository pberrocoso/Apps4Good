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
using Sidercar.Data.SQLite;
using Sidercar.Droid;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency(typeof(ImplementaFileHelper))]
namespace Sidercar.Droid
{
    public class ImplementaFileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}