using System;
using System.Collections.Generic;
using sppenyakitlambung.Models;
using sppenyakitlambung.Models.SubModel;
using sppenyakitlambung.ViewModel;
using Xamarin.Forms;

namespace sppenyakitlambung
{
    public partial class Diagnosa : ContentPage
    {
        private DiagnosaViewModel ViewModel;
        public Diagnosa()
        {
            InitializeComponent();
            ViewModel = new DiagnosaViewModel();
            BindingContext = ViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void NavigateToHasilDiagnosa(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HasilDiagnosa());
        }
        public void NavigateToHome(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }

        KondisiUser previousModel;
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (previousModel != null)
            {
                previousModel.IsSelected = false;
            }
            KondisiUser currentModel = ((CheckBox)sender).BindingContext as KondisiUser;
            ViewModel.SelectedIDKondisi = currentModel._id;
            previousModel = currentModel;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (previousModel != null)
            {
                previousModel.IsSelected = false;
            }
            KondisiUser currentModel = e.SelectedItem as KondisiUser;
            currentModel.IsSelected = true;
            ViewModel.SelectedIDKondisi = currentModel._id;
            previousModel = currentModel;
        }

        void btnNextClicked(object sender, EventArgs e)
        {
            
           //diagnosaViewModel.ListCFUser.Add(new CfUser { pertanyaanId = diagnosaViewModel.ListPertanyaan })
        }
    }

}