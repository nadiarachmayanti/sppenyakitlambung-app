using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sppenyakitlambung.Utils;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;

namespace sppenyakitlambung
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        public void NavigateToLogin(object sender , EventArgs e)
        {

            if (PreferencesWriter.UserId != "")
            {
                Navigation.PushAsync(new Home());
            }
            else
            {
                Navigation.PushAsync(new Login());
            }
            
        }
            
    }

}
