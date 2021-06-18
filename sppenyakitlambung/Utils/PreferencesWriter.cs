using System;
using Xamarin.Essentials;

namespace sppenyakitlambung.Utils
{
    public class PreferencesWriter
    {
        private const string CURRENT_USER_ID = "CURRENT_USER_ID";
        public static string UserId
        {
            get => Preferences.Get(CURRENT_USER_ID, "");
            set => Preferences.Set(CURRENT_USER_ID, value);
        }
    }
}
