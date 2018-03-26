using NuMo.DatabaseItems;
using NuMo.ItemViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NuMo
{
    /// <summary>
    /// MyDay page is the starting page for NuMo, users can select a date to view foodItems from this day as well as
    /// access all other page funcitonalities from here.
    /// </summary>
	public partial class MyDayPage : ContentPage
	{
        DateTime date;
        List<IMyDayViewItem> ViewItemList;
        Color selectedColor = Color.FromRgb(123, 199, 193);

        //Initially we only want to see info for 1 day.
        int daysToLoad = 1;

        public MyDayPage()
		{
            InitializeComponent();

            //subscribe to events to refresh the page and update a food item
            MessagingCenter.Subscribe<MyDayFoodItem>(this, "RefreshMyDay", (sender) =>
            {
                this.OnAppearing();
            });
            MessagingCenter.Subscribe<MyDayFoodItem>(this, "UpdateMyDayFoodItem", (sender) =>
            {
                UpdateMyDayFoodItem(sender);
            });

            ViewItemList = new List<IMyDayViewItem>();


            ToolbarItem plus = new ToolbarItem();
            ToolbarItem plusText = new ToolbarItem();
            plus.Icon = "ic_add_black_24dp.png";
            plusText.Text = "Add Food +";
            plus.Clicked += AddButton;

            //ToolbarItems.Add(plusText);
            ToolbarItems.Add(plus);
            

            timeLengthChoice.SelectedIndex = 0;
            //ItemsButton.BackgroundColor = selectedColor;
            date = datePicker.Date;
        }

        //Set the profile picture, default to a smiley face if one does not exist.
        void UpdateMyDayPicture()
        {
            var db = DataAccessor.getDataAccessor();
            var oldItems = db.getRemainders("ProfileImage");
            MyDayRemainderItem myDayRemainderItem = null;
            if (oldItems.Count > 0)
                myDayRemainderItem = oldItems.First<MyDayRemainderItem>();
            if (myDayRemainderItem != null)
            {
                pic.Source = myDayRemainderItem.RemainderImage.Source;
            } else {
                pic.Source = "ic_logo_24dp.png";
            }
        }

        //Update a food item's information
        async void UpdateMyDayFoodItem(MyDayFoodItem item)
        {
            AddItemUpdate update = new AddItemUpdate(item);
            await Navigation.PushAsync(update.nutrFacts);

        }

        //Plus button in top right to add a food item to today's history
        async void AddButton(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AddItemToFoodHistory(date));
        }

        //Display the food items associated with today, and back in time to the number of selected days.
        void OnItemsClicked()
        {
            
            Image pic2 = new Image();
            if (Application.Current.Properties.ContainsKey("Profile Pic"))
            {
                pic2 = Application.Current.Properties["Profile Pic"] as Image;
                pic.Source = pic2.Source;
            }

            listView.BeginRefresh();
            listView.ItemsSource = null;
            var db = DataAccessor.getDataAccessor();
            ViewItemList.Clear();
            var remainderList = new List<MyDayRemainderItem>();
            for (int i = 0; i < daysToLoad; i++)
            {
                remainderList = db.getRemainders(date.AddDays(-i).ToString());
                foreach (var item in remainderList)
                {
                    ViewItemList.Add(item);
                }
            }
            for (int i = 0; i < daysToLoad; i++)
            {
                var baseList = db.getFoodHistory(date.AddDays(-i).ToString());

                foreach (var item in baseList)
                {
                    ViewItemList.Add(item);
                }
            }
            listView.ItemsSource = ViewItemList;
            listView.EndRefresh();
        }

        //Ensure only Items OR nutrients is selected, never both.
        void viewToggle(object sender, EventArgs args)
        {
            var button = (Button)sender;
            //button.BackgroundColor = selectedColor;

            if(button == NutrientsButton)
            {
                OnNutrientsClicked();
            }
            else
            {
                OnItemsClicked();
            }
        }

        //Display nutrient info for the day/history range selected.
        void OnNutrientsClicked()
        {
            //ItemsButton.BackgroundColor = Color.Default;
            //NutrientsButton.BackgroundColor = selectedColor;

            listView.BeginRefresh();
            listView.ItemsSource = null;

            var nutrientList = getNutrients(); 
            ViewItemList.Clear();
            foreach (var item in nutrientList)
            {
                ViewItemList.Add(item);
            }
            listView.ItemsSource = ViewItemList;
            listView.EndRefresh();
        }

        //Open a page of graphs to see how a user is doing compared to their goals.
        async void OnVisualizeClicked(object sender, EventArgs args)
        {
			await Navigation.PushAsync(new Visualize(this.Title + ": " +date.Month + "/" + date.Day + "/" + date.Year, getNutrients()));
        }

        //Get all the nutrients associated with the current selection of days.
        private List<Nutrient> getNutrients()
        {
            var db = DataAccessor.getDataAccessor();
            var baseList = new List<FoodHistoryItem>();
            for (int i = 0; i < daysToLoad; i++)
            {
                baseList.AddRange(db.getFoodHistoryList(date.AddDays(-i).ToString()));
            }
            var nutrientList =  db.getNutrientsFromHistoryList(baseList);

            foreach (var item in nutrientList)
            {
                if (item.name != "Omega6/3Ratio")
                    item.quantity /= daysToLoad;
            }
            return nutrientList;
        }

        //Open a page to create a new remainder item for the selected day.
        async void OnReminderClicked(object sender, EventArgs args)
        {
            //Go to camera
            await Navigation.PushAsync(new CameraPage(this.date.ToString()));
        }

        //Allow user to update the current date
        void dateClicked(object sender, DateChangedEventArgs e)
        {
            date = e.NewDate;
			this.OnAppearing();
        }

        //Allow user to update their choice of time range
        void OnTimeLengthChoiceChanged(object sender, EventArgs e)
        {
            this.Title = timeLengthChoice.Items.ElementAt(timeLengthChoice.SelectedIndex);
            if(this.Title == "One Day Report")
            {
                daysToLoad = 1;
            }
            else if(this.Title == "7 Day Report")
            {
                daysToLoad = 7;
            }
            else if(this.Title == "30 Day Report")
            {
                daysToLoad = 30;
            }
            this.OnAppearing();
        }

        //Make sure the most current information is being used.
        protected override void OnAppearing()
        {
            UpdateMyDayPicture();
            base.OnAppearing();
            OnItemsClicked();
        }
    }
}
