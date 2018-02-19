using System.Diagnostics;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Media;
using NuMo.ItemViews;
using System.Linq;

/* This is the page where the user's will enter their settings.
 * They are forced here the first time they open the app.
 * They are not allowed to leave unless certain things are filled in
 * These settings affect the DRI values
 */

namespace NuMo
{
	public partial class SettingsPage : ContentPage
	{
        public Image MainImage = new Image();
        public SettingsPage()
		{
			InitializeComponent();
            //load the saved settings 
            //(for now just ones for DRI)

            var db = DataAccessor.getDataAccessor();
            //name
		    this.FindByName<EntryCell>("settings_name").Text = db.getSettingsItem("dri_name");

			//gender
            String gender = db.getSettingsItem("dri_gender");
			//male
			if (gender.Equals("1"))
			{
				this.FindByName<Picker>("settings_gender").SelectedIndex = 1;
			}
			//female 0
			else {
				this.FindByName<Picker>("settings_gender").SelectedIndex = 0;
			}

			//age
            this.FindByName<EntryCell>("settings_age").Text = db.getSettingsItem("dri_age");

			//height

			//pregnant? 1 true 0 false
            String pregnant = db.getSettingsItem("dri_pregnant");
			if (pregnant == "1")
			{
				this.FindByName<SwitchCell>("settings_pregnant").On = true;
			}
			else {
				this.FindByName<SwitchCell>("settings_pregnant").On = false;
			}

			//lactating? 1 true 0 false
            String lactating = db.getSettingsItem("dri_lactating");
			if (lactating == "1")
			{
				this.FindByName<SwitchCell>("settings_lactating").On = true;
			}
			else {
				this.FindByName<SwitchCell>("settings_lactating").On = false;
			}

			//Health Concerns
            String blood_pressure = db.getSettingsItem("settings_blood_pressure");
			if (blood_pressure == "1")
			{
				this.FindByName<SwitchCell>("settings_blood_pressure").On = true;
			}
			else {
				this.FindByName<SwitchCell>("settings_blood_pressure").On = false;
			}
            
            String t2d = db.getSettingsItem("settings_t2d");
			if (t2d == "1")
			{
				this.FindByName<SwitchCell>("settings_t2d").On = true;
			}
			else {
				this.FindByName<SwitchCell>("settings_t2d").On = false;
			}

            //Reload photo here
            var oldItems = db.getRemainders("ProfileImage");
            MyDayRemainderItem myDayRemainderItem = null;
            if (oldItems.Count > 0)
                myDayRemainderItem = oldItems.First<MyDayRemainderItem>();
            if (myDayRemainderItem != null)
            {
                profilePic.Source = myDayRemainderItem.RemainderImage.Source;
            }
        }

        private async void PickPhotoButton_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Oops", "Pick photo is not supported!", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            MainImage.Source = ImageSource.FromStream(() => file.GetStream());
            profilePic.Source = MainImage.Source;
        }

        //called when user clicks "save"
		public async void onSaveClicked(object sender, EventArgs e)
		{
			//error string
			String needed = "Please enter: ";

			//check to see if necessary fields have been input

			//didn't enter a name :(
			if (this.FindByName<EntryCell>("settings_name").Text == null)
			{
				needed += "\nName";
			}
			else {
				if (this.FindByName<EntryCell>("settings_name").Text.Equals(""))
				{
					needed += "\nName";
				}
			}

			//didn't select a gender ;(
			if (this.FindByName<Picker>("settings_gender").SelectedIndex == -1)
			{
				needed += "\nGender";
			}

			//Error checking

			//0<age<100
			String age = this.FindByName<EntryCell>("settings_age").Text;

			//if they entered an age....
			if (age != "" && age != null)
			{
				int ageNum = int.Parse(age);
				//must be in a certain age range, else error
				if (ageNum < 0 || ageNum >= 100)
				{
					needed += "\nAge >= 0 and <=99";
				}
			}
			//they didn't enter an age :(
			else {
				needed += "\nAge >= 0 and <=99";
			}
				
			//if everything has been input, go back to the main page
			if (needed.Equals("Please enter: ")){

                var db = DataAccessor.getDataAccessor();
                //save the info

                db.saveSettingsItem("dri_name", this.FindByName<EntryCell>("settings_name").Text);
				db.saveSettingsItem("dri_age", this.FindByName<EntryCell>("settings_age").Text);
                String gender = "0";
				if (this.FindByName<Picker>("settings_gender").SelectedIndex == 1)
				{
					gender = "1";
				}
				db.saveSettingsItem("dri_gender", gender);

				if (this.FindByName<SwitchCell>("settings_pregnant").On == true)
				{
					db.saveSettingsItem("dri_pregnant", "1");
				}
				else
                {
                    db.saveSettingsItem("dri_pregnant", "0");
                }

				if (this.FindByName<SwitchCell>("settings_lactating").On == true)
				{
					db.saveSettingsItem("dri_lactating", "1");
				}
				else
                {
                    db.saveSettingsItem("dri_lactating", "0");
                }
                if(MainImage.Source != null)
                {
                    var remainderItem = new MyDayRemainderItem();
                    remainderItem.Date = "ProfileImage";
                    remainderItem.RemainderImage = new Image();
                    remainderItem.RemainderImage.Source = MainImage.Source;
                    var oldItems = db.getRemainders("ProfileImage");
                    MyDayRemainderItem oldItem = null;
                    if(oldItems.Count > 0)
                        oldItem = oldItems.First<MyDayRemainderItem>(i => i.Date =="ProfileImage");
                    if(oldItem != null)
                    {
                        db.deleteFoodHistoryItem(oldItem.id);
                    }
                    db.insertRemainder(remainderItem);
                }
                
                MessagingCenter.Send(new MyDayFoodItem(), "RefreshMyDay");
                try
                {
                    await Navigation.PopModalAsync();//only used the first run, will error every other time.
					DRIPage driP = new DRIPage();
					driP.saveNoLoad();
                }
                catch(Exception)
                {
                    
                }

                //display settings were saved
                await DisplayAlert("Settings Saved", "", "OK");
            }
			//display the error message
			else {
				await DisplayAlert("Error", needed, "OK");
			    }


		}
	}
}
