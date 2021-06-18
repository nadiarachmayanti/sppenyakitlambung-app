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
            BindingContext = new HistoryDiagnosaViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void NavigateToHome(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Console.WriteLine(e.SelectedItem);
        }

    }

}