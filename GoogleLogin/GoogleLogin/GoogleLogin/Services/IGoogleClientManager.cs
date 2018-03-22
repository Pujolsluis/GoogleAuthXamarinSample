using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GoogleLogin.Models;

namespace GoogleLogin.Services
{

    public enum GoogleActionStatus
    {
        Canceled,
        Unauthorized,
        Completed,
        Error
    }

    class GoogleClientResultEventArgs<T> : EventArgs
    {
        public T Data { get; set; }
        public GoogleActionStatus Status { get; set; }
        public string Message { get; set; }

        public GoogleClientResultEventArgs(T data, GoogleActionStatus status, string msg = "")
        {
            Data = data;
            Status = status;
            Message = msg;
        }
    }

    class GoogleResponse<T>
    {
        public T Data { get; set; }
        public GoogleActionStatus Status { get; set; }
        public string Message { get; set; }

        public GoogleResponse(GoogleClientResultEventArgs<T> evtArgs)
        {
            Data = evtArgs.Data;
            Status = evtArgs.Status;
            Message = evtArgs.Message;
        }

        public GoogleResponse(T user, GoogleActionStatus status, string msg = "")
        {
            Data = user;
            Status = status;
            Message = msg;
        }
    }


    /// <summary>
    /// Interface for GoogleClient
    /// </summary>
    interface IGoogleClientManager
    {
        event EventHandler<GoogleClientResultEventArgs<GoogleUser>> OnLogin;
        event EventHandler OnLogout;
        Task<GoogleResponse<GoogleUser>> LoginAsync();
        void Logout();
        bool IsLoggedIn { get; }
    }
}
