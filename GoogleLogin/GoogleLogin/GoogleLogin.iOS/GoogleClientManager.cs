using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using GoogleLogin.iOS;
using GoogleLogin.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency( typeof (GoogleClientManager))]
namespace GoogleLogin.iOS
{
    class GoogleClientManager : IGoogleClientManager
    {
        public event EventHandler<LoginEventArgs> OnLogin;
        public event EventHandler OnLogout;

        public void Login()
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}