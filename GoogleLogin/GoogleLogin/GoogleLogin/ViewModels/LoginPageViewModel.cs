using GoogleLogin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using GoogleLogin.Services;
using Xamarin.Forms;

namespace GoogleLogin.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public GoogleUser user { get; set; }
        public string Name
        {
            get { return user.Name; }
            set { user.Name = value; }
        }

        public string Email
        {
            get { return user.Email; }
            set { user.Email = value; }
        }

        public Uri Picture
        {
            get { return user.Picture; }
            set { user.Picture = value; }
        }

        public  bool IsLoggedIn { get; set; }

        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        private IGoogleClientManager googleClientManager;
        public event PropertyChangedEventHandler PropertyChanged;

        public LoginPageViewModel()
        {
            googleClientManager = DependencyService.Get<IGoogleClientManager>();
            LoginCommand = new Command(Login);
            LogoutCommand = new Command(Logout);

            IsLoggedIn = false;
        }

        public void Login()
        {
            googleClientManager.OnLogin += OnLoginCompleted;
            googleClientManager.Login();
        }


        private void OnLoginCompleted(object sender, LoginEventArgs loginEventArgs)
        {
            if (loginEventArgs.User != null)
            {
                user = loginEventArgs.User;
                System.Diagnostics.Debug.WriteLine(loginEventArgs.User.Email);
                IsLoggedIn = true;
               // App.Current.MainPage.DisplayAlert("Success?", user.Name + "\n" + user.Email + "\n" + user.Picture, "OK");
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", loginEventArgs.Message, "OK");
            }

            googleClientManager.OnLogin -= OnLoginCompleted;

        }

        public void Logout()
        {
            googleClientManager.OnLogout += OnLogoutCompleted;
            googleClientManager.Logout();
        }

        private void OnLogoutCompleted(object sender, EventArgs loginEventArgs)
        {
            IsLoggedIn = false;
            user.Email = "Offline";
            googleClientManager.OnLogout -= OnLogoutCompleted;
        }

    }
}
