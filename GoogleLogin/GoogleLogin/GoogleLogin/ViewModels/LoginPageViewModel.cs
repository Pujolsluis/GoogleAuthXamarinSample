using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
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
            LoginCommand = new Command(LoginAsync);
            LogoutCommand = new Command(Logout);
            googleClientManager = CrossGoogleClient.Current;
            IsLoggedIn = false;
        }

        public void LoginAsync()
        {
            googleClientManager.OnLogin += OnLoginCompleted;
            googleClientManager.LoginAsync();
        }


        private void OnLoginCompleted(object sender, GoogleClientResultEventArgs<GoogleUser> loginEventArgs)
        {
            if (loginEventArgs.Data != null)
            {
                user = loginEventArgs.Data;

                // Log the current user email
                Debug.WriteLine(user.Email);
                IsLoggedIn = true;
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
