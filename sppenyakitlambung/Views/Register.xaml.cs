using System;
using System.Collections.Generic;
using sppenyakitlambung.Utils;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;


namespace sppenyakitlambung
{
    public partial class Register : ContentPage
    {
        public Register()
        {
            
            InitializeComponent();
            BindingContext = new RegisterViewModel();
            NavigationPage.SetHasNavigationBar(this, false);

        }
        public void NavigateToProfile(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Profile());
        }
        public void NavigateToLogin(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Login());
        }

    }
}
