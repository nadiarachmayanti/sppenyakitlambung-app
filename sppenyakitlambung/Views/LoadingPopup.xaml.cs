using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace sppenyakitlambung.Views
{
    public partial class LoadingPopup : PopupPage
    {
        /// <summary>
        /// Closes the popup when close button is clicked
        /// </summary>
        public void ClosePopup(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }

        public bool showLoading(bool loading)
        {
            InitializeComponent();
            if (loading)
            {
                PopupNavigation.Instance.PushAsync(this);
                return false;
            }
            else
            {
                PopupNavigation.Instance.PopAsync();
                return true;
            }
        }

    }
}
