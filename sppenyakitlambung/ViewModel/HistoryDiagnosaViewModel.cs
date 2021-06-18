using System;
using System.Collections.Generic;
using sppenyakitlambung.Models;
using sppenyakitlambung.Services;
using sppenyakitlambung.Settings;
using sppenyakitlambung.Utils;
using sppenyakitlambung.Views;
using Xamarin.Forms;
namespace sppenyakitlambung.ViewModel
{
    public class HistoryDiagnosaViewModel : BaseViewModel
    {
        protected readonly string baseUrl;

        private bool _showContent;
        public bool ShowContent
        {
            get => _showContent;
            set
            {
                _showContent = value;
                RaisePropertyChanged(nameof(ShowContent));
            }
        }

        private List<UserHistoryDiagnosa> _listHistory;
        public List<UserHistoryDiagnosa> ListHistory {
            get => _listHistory;
            set {
                _listHistory = value;
                RaisePropertyChanged(nameof(ListHistory));
            }
        }

        public HistoryDiagnosaViewModel()
        {
            ListHistory = new List<UserHistoryDiagnosa>();
            // Set baseUrl
            baseUrl = AppSettingsManager.Settings["BaseUrl"];
            ShowContent = new LoadingPopup().showLoading(true);
            getAllHistoryByUser();
        }

        /// <summary>
        /// Get Hisory By UserId dari API
        /// </summary>
        private async void getAllHistoryByUser()
        {
            // Get History
            var httpRequest = await HttpService.GetAsync<List<UserHistoryDiagnosa>>($"{baseUrl}{AppSettingsManager.Settings["HistoryKonsultasiUrl"]}/user/{PreferencesWriter.UserId}"); //{AppSettingsManager.Settings["PertanyaanUrl"]}
            var msg = $"{(httpRequest.Successful ? "" : "Not ")}Found History";
            ListHistory = httpRequest.Result;
            Console.WriteLine($"{msg} : {ListHistory[0].hasilnilai}");
            ShowContent = new LoadingPopup().showLoading(false);
        }
    }
}
