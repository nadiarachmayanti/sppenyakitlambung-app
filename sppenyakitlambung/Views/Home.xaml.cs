using System;
using System.Collections.Generic;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;

namespace sppenyakitlambung
{
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        public void NavigateToProfile(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Profile());
        }

        public void NavigateToAbout(object sender, EventArgs e)
        {
            Navigation.PushAsync(new About());
        }
        public void NavigateToDiagnosa(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Diagnosa());
        }
        public void NavigateToLogout(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Logout());
        }
        public void NavigateToHistoryDiagnosa(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistoryDiagnosa());
        }

    }
}