using System;
using System.Collections.Generic;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;

namespace sppenyakitlambung
{
    public partial class HasilDiagnosa : ContentPage
    {
        public HasilDiagnosa()
        {
            InitializeComponent();
            BindingContext = new HasilDiagnosaViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void NavigateToHome(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }

    }
}