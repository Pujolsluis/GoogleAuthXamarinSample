using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleLogin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoogleLogin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            BindingContext = new LoginPageViewModel();

		    this.Title = "Home";
		}
	}
}