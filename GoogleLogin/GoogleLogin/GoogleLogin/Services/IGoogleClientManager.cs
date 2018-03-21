using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using GoogleLogin.Models;

namespace GoogleLogin.Services
{

    class LoginEventArgs : EventArgs
    {
        public GoogleUser User { get; set; }
        public string Message { get; set; }
    }

    interface IGoogleClientManager
    {
        event EventHandler<LoginEventArgs> OnLogin;
        event EventHandler OnLogout;
        void Login();
        void Logout();
    }
}
