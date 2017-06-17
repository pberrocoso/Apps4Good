using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidercar.Data.SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sidercar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
            this.BindingContext = this;
            ConfigTable datosActuales = DatabaseHelper.Database.GetItemsAsync().Result[0];
            this.pDistancia.SelectedItem = datosActuales.Metros.ToString();
            this.pTiempo.SelectedItem = datosActuales.Tiempo.ToString();
        }

        public List<string> MetrosDistancia = new List<string> {"200","500","1000"};
        public List<string> TiempoSegundos = new List<string> { "2", "4", "6" };

        private void ButtonVolver_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void PickerDistancia_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker pDis = (Picker)sender;
            ConfigTable datosActuales = DatabaseHelper.Database.GetItemsAsync().Result[0];
            datosActuales.Metros = int.Parse(pDis.SelectedItem.ToString());
            DatabaseHelper.Database.SaveItemAsync(datosActuales);

        }

        private void PickerTiempo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker pTie = (Picker)sender;
            ConfigTable datosActuales = DatabaseHelper.Database.GetItemsAsync().Result[0];
            datosActuales.Tiempo = int.Parse(pTie.SelectedItem.ToString());
            DatabaseHelper.Database.SaveItemAsync(datosActuales);
        }


    }
}