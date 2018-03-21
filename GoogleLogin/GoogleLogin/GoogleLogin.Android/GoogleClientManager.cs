using System;
using System.Runtime.CompilerServices;
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
using Application = Android.App.Application;
using Object = Java.Lang.Object;

[assembly: Xamarin.Forms.Dependency(typeof(GoogleClientManager))]
namespace GoogleLogin.Droid
{
    class GoogleClientManager : Object, IGoogleClientManager, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        public static GoogleApiClient googleApiClient { get; set; }
        public static Activity CurrentActivity { get; set; }


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

        static EventHandler<LoginEventArgs> _onLogin;
        public event EventHandler<LoginEventArgs> OnLogin
        {
            add => _onLogin += value;
            remove => _onLogin -= value;
        }

        public void Login()
        {
            Intent intent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            CurrentActivity.StartActivityForResult(intent, 1);
            googleApiClient.Connect();
        }

        protected virtual void OnLoginCompleted(LoginEventArgs e)
        {
            _onLogin?.Invoke(this, e);
        }

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
            System.Diagnostics.Debug.WriteLine("Is it Connected? " + googleApiClient.IsConnected);
           
            // Send the logout result to the receivers
            OnLogoutCompleted(EventArgs.Empty);
        }

        public void OnAuthCompleted(GoogleSignInResult result)
        {
            LoginEventArgs args = new LoginEventArgs();

            // Assume the authentication failed
            args.Message = "Authentication Failed";

            // Log the result of the authentication
            System.Diagnostics.Debug.WriteLine("Is it Authenticated? " + result.IsSuccess);

            if (result.IsSuccess)
            {          
                GoogleSignInAccount userAccount = result.SignInAccount;
                args.User = new GoogleUser
                {
                    Name = userAccount.DisplayName,
                    Email = userAccount.Email,
                    Picture = new Uri((userAccount.PhotoUrl != null ? $"{userAccount.PhotoUrl}" : $"https://autisticdating.net/imgs/profile-placeholder.jpg"))
                };

                args.Message = "The authentication was a success!";
            }

            // Send the result to the receivers
            OnLoginCompleted(args);
        }

        public void OnConnected(Bundle connectionHint)
        {
           
        }

        public void OnConnectionSuspended(int cause)
        {
            LoginEventArgs e = new LoginEventArgs();
            e.Message = "Canceled!";
            _onLogin?.Invoke(this, e);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            LoginEventArgs e = new LoginEventArgs();
            e.Message = result.ErrorMessage;
            _onLogin?.Invoke(this, e);
        }

    }
}