using System;
using Xamarin.Forms;
namespace NuMo
{
	public class App: Application
	{
		public App()
		{
            //This app will be based off of navigation pages
			MainPage = new NavigationPage(new NuMo.MainPage());
        }
        protected override void OnStart()
		{
            // Handle when your app starts
        }

        protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
            // Handle when your app resumes
        }
    }
}
