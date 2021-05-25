using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace sppenyakitlambung.Models
{
    public interface IBaseModel : INotifyPropertyChanged
    {
        bool Set<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null);

        void RaisePropertyChanged(string propertyName = "");
    }
}
