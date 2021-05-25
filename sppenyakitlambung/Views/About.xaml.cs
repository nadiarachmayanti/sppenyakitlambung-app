using System;
using System.Collections.Generic;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;

namespace sppenyakitlambung
{
    public partial class About : ContentPage
    {
        public About()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void NavigateToHome(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }

    }

}