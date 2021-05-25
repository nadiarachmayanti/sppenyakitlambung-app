using System;
using System.Collections.Generic;
using sppenyakitlambung.Models;
using sppenyakitlambung.Services;
using sppenyakitlambung.Settings;
using Xamarin.Forms;
namespace sppenyakitlambung.ViewModel
{
    class DiagnosaViewModel : BaseViewModel

    {
        protected readonly string baseUrl;
        private string _status;
        public string Status
        {
            get => _status; set => _status = value;
        }
        
        private List<DaftarPertanyaan> _listPertanyaan;
        public List<DaftarPertanyaan> ListPertanyaan
        {
            get => _listPertanyaan;
            private set {
                _listPertanyaan = value;
                RaisePropertyChanged(nameof(ListPertanyaan));
            }
        }
        
        private DaftarPertanyaan _pertanyaan;
        public DaftarPertanyaan Pertanyaan {
            get => _pertanyaan;
            private set {
                _pertanyaan = value;
                RaisePropertyChanged(nameof(Pertanyaan));
           }
        }

        private int currentIndex;
        public int CurrentIndex {
            get => currentIndex;
            private set {
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
            NextCommand = new Command(() => nextPertanyaan());
            BackCommand = new Command(() => backPertanyaan());
            // Set default index
            CurrentIndex = 0;
            // Set Title
            NoPertanyaan = $"Pertanyaan {CurrentIndex+1}";
            // Set baseUrl
            baseUrl = AppSettingsManager.Settings["BaseUrl"];
            // getPertanyaan from API
            getPertanyaan(); 
            
        }

        public async void  getPertanyaan()
        {
            var httpRequest = await HttpService.GetAsync<List<DaftarPertanyaan>>($"{baseUrl}{AppSettingsManager.Settings["PertanyaanUrl"]}"); //{AppSettingsManager.Settings["PertanyaanUrl"]}
            var msg = $"{(httpRequest.Successful ? "" : "Not ")}Found Pertanyaan";
            ListPertanyaan = httpRequest.Result;
            Pertanyaan = ListPertanyaan[CurrentIndex];
            Console.WriteLine($"{msg} : {ListPertanyaan.Count}");
        }

        Command _nextCommand;
        public Command NextCommand {
            get => _nextCommand;
            set {
                _nextCommand = value;
                RaisePropertyChanged(nameof(NextCommand));
            }
        }

        Command _backCommand;
        public Command BackCommand {
            get => _backCommand;
            set {
                _backCommand = value;
                RaisePropertyChanged(nameof(BackCommand));
            }
        }

        public void nextPertanyaan()
        {
            if (CurrentIndex < (ListPertanyaan.Count-1)) {
                CurrentIndex++;
                Pertanyaan = ListPertanyaan[CurrentIndex];
                NoPertanyaan = $"Pertanyaan {CurrentIndex + 1}";
                Console.WriteLine(CurrentIndex);
            }
            else
            {
                Console.WriteLine("Pindah ke home");
            }
            
        }

        public void backPertanyaan()
        {
            if(CurrentIndex != 0)
            {
                CurrentIndex--;
                Pertanyaan = ListPertanyaan[CurrentIndex];
                NoPertanyaan = $"Pertanyaan {CurrentIndex+1}";
                Console.WriteLine(CurrentIndex);
            }
            else
            {
                Console.WriteLine("back nya ilang");
            }
        }


    }
}
