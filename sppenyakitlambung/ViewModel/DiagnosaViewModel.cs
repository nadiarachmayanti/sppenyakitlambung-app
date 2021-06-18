using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using sppenyakitlambung.Models;
using sppenyakitlambung.Models.Request;
using sppenyakitlambung.Models.SubModel;
using sppenyakitlambung.Services;
using sppenyakitlambung.Settings;
using sppenyakitlambung.Utilities.Helper;
using sppenyakitlambung.Utilities.Models.Response;
using sppenyakitlambung.Utils;
using sppenyakitlambung.Views;
using Xamarin.Forms;
namespace sppenyakitlambung.ViewModel
{
    public class DiagnosaViewModel : BaseViewModel

    {
        protected readonly string baseUrl;
        private string _status;
        public string Status
        {
            get => _status; set => _status = value;
        }

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

        private string selectedIDKondisi;
        public string SelectedIDKondisi
        {
            get => selectedIDKondisi;
            set
            {
                selectedIDKondisi = value;
                RaisePropertyChanged(nameof(SelectedIDKondisi));
            }
        }

        /// <summary>
        /// Menyimpan Daftar Jawaban
        /// </summary>
        private List<CfUser> _listCFUser;
        public List<CfUser> ListCFUser {
            get => _listCFUser;
            set {
                _listCFUser = value;
                RaisePropertyChanged(nameof(ListCFUser));
            }
        }

        /// <summary>
        /// Menyimpan Daftar Pertanyaan
        /// </summary>
        private List<DaftarPertanyaan> _listPertanyaan;
        public List<DaftarPertanyaan> ListPertanyaan
        {
            get => _listPertanyaan;
            private set {
                _listPertanyaan = value;
                RaisePropertyChanged(nameof(ListPertanyaan));
            }
        }
        /// <summary>
        /// Menyimpan Daftar Kondisi
        /// </summary>
        private List<KondisiUser> _listKondisi;
        public List<KondisiUser> ListKondisi
        {
            get => _listKondisi;
            private set
            {
                _listKondisi = value;
                RaisePropertyChanged(nameof(ListKondisi));
            }
        }
        /// <summary>
        /// Current Pertanyaan
        /// </summary>
        private DaftarPertanyaan _pertanyaan;
        public DaftarPertanyaan Pertanyaan {
            get => _pertanyaan;
            set {
                _pertanyaan = value;
                RaisePropertyChanged(nameof(Pertanyaan));
           }
        }

        /// <summary>
        /// Index Pertanyaan
        /// </summary>
        private int currentIndex;
        public int CurrentIndex {
            get => currentIndex;
            set {
                currentIndex = value;
                RaisePropertyChanged(nameof(CurrentIndex));
            }
        }

        private string _noPertanyaan;
        public string NoPertanyaan {
            get => _noPertanyaan;
            set {
                _noPertanyaan = value;
                RaisePropertyChanged(nameof(NoPertanyaan));
            }
        }


        public DiagnosaViewModel()
        {
            Pertanyaan = new DaftarPertanyaan();
            ListCFUser = new List<CfUser>();
            NextCommand = new Command(() => nextPertanyaan());
            BackCommand = new Command(() => backPertanyaan());
            // Set default index
            CurrentIndex = 0;
            // Set Title
            NoPertanyaan = $"Pertanyaan {CurrentIndex+1}";
            // Set baseUrl
            baseUrl = AppSettingsManager.Settings["BaseUrl"];
            // Show Loading
            ShowContent = new LoadingPopup().showLoading(true);
            // getPertanyaan from API
            getPertanyaan(); 
            
        }

        /// <summary>
        /// Get Pertanyaan dari API
        /// </summary>
        private async void  getPertanyaan()
        {
            // Pertanyaan
            var httpRequest = await HttpService.GetAsync<List<DaftarPertanyaan>>($"{baseUrl}{AppSettingsManager.Settings["PertanyaanUrl"]}"); //{AppSettingsManager.Settings["PertanyaanUrl"]}
            var msg = $"{(httpRequest.Successful ? "" : "Not ")}Found Pertanyaan";
            ListPertanyaan = httpRequest.Result;
            Pertanyaan = ListPertanyaan[CurrentIndex];
            Console.WriteLine($"{msg} : {ListPertanyaan.Count}");
            getKondisi();
            // Hide Loading
            ShowContent = new LoadingPopup().showLoading(false);
        }
        /// <summary>
        /// Get kondisi dari API
        /// </summary>
        private async void getKondisi()
        {
            // Kondisi User
            var httpRequest = await HttpService.GetAsync<List<KondisiUser>>($"{baseUrl}{AppSettingsManager.Settings["KondisiUrl"]}");
            ListKondisi = httpRequest.Result;
            Console.WriteLine($"{httpRequest.Successful} : {ListKondisi[2].bobot}");
            // binding xaml
            for (int i = 0; i < ListKondisi.Count; i++)
            {
                ListKondisi[i].IsSelected = false;
            }
        }

        private async void postDiagnosa(String Id, List<CfUser> listcf)
        {
            try
            {
                var url = $"{AppSettingsManager.Settings["BaseUrl"]}{AppSettingsManager.Settings["KonsultasiUrl"]}";
                if (await HttpServiceHelper.ProcessHttpRequestAsync<MobileDiagnosaRequest, ResponseUser>(
                        HttpMethod.Post, $"{url}",
                        new MobileDiagnosaRequest { userId= Id , temp_cfuser = listcf })
                    is BaseHttpResponse<ResponseUser> response)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(response.Message);
                        Console.WriteLine(response.Result);
                        App.Current.MainPage.DisplayAlert("Alert", response.Message, "Ok");
                        await Application.Current.MainPage.Navigation.PushAsync(new HasilDiagnosa());
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert("Alert", response.Message, "Ok");
                        return;
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                App.Current.MainPage.DisplayAlert("Alert", exception.Message, "Ok");
            }
        }

        /// <summary>
        /// Next Button Command
        /// </summary>
        Command _nextCommand;
        public Command NextCommand {
            get => _nextCommand;
            set {
                _nextCommand = value;
                RaisePropertyChanged(nameof(NextCommand));
            }
        }
        /// <summary>
        /// Back Button Command
        /// </summary>
        Command _backCommand;
        public Command BackCommand {
            get => _backCommand;
            set {
                _backCommand = value;
                RaisePropertyChanged(nameof(BackCommand));
            }
        }

        /// <summary>
        /// Next Pertanyaan Function
        /// </summary>
        public void nextPertanyaan()
        {
            if (CurrentIndex < (ListPertanyaan.Count - 1))
            {
                // Saave jawaban dulu
                if(SelectedIDKondisi != "606aeb9a3a56e905f4672f0f")
                {
                    ListCFUser.Add(new CfUser { pertanyaanId = ListPertanyaan[CurrentIndex]._id, kondisiuserId = SelectedIDKondisi });
                }
                //Console.WriteLine($"{ListCFUser[CurrentIndex].kondisiuserId}");
                // next index
                CurrentIndex++;
                Pertanyaan = ListPertanyaan[CurrentIndex];
                NoPertanyaan = $"Pertanyaan {CurrentIndex + 1}";
                Console.WriteLine(CurrentIndex);
            }
            else
            {
                // save last id
                ListCFUser.Add(new CfUser { pertanyaanId = ListPertanyaan[CurrentIndex]._id, kondisiuserId = SelectedIDKondisi });
                // then save
                if (ListCFUser.Count > 0)
                {
                    postDiagnosa(PreferencesWriter.UserId, ListCFUser);
                }
                
            }
        }

        /// <summary>
        /// Back Pertanyaan Function
        /// </summary>
        public void backPertanyaan()
        {
            if (CurrentIndex != 0)
            {
                CurrentIndex--;
                Pertanyaan = ListPertanyaan[CurrentIndex];
                NoPertanyaan = $"Pertanyaan {CurrentIndex + 1}";
                Console.WriteLine(CurrentIndex);
            }
            else
            {
                Console.WriteLine("back nya ilang");
            }
        }


    }
}
