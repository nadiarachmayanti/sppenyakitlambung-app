using System;
using System.Collections.Generic;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;

namespace sppenyakitlambung
{
    public partial class Logout : ContentPage
    {
        public Logout()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
            public void NavigateToMainPage(object sender, EventArgs e)
            {
                Navigation.PushAsync(new MainPage());
            }
        public void NavigateToHome(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }

    }

}
