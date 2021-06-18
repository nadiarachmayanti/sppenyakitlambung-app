using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace sppenyakitlambung.Models
{
    public class KondisiUser : INotifyPropertyChanged
    {
        public string _id { get; set; }
        public double bobot { get; set; }
        public string namakondisi { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        bool isSelected;
        public bool IsSelected
        {
            set
            {
                isSelected = value;
                onPropertyChanged();
            }
            get => isSelected;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
