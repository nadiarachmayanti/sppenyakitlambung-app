using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using sppenyakitlambung.Models;
using sppenyakitlambung.Services;
using sppenyakitlambung.Settings;
using sppenyakitlambung.Utilities.Helper;
using sppenyakitlambung.Utilities.Models;
using sppenyakitlambung.Utils;
using Xamarin.Forms;

namespace sppenyakitlambung.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private Pengguna users;
        private string formValidation;
        protected readonly string baseUrl;
        protected readonly string userUrl;
        //public ICommand NavigateToProfileCommand { get; private set; }


        public RegisterViewModel()
        {
            Users = new Pengguna();
            Users.umur = 0;
            Users.phone_number = "00000000000";
            Users.status = "user";
            Users.address = "default";
            Users.gender = "default";
            baseUrl = AppSettingsManager.Settings["BaseUrl"];
            userUrl = AppSettingsManager.Settings["PenggunaUrl"];
            RegisterCommand = new Command(() => postPenggunaAsync());
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

        public Pengguna Users {
            get => users;
            set {
                users = value;
                RaisePropertyChanged(nameof(Users));
            }
        }

        Command _registerUser;
        public Command RegisterCommand
        {
            get => _registerUser;
            set
            {
                _registerUser = value;
                RaisePropertyChanged(nameof(RegisterCommand));
            }
        }

        public async void postPenggunaAsync()
        {
            try
            {
                Console.WriteLine(Users.fullname);
                var url = $"{AppSettingsManager.Settings["BaseUrl"]}{AppSettingsManager.Settings["PenggunaUrl"]}";
                if (await HttpServiceHelper.ProcessHttpRequestAsync<MobileRegisterRequest, Pengguna>(
                        HttpMethod.Post, $"{url}",
                        new MobileRegisterRequest { fullname = (String)Users.fullname, username = Users.username, email = Users.email, password = Users.password, gender = Users.gender, umur = Users.umur, phone_number = Users.phone_number, address = Users.address, status = Users.status })
                    is BaseHttpResponse<Pengguna> response)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //AppSettings.AddOrUpdateValue("Username", UserName);
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(response.Message);
                        Console.WriteLine(response.Result);
                        App.Current.MainPage.DisplayAlert("Alert", response.Message, "Ok");
                        CurrentUserId = response.Result._id;
                        NavigateToProfile();
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

        public async void NavigateToProfile()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Profile());
        }

    }
}
