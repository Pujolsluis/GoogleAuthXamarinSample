using System;

using Android.App;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace GoogleLogin.Droid
{
    [Activity(Label = "GoogleLogin", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private GoogleClientManager GoogleManager;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            GoogleManager = new GoogleClientManager();
            GoogleClientManager.Initialize(this);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                GoogleManager.OnAuthCompleted(result);
            }
        }
    }
}

