using System;
using sppenyakitlambung.Models;
using Xamarin.Forms;

namespace sppenyakitlambung.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private Pengguna pengguna;

        public RegisterViewModel()
        {
            pengguna = new Pengguna();
        }

        public Pengguna Pengguna {
            get => pengguna;
            set {
                pengguna = value;
                RaisePropertyChanged(nameof(Pengguna));
            }
        }
    }
}
