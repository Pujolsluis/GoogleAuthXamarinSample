using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.GoogleClient;
using Prism;
using Prism.Ioc;

namespace GoogleLogin.Droid
{
    [Activity(Label = "Google Auth ", Icon = "@drawable/ic_launcher_crossgeeks", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
            GoogleClientManager.Initialize(this);

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, data);
        }

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                containerRegistry.RegisterInstance<IGoogleClientManager>(CrossGoogleClient.Current);
            }
        }
    }
}

