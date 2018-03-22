﻿using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using GoogleLogin.Droid;
using GoogleLogin.Models;
using GoogleLogin.Services;
using Xamarin.Forms;
using Application = Android.App.Application;
using Debug = System.Diagnostics.Debug;
using Object = Java.Lang.Object;
using String = System.String;

[assembly: Dependency(typeof(GoogleClientManager))]
namespace GoogleLogin.Droid
{
    class GoogleClientManager : Object, IGoogleClientManager, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        // Class Debug Tag
        private String Tag = typeof(GoogleClientManager).FullName;
        public static GoogleApiClient googleApiClient { get; set; }
        public static Activity CurrentActivity { get; set; }
        static TaskCompletionSource<GoogleResponse<GoogleUser>> _loginTcs;


        public GoogleClientManager()
        {
            GoogleSignInOptions googleSignInOptions =
                new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                    .RequestEmail()
                    .Build();

            googleApiClient = new GoogleApiClient.Builder(Application.Context)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, googleSignInOptions)
                .AddScope(new Scope(Scopes.Profile))
                .Build();
        }

        public static void Initialize(Activity activity)
        {
            CurrentActivity = activity;
        }

        static EventHandler<GoogleClientResultEventArgs<GoogleUser>> _onLogin;
        public event EventHandler<GoogleClientResultEventArgs<GoogleUser>> OnLogin
        {
            add => _onLogin += value;
            remove => _onLogin -= value;
        }

        public async Task<GoogleResponse<GoogleUser>> LoginAsync()
        {
            _loginTcs = new TaskCompletionSource<GoogleResponse<GoogleUser>>();
            Intent intent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            CurrentActivity.StartActivityForResult(intent, 1);
            googleApiClient.Connect();

            return await _loginTcs.Task;
        }

        //public void Login()
        //{
        //    Intent intent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
        //    CurrentActivity.StartActivityForResult(intent, 1);
        //    googleApiClient.Connect();
        //}

        static EventHandler _onLogout;
        public event EventHandler OnLogout
        {
            add => _onLogout += value;
            remove => _onLogout -= value;
        }

        protected virtual void OnLogoutCompleted(EventArgs e)
        {
            _onLogout?.Invoke(this, e);
        }

        public void Logout()
        {
            Auth.GoogleSignInApi.SignOut(googleApiClient);
            googleApiClient.Disconnect();

            // Log the state of the client
            Debug.WriteLine(Tag + ": Is the user Connected? " + googleApiClient.IsConnected);
           
            // Send the logout result to the receivers
            OnLogoutCompleted(EventArgs.Empty);
        }

        public bool IsLoggedIn { get; }

        public void OnAuthCompleted(GoogleSignInResult result)
        {
            GoogleUser googleUser = null;

            // Log the result of the authentication
            Debug.WriteLine(Tag + ": Is it Authenticated? " + result.IsSuccess);

            if (result.IsSuccess)
            {          
                GoogleSignInAccount userAccount = result.SignInAccount;
                googleUser = new GoogleUser
                {
                    Name = userAccount.DisplayName,
                    Email = userAccount.Email,
                    Picture = new Uri((userAccount.PhotoUrl != null ? $"{userAccount.PhotoUrl}" : $"https://autisticdating.net/imgs/profile-placeholder.jpg"))
                };

                var googleArgs =
                    new GoogleClientResultEventArgs<GoogleUser>(googleUser, GoogleActionStatus.Completed, result.Status.StatusMessage);
               
                // Send the result to the receivers
                _onLogin?.Invoke(this, googleArgs);
                _loginTcs.TrySetResult(new GoogleResponse<GoogleUser>(googleArgs));
            }
            else
            {
                var googleArgs =
                    new GoogleClientResultEventArgs<GoogleUser>(googleUser, GoogleActionStatus.Canceled, result.Status.StatusMessage);
                
                // Send the result to the receivers
                _onLogin?.Invoke(this, googleArgs);
                _loginTcs.TrySetResult(new GoogleResponse<GoogleUser>(googleArgs));
            }

        }

        public void OnConnected(Bundle connectionHint)
        {
           
        }

        public void OnConnectionSuspended(int cause)
        {
            GoogleClientResultEventArgs<GoogleUser> googleArgs = new GoogleClientResultEventArgs<GoogleUser>(null, GoogleActionStatus.Error, "the user has disconnected");
            _onLogin?.Invoke(this, googleArgs);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            GoogleClientResultEventArgs<GoogleUser> googleArgs = new GoogleClientResultEventArgs<GoogleUser>(null, GoogleActionStatus.Error, "the connection to the client has failed");
            _onLogin?.Invoke(this, googleArgs);
        }

    }
}