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

        public void Logout()
        {
            SignIn.SharedInstance.SignOutUser();
        }

        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            if (user != null && error == null)
            {

            }
            else
            {

            }
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