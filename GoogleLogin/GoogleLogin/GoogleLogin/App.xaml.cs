using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleLogin.ViewModels;
using GoogleLogin.Views;
using Plugin.GoogleClient;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly:XamlCompilation(XamlCompilationOptions.Compile)]
namespace GoogleLogin
{
	public partial class App : PrismApplication
	{
	    public App(IPlatformInitializer initializer = null) : base(initializer) { }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

	    protected override void RegisterTypes(IContainerRegistry containerRegistry)
	    {
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();

           //containerRegistry.RegisterInstance<IGoogleClientManager>(CrossGoogleClient.Current);
	    }

	    protected override void OnInitialized()
	    {
            InitializeComponent();
	        NavigationService.NavigateAsync("LoginPage");
	    }

	    protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
