using System;
using System.Collections.Generic;


using Xamarin.Forms;

namespace NuMo
{
	public partial class MasterPage : ContentPage
	{
		public ListView ListView { get { return listView; } }

		public MasterPage()
		{
			InitializeComponent();

            //Navigation bar buttons to allow the user to easily access the NuMo pages
            var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "My Day",
				IconSource = "ic_today_black_24dp.png",
				TargetType = typeof(MyDayPage)
			});

			masterPageItems.Add(new MasterPageItem
			{
				Title = "Create Recipe",
				IconSource = "ic_local_dining_black_24dp.png",
				TargetType = typeof(CreateRecipePage)
			});

			masterPageItems.Add(new MasterPageItem
			{
				Title = "Create Food",
				IconSource = "ic_shopping_basket_black_24dp.png",
				TargetType = typeof(CreateFoodPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Settings",
				IconSource = "ic_settings_black_24dp.png",
				TargetType = typeof(SettingsPage)
			});

			masterPageItems.Add(new MasterPageItem
			{
				Title = "Dietary Reference Intakes",
				IconSource = "ic_check_box_black_24dp.png",
				TargetType = typeof(DRIPage)
			});


			listView.ItemsSource = masterPageItems;
		}
	}
}
