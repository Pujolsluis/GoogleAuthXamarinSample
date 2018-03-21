using System;
using System.ComponentModel;


namespace GoogleLogin.Models
{
    public class GoogleUser : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Picture { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
