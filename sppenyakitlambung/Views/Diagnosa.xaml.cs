using System;
using System.Collections.Generic;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;

namespace sppenyakitlambung
{
    public partial class Diagnosa : ContentPage
    {
        public Diagnosa()
        {
            InitializeComponent();
            BindingContext = new DiagnosaViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void NavigateToHasilDiagnosa(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HasilDiagnosa());
        }

    }

}
