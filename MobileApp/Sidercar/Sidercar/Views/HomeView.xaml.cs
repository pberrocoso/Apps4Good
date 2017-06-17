using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sidercar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void ButtonConductor_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IAbrirPage>().AbrirPaginaConductor();

        }

        private void ButtonCiclista_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IAbrirPage>().AbrirPaginaCiclista();

        }

        private void ButtonConfig_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync((new SettingsView()));
        }
    }
}
