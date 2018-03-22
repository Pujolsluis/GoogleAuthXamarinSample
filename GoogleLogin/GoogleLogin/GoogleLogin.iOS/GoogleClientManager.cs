using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Google.SignIn;
using GoogleLogin.iOS;
using GoogleLogin.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency( typeof (GoogleClientManager))]
namespace GoogleLogin.iOS
{
    class GoogleClientManager : NSObject, IGoogleClientManager, ISignInDelegate, ISignInUIDelegate
    {
        // Class Debug Tag
        private String Tag = typeof(GoogleClientManager).FullName;
        private UIViewController ViewController { get; set; }
        static TaskCompletionSource<GoogleResponse<Models.GoogleUser>> _loginTcs;

        public GoogleClientManager()
        {
            SignIn.SharedInstance.UIDelegate = this;
            SignIn.SharedInstance.Delegate = this;
        }

        static EventHandler<GoogleClientResultEventArgs<Models.GoogleUser>> _onLogin;
        public event EventHandler<GoogleClientResultEventArgs<Models.GoogleUser>> OnLogin
        {
            add => _onLogin += value;
            remove => _onLogin -= value;
        }

        public async Task<GoogleResponse<Models.GoogleUser>> LoginAsync()
        {

            _loginTcs = new TaskCompletionSource<GoogleResponse<Models.GoogleUser>>();
            
            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            ViewController = viewController;

            SignIn.SharedInstance.SignInUser();

            return await _loginTcs.Task;

        }

        //public void Login()
        //{

        //    var window = UIApplication.SharedApplication.KeyWindow;
        //    var viewController = window.RootViewController;
        //    while (viewController.PresentedViewController != null)
        //    {
        //        viewController = viewController.PresentedViewController;
        //    }

        //    ViewController = viewController;

        //    SignIn.SharedInstance.SignInUser();
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
            SignIn.SharedInstance.SignOutUser();

            // Send the logout result to the receivers
            OnLogoutCompleted(EventArgs.Empty);
        }

        public bool IsLoggedIn { get; }

        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {

            Models.GoogleUser googleUser = null;

            if (user != null && error == null)
            {
                googleUser = new Models.GoogleUser
                {
                    Name = user.Profile.Name,
                    Email = user.Profile.Email,
                    Picture = user.Profile.HasImage
                        ? new Uri(user.Profile.GetImageUrl(500).ToString())
                        : new Uri(string.Empty)
                };
                var googleArgs =
                    new GoogleClientResultEventArgs<Models.GoogleUser>(googleUser, GoogleActionStatus.Completed, "the user is authenticated correctly");

                // Log the result of the authentication
                System.Diagnostics.Debug.WriteLine(Tag + ": Authentication " + GoogleActionStatus.Completed);

                // Send the result to the receivers
                _onLogin?.Invoke(this, googleArgs);
                _loginTcs.TrySetResult(new GoogleResponse<Models.GoogleUser>(googleArgs));
            }
            else
            {
                var googleArgs =
                    new GoogleClientResultEventArgs<Models.GoogleUser>(googleUser, GoogleActionStatus.Canceled, error?.LocalizedDescription);

                // Log the result of the authentication
                System.Diagnostics.Debug.WriteLine(Tag + ": Authentication " + GoogleActionStatus.Canceled);

                // Send the result to the receivers
                _onLogin?.Invoke(this, googleArgs);
                _loginTcs.TrySetResult(new GoogleResponse<Models.GoogleUser>(googleArgs));
            }

        }

        [Export("signIn:didDisconnectWithUser:with:Error:")]
        public void DidDisconnect(SignIn signIn, GoogleUser user, NSError error)
        {
            // Perform any operations when the user disconnects from app here.

            // Log the state of the client
            System.Diagnostics.Debug.WriteLine(Tag + ": the user has disconnected.");
        }

        [Export("signInWillDispatch:error:")]
        public void WillDispatch(SignIn signIn, NSError error)
        {
            // Stop any animations in the UI
        }

        [Export("signIn.presentViewController:")]
        public void PresentViewController(SignIn signIn, UIViewController viewController)
        {
            ViewController?.PresentViewController(viewController, true, null);
        }

        [Export("signIn:dismissViewController:")]
        public void DismissViewController(SignIn signIn, UIViewController viewController)
        {
            ViewController?.DismissViewController(true, null);
        }


    }
}