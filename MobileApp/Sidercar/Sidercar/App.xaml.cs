using Sidercar.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Sidercar
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ConfigTable con = new ConfigTable();
            var configuracion = DatabaseHelper.Database.GetItemsAsync().Result;
            if (configuracion.Count > 0)
            {

                con.Metros = configuracion[0].Metros;
                con.Name = configuracion[0].Name;
                con.Tiempo = configuracion[0].Tiempo;
            }
            else
            {
                ///Configuración por defecto

                con.Metros = 1000;
                con.Name = string.Empty; 
                con.Tiempo = 5;

                DatabaseHelper.Database.SaveItemAsync(con);
            }         
            MainPage = new NavigationPage(new Sidercar.Views.HomeView());
         
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
