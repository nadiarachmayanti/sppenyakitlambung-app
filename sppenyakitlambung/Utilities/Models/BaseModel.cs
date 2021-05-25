using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using sppenyakitlambung.Models;

namespace sppenyakitlambung.Utilities.Models
{
    public class BaseModel : IBaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModel"/> class.
        /// </summary>
        public BaseModel() { }

        public bool Set<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises the property changed event if the property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
