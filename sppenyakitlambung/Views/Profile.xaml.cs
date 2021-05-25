using System;
using System.Collections.Generic;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;

namespace sppenyakitlambung
{
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
            NavigationPage.SetHasNavigationBar(this, false);

        }
        public void NavigateToHome(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }

    }

}
