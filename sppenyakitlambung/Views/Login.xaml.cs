using System;
using System.Collections.Generic;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;


namespace sppenyakitlambung
{
    public partial class Login : ContentPage
    {
            public Login()
            {
                InitializeComponent();
                BindingContext = new LoginViewModel();
                NavigationPage.SetHasNavigationBar(this, false);

            }
        public void NavigateToHome(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }
        public void NavigateToRegister(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Register());
        }

    }

    }
