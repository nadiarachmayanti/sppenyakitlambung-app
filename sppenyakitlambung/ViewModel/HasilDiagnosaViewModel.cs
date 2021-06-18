using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using sppenyakitlambung.Models;
using sppenyakitlambung.Models.Request;
using sppenyakitlambung.Services;
using sppenyakitlambung.Settings;
using sppenyakitlambung.Utilities.Helper;
using sppenyakitlambung.Utilities.Models.Response;
using sppenyakitlambung.Utils;
using sppenyakitlambung.Views;
using Xamarin.Forms;
namespace sppenyakitlambung.ViewModel
{
    public class HasilDiagnosaViewModel : BaseViewModel
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

        private string _hasilAkhir;
        public string HasilAkhir {
            get => _hasilAkhir;
            set {
                _hasilAkhir = value;
                RaisePropertyChanged(nameof(HasilAkhir));
             }
        }

        private UserDiagnosa _konsultasiUser;
        public UserDiagnosa KonsultasiUser
        {
            get => _konsultasiUser;
            set
            {
                _konsultasiUser = value;
                RaisePropertyChanged(nameof(KonsultasiUser));
            }
        }

        private UserHasilKonsultasi _hasilKonsultasi;
        public UserHasilKonsultasi HasilKonsultasi
        {
            get => _hasilKonsultasi;
            set
            {
                _hasilKonsultasi = value;
                RaisePropertyChanged(nameof(HasilKonsultasi));
            }
        }

        

        public HasilDiagnosaViewModel()
        {
            // Set baseUrl
            baseUrl = AppSettingsManager.Settings["BaseUrl"];
            ShowContent = new LoadingPopup().showLoading(true);
            getHasilPerhitungan();

        }


        /// <summary>
        /// Get Konsultasi By Id dari API
        /// </summary>
        private async void getHasilPerhitungan()
        {
            // Get Konsultasi
            var httpRequest = await HttpService.GetAsync<UserDiagnosa>($"{baseUrl}{AppSettingsManager.Settings["KonsultasiUrl"]}/user/{PreferencesWriter.UserId}"); //{AppSettingsManager.Settings["PertanyaanUrl"]}
            var msg = $"{(httpRequest.Successful ? "" : "Not ")}Found Konsultasi";
            KonsultasiUser = httpRequest.Result;
            Console.WriteLine($"{msg} : {KonsultasiUser._id}");
            getPerhitungan();
        }

        /// <summary>
        /// Get Hasil Perhitungan By Id dari API
        /// </summary>
        private async void getPerhitungan()
        {
            // Get Perhitungan
            var httpRequest = await HttpService.GetAsync<UserHasilKonsultasi>($"{baseUrl}{AppSettingsManager.Settings["PerhitunganUrl"]}/{KonsultasiUser._id}"); //{AppSettingsManager.Settings["PertanyaanUrl"]}
            var msg = $"{(httpRequest.Successful ? "" : "Not ")}Found Perhitungan";
            HasilKonsultasi = httpRequest.Result;
            Console.WriteLine($"{msg} : {HasilKonsultasi.user.fullname}");
            //Console.WriteLine($"{msg} : {HasilKonsultasi.gejalaUser.Count}");
            Console.WriteLine($"{msg} : {HasilKonsultasi.hasil_konsultasi.hasil}");
            // Set Hasil Akhit
            HasilAkhir = $"Kemungkinan anda terdeteksi penyakit {HasilKonsultasi.hasil_konsultasi.penyakit.namapenyakit} dengan derajat keyakinan {HasilKonsultasi.hasil_konsultasi.hasil}%";
            // Add History
            postHistory();
            // Hide Loading
            ShowContent = new LoadingPopup().showLoading(false);
        }

        private async void postHistory()
        {
            if (KonsultasiUser._id != "")
            {

                var url = $"{AppSettingsManager.Settings["BaseUrl"]}{AppSettingsManager.Settings["HistoryKonsultasiUrl"]}";
                if (await HttpServiceHelper.ProcessHttpRequestAsync<MobileHistoryRequest, ResponseUser>(
                        HttpMethod.Post, $"{url}",
                        new MobileHistoryRequest { hasilnilai = HasilKonsultasi.hasil_konsultasi.hasil, konsultasiId=KonsultasiUser._id, penyakitId=HasilKonsultasi.hasil_konsultasi.penyakit._id })
                    is BaseHttpResponse<ResponseUser> response)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //AppSettings.AddOrUpdateValue("Username", UserName);
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(response.Message);
                        Console.WriteLine(response.Result);
                        //App.Current.MainPage.DisplayAlert("Alert", response.Message, "Ok");
                    }
                    else
                    {
                        //App.Current.MainPage.DisplayAlert("Alert", response.Message, "Ok");
                        return;
                    }
                }
            }
        }
    }
}
