using System;
using System.Net;
using System.Net.Http;
using Rg.Plugins.Popup.Services;
using sppenyakitlambung.Models;
using sppenyakitlambung.Services;
using sppenyakitlambung.Settings;
using sppenyakitlambung.Utilities.Helper;
using sppenyakitlambung.Utilities.Models;
using sppenyakitlambung.Utilities.Models.Response;
using sppenyakitlambung.Utils;
using sppenyakitlambung.Views;
using Xamarin.Forms;
namespace sppenyakitlambung.ViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        private Pengguna users;
        protected readonly string baseUrl;
        protected readonly string userUrl;
        //private bool _isLoading;
        private bool _showContent;

        public ProfileViewModel()
        {
            baseUrl = AppSettingsManager.Settings["BaseUrl"];
            userUrl = AppSettingsManager.Settings["PenggunaUrl"];
            SaveUserCommand = new Command(() => SaveUserAsync());
            // loading true showcontent false
            ShowContent = new LoadingPopup().showLoading(true);
            Console.WriteLine($"Current id {CurrentUserId}");
            getUserDetail();

        }

        public string CurrentUserId
        {
            get
            {
                string id = PreferencesWriter.UserId;
                if (id == "")
                {
                    id = "";
                }
                return id;
            }
            set => PreferencesWriter.UserId = value;
        }

        private async void SaveUserAsync()
        {
            if(CurrentUserId != "")
            {

                var url = $"{AppSettingsManager.Settings["BaseUrl"]}{AppSettingsManager.Settings["PenggunaUrl"]}/{CurrentUserId}";
                Console.WriteLine($"Save clicked! {url}");
                if (await HttpServiceHelper.ProcessHttpRequestAsync<MobileRegisterRequest, ResponseUser>(
                        HttpMethod.Put, $"{url}",
                        new MobileRegisterRequest { fullname = (String)Users.fullname, username = Users.username, email = Users.email, password = Users.password, gender = SelectedGender.ToString(), umur = Users.umur, phone_number = Users.phone_number, address = Users.address, status = Users.status })
                    is BaseHttpResponse<ResponseUser> response)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //AppSettings.AddOrUpdateValue("Username", UserName);
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(response.Message);
                        Console.WriteLine(response.Result);
                        App.Current.MainPage.DisplayAlert("Alert", response.Message, "Ok");
                        NavigateToHome();
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert("Alert", response.Message, "Ok");
                        return;
                    }
                }
            }
            
        }

        private async void getUserDetail()
        {
            var httpRequest = await HttpService.GetAsync<Pengguna>($"{baseUrl}{userUrl}/{PreferencesWriter.UserId}"); //{AppSettingsManager.Settings["PertanyaanUrl"]}
            var msg = $"{(httpRequest.Successful ? "" : "Not ")}Found User";
            Users = httpRequest.Result;
            Console.WriteLine($"{msg} : {Users.fullname}");
            if(Users.gender == "Female")
            {
                IsFemale = true;
            }else if(Users.gender == "Male")
            {
                IsMale = true;
            }
            ShowContent = new LoadingPopup().showLoading(false);
        }

        public Pengguna Users
        {
            get => users;
            set
            {
                users = value;
                RaisePropertyChanged(nameof(Users));
            }
        }

        //public bool IsLoading {
        //    get => _isLoading;
        //    set {
        //        _isLoading = value;
        //        RaisePropertyChanged(nameof(IsLoading));
        //    }
        //}

        public bool ShowContent {
            get => _showContent;
            set {
                _showContent = value;
                RaisePropertyChanged(nameof(ShowContent));
            }
        }
        //RadioButton Button
        object _selectedGender;
        public object SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                RaisePropertyChanged(nameof(SelectedGender));
            }
        }

        string groupName;
        public string GroupName {
            get => groupName;
            set {
                groupName = value;
                RaisePropertyChanged(nameof(GroupName));
            }
        }

        private bool _isMale = false;
        public bool IsMale {
            get => _isMale;
            set {
                _isMale = value;
                RaisePropertyChanged(nameof(IsMale));
            }
        }

        private bool _isFemale = false;
        public bool IsFemale
        {
            get => _isFemale;
            set
            {
                _isFemale = value;
                RaisePropertyChanged(nameof(IsFemale));
            }
        }


        Command _saveUserCommand;
        public Command SaveUserCommand {
            get => _saveUserCommand;
            set {
                _saveUserCommand = value;
                RaisePropertyChanged(nameof(SaveUserCommand));
            }
        }

        

        public async void NavigateToHome()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Home());
        }
    }
}
