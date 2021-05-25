using System;
using System.Collections.Generic;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;


namespace sppenyakitlambung
{
    public partial class HistoryDiagnosa : ContentPage
    {
        public HistoryDiagnosa()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void NavigateToHome(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }

    }

}