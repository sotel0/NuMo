using System;
using System.Collections.Generic;

using Xamarin.Forms;

// First page that comes up in app. Gets the whole thing rolling, including navigation pages and the welcome screen.

namespace NuMo
{
	public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
			InitializeComponent();
			masterPage.ListView.ItemSelected += OnItemSelected;
            NavigationPage.SetHasNavigationBar(this, false);

            masterPage.ListView.SelectedItem = null;
        }


        protected override async void OnAppearing()
        {
            //to only show the startup message the first time the app is opened
            if (Application.Current.Properties.ContainsKey("start_up"))
            {
                Application.Current.Properties["start_up"] = "false";
            }
            else
            {

                Application.Current.Properties["start_up"] = "true";

                await DisplayAlert("Welcome to NuMo", "NuMo aims to emphasize the most important nutritional " +
				                   "values for your health. \n\n A Better Diet -- A Better Life \n\n You will be redirected to the app settings, which will" +
				                   " personalize your nutritional and health goals.\n\nThe database contains \"commodity\" foods.", "OK");

                await Navigation.PushModalAsync(new NavigationPage(new SettingsPage()));
            }

        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item != null)
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}
	}
}
