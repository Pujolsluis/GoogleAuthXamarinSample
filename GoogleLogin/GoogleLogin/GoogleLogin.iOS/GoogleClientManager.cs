using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public event EventHandler OnLogout;
        private UIViewController ViewController { get; set; }
        private String Tag = typeof(GoogleClientManager).FullName;


        public GoogleClientManager()
        {
            SignIn.SharedInstance.UIDelegate = this;
            SignIn.SharedInstance.Delegate = this;
        }

        static EventHandler<LoginEventArgs> _onLogin;
        public event EventHandler<LoginEventArgs> OnLogin
        {
            add => _onLogin += value;
            remove => _onLogin -= value;
        }

        public void Login()
        {

            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            ViewController = viewController;

            SignIn.SharedInstance.SignInUser();
        }

        protected virtual void OnLoginCompleted(LoginEventArgs e)
        {
            _onLogin?.Invoke(this, e);
        }

        public void Logout()
        {
            SignIn.SharedInstance.SignOutUser();
        }

        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            LoginEventArgs args = new LoginEventArgs();

            // Assume the authentication failed
            args.Message = "Authentication Failed";

            // Log the result of the authentication
            System.Diagnostics.Debug.WriteLine(Tag + ": Authentication Failed");

            if (user != null && error == null)
            {
                args.User = new Models.GoogleUser
                {
                    Name = user.Profile.Name,
                    Email = user.Profile.Email,
                    Picture = user.Profile.HasImage
                        ? new Uri(user.Profile.GetImageUrl(500).ToString())
                        : new Uri(string.Empty)
                };
            }
            else
            {
                args.Message = "The authentication was a success!";
            }

            // Send the result to the receivers
            OnLoginCompleted(args);
        }

        [Export("signIn:didDisconnectWithUser:with:Error:")]
        public void DidDisconnect(SignIn signIn, GoogleUser user, NSError error)
        {
            // Perform any operations when the user disconnects from app here.
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