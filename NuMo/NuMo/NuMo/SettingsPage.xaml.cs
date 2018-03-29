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
            //not all of the settings are currently being used for calculations

            //load the saved settings
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

            //weight
            this.FindByName<EntryCell>("settings_weight").Text = db.getSettingsItem("dri_weight");

			//height
            this.FindByName<EntryCell>("settings_feet").Text = db.getSettingsItem("dri_feet");
            this.FindByName<EntryCell>("settings_inches").Text = db.getSettingsItem("dri_inches");

            //Health Concerns
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

            //high blood pressure
            String blood_pressure = db.getSettingsItem("dri_blood_pressure");
			if (blood_pressure == "1")
			{
				this.FindByName<SwitchCell>("settings_blood_pressure").On = true;
			}
			else {
				this.FindByName<SwitchCell>("settings_blood_pressure").On = false;
			}

            //type 2 diabetes
            String t2d = db.getSettingsItem("dri_t2d");
			if (t2d == "1")
			{
				this.FindByName<SwitchCell>("settings_t2d").On = true;
			}
			else {
				this.FindByName<SwitchCell>("settings_t2d").On = false;
			}

            //gluten sensitvity
            String gluten_sensitivity = db.getSettingsItem("dri_gluten_sensitivity");
            if (gluten_sensitivity == "1")
            {
                this.FindByName<SwitchCell>("settings_gluten_sensitivity").On = true;
            }
            else
            {
                this.FindByName<SwitchCell>("settings_gluten_sensitivity").On = false;
            }

            //cardiovascular disease
            String cvd = db.getSettingsItem("dri_cvd");
            if (cvd == "1")
            {
                this.FindByName<SwitchCell>("settings_cvd").On = true;
            }
            else
            {
                this.FindByName<SwitchCell>("settings_cvd").On = false;
            }

            //liver disease
            String liver_disease = db.getSettingsItem("dri_liver_disease");
            if (liver_disease == "1")
            {
                this.FindByName<SwitchCell>("settings_liver_disease").On = true;
            }
            else
            {
                this.FindByName<SwitchCell>("settings_liver_disease").On = false;
            }

            //liver disease
            String kidney_disease = db.getSettingsItem("dri_kidney_disease");
            if (kidney_disease == "1")
            {
                this.FindByName<SwitchCell>("settings_kidney_disease").On = true;
            }
            else
            {
                this.FindByName<SwitchCell>("settings_kidney_disease").On = false;
            }

            //liver disease
            String sibo = db.getSettingsItem("dri_sibo");
            if (sibo == "1")
            {
                this.FindByName<SwitchCell>("settings_sibo").On = true;
            }
            else
            {
                this.FindByName<SwitchCell>("settings_sibo").On = false;
            }

            //maximize macro balance
            String macro_balance = db.getSettingsItem("dri_macro_balance");
            if (macro_balance == "1")
            {
                this.FindByName<SwitchCell>("settings_macro_balance").On = true;
            }
            else
            {
                this.FindByName<SwitchCell>("settings_macro_balance").On = false;
            }

            //maximize weight loss
            String weight_loss = db.getSettingsItem("dri_weight_loss");
            if (weight_loss == "1")
            {
                this.FindByName<SwitchCell>("settings_weight_loss").On = true;
            }
            else
            {
                this.FindByName<SwitchCell>("settings_weight_loss").On = false;
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

			//didn't enter a name
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

			//didn't select a sex ;(
			if (this.FindByName<Picker>("settings_gender").SelectedIndex == -1)
			{
				needed += "\nSex";
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
			//they didn't enter an age
			else {
				needed += "\nAge >= 0 and <=99";
			}

            //0<=feet<=10
            String feet = this.FindByName<EntryCell>("settings_feet").Text;
            //if they entered any feet
            if (feet != "" && feet != null)
            {
                int feetNum = int.Parse(feet);
                //must be in a certain range, else error
                if (feetNum < 0 || feetNum > 10)
                {
                    needed += "\nFeet >= 0 and <=10";
                }
            }
            //they didn't enter any feet
            else
            {
                needed += "\nFeet >= 0 and <=10";
            }

            //0<=inches<=11
            String inches = this.FindByName<EntryCell>("settings_inches").Text;
            //if they entered any inches
            if (inches != "" && inches != null)
            {
                int inchesNum = int.Parse(inches);
                //must be in a certain range, else error
                if (inchesNum < 0 || inchesNum > 11)
                {
                    needed += "\nInches >= 0 and <=11";
                }
            }
            //they didn't enter any inches
            else
            {
                needed += "\nInches >= 0 and <=11";
            }

            //non negative weight
            String weight = this.FindByName<EntryCell>("settings_weight").Text;
            //if they entered any inches
            if (weight != "" && weight != null)
            {
                int weightNum = int.Parse(weight);
                //must be in a certain range, else error
                if (weightNum < 0)
                {
                    needed += "\nWeight >= 0";
                }
            }
            //they didn't enter any weight
            else
            {
                needed += "\nWeight >= 0";
            }

            //using "Please enter: " string to check if there are no errors
			//if everything has been input, go back to the main page
			if (needed.Equals("Please enter: ")){

                var db = DataAccessor.getDataAccessor();

                //saving the user settings
                db.saveSettingsItem("dri_name", this.FindByName<EntryCell>("settings_name").Text);
				db.saveSettingsItem("dri_age", this.FindByName<EntryCell>("settings_age").Text);
                db.saveSettingsItem("dri_weight", this.FindByName<EntryCell>("settings_weight").Text);
                db.saveSettingsItem("dri_feet", this.FindByName<EntryCell>("settings_feet").Text);
                db.saveSettingsItem("dri_inches", this.FindByName<EntryCell>("settings_inches").Text);

                //saving sex in the db by whichever value is selected
				if (this.FindByName<Picker>("settings_gender").SelectedIndex == 1)
				{
                    db.saveSettingsItem("dri_gender", "1");
                } else {
                    db.saveSettingsItem("dri_gender", "0");
                }
				
                //saving pregnant toggle in the db, by whichever pregnant value is selected
				if (this.FindByName<SwitchCell>("settings_pregnant").On == true)
				{
					db.saveSettingsItem("dri_pregnant", "1");
				} else {
                    db.saveSettingsItem("dri_pregnant", "0");
                }

				if (this.FindByName<SwitchCell>("settings_lactating").On == true)
				{
					db.saveSettingsItem("dri_lactating", "1");
				} else {
                    db.saveSettingsItem("dri_lactating", "0");
                }

                if (this.FindByName<SwitchCell>("settings_blood_pressure").On == true)
                {
                    db.saveSettingsItem("dri_blood_pressure", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_blood_pressure", "0");
                }

                if (this.FindByName<SwitchCell>("settings_t2d").On == true)
                {
                    db.saveSettingsItem("dri_t2d", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_t2d", "0");
                }

                if (this.FindByName<SwitchCell>("settings_gluten_sensitivity").On == true)
                {
                    db.saveSettingsItem("dri_gluten_sensitivity", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_gluten_sensitivity", "0");
                }
                if (this.FindByName<SwitchCell>("settings_cvd").On == true)
                {
                    db.saveSettingsItem("dri_cvd", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_cvd", "0");
                }
                if (this.FindByName<SwitchCell>("settings_liver_disease").On == true)
                {
                    db.saveSettingsItem("dri_liver_disease", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_liver_disease", "0");
                }
                if (this.FindByName<SwitchCell>("settings_kidney_disease").On == true)
                {
                    db.saveSettingsItem("dri_kidney_disease", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_kidney_disease", "0");
                }
                if (this.FindByName<SwitchCell>("settings_sibo").On == true)
                {
                    db.saveSettingsItem("dri_sibo", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_sibo", "0");
                }
                if (this.FindByName<SwitchCell>("settings_macro_balance").On == true)
                {
                    db.saveSettingsItem("dri_macro_balance", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_macro_balance", "0");
                }
                if (this.FindByName<SwitchCell>("settings_weight_loss").On == true)
                {
                    db.saveSettingsItem("dri_weight_loss", "1");
                }
                else
                {
                    db.saveSettingsItem("dri_weight_loss", "0");
                }


                //try and save profile photo
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
