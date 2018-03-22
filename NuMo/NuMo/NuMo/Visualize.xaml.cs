using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;

/* Page to display the nutrient graphs. 
 * Utilizes dependency service, because of dif. between android and iOS implementations
 */


namespace NuMo

{
    public partial class Visualize : ContentPage
    {
		List<String> names;
		List<Double> quantities;
		List<Double> dris;
        List<ProgressBar> progBars;

		public Visualize(String titleExtra, List<Nutrient> nutrientList)
		{
            InitializeComponent();

            //nutrient names
			names = new List<String>();
			//quantities consumed
			quantities = new List<Double>();
			//dri values
			dris = new List<Double>();
            //Progress Bars
            progBars = new List<ProgressBar>();

			

            Title += " " + titleExtra;

			//call to fill the names/quantities/dri lists
            getData(nutrientList);

			//force the screen to be in landscape mode
			//DependencyService.Get<IVisualize>().forceLandscape();
			//add the graphs to the main stack
			//mainVStack.Children.Add(DependencyService.Get<IVisualize>().loadGraphs(names, quantities, dris));
            //initial progress is 20%

		}

        protected override void OnAppearing() {  
            base.OnAppearing();
            animateBars();
            //await progress.ProgressTo(0.2, 1000, Easing.Linear);  
            //await sugarProgress.ProgressTo(0.45, 1000, Easing.Linear);
            //await fatProgress.ProgressTo(0.75, 1000, Easing.Linear);
        } 

        private void animateBars(){

            for (int i = 0; i < names.Count; i++)
            {

                var ratio = quantities[i] / dris[i];
                var progress = 1 - ratio / 2;
                //Nut1.Text = names[i] + ": ";
                //Nut1Data.Text = "Consumed: " + quantities[i] + "g" + "   DRI: " + dris[i] + "\nRatio: " + (ratio * 100).ToString().Substring(0, 5) + "%";

                var color = new Color();

                if (progress > 0.75)
                {
                    color = Color.Yellow;
                }
                else if (progress <= 0.75 && progress > 0.25)
                {
                    color = Color.LimeGreen;
                }
                else
                {
                    color = Color.Red;
                }


                var title = new Label
                {
                    Text = '\n' + names[i]
                };

                var nutData = new Label
                {
                    Text = "Consumed: " + quantities[i] + "   DRI: " + dris[i] + "\nRatio: " + (ratio * 100).ToString() + "%" //.Substring(0, 5) + "%"
                };

                //progBars.Add(new ProgressBar
                var bar = new ProgressBar
                {
                    Progress = 1,
                    WidthRequest = 100,
                    HeightRequest = 10,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Rotation = 180,
                    BackgroundColor = color
                };//);

                var barContentView = new ContentView
                {
                    Scale = 3,
                    Content = bar//progBars.Last()
                };

                layout.Children.Add(title);
                layout.Children.Add(nutData);
                layout.Children.Add(barContentView);

                bar.ProgressTo(progress, 1000, Easing.Linear);
            }

            foreach (var bar in progBars){
               bar.ProgressTo(0.5, 1000, Easing.Linear);
            }

            //sugarProgress.ProgressTo(0.45, 1000, Easing.Linear);
            //fatProgress.ProgressTo(0.75, 1000, Easing.Linear);
            //progress2.ProgressTo(0.33, 1000, Easing.Linear);

        }

		private void getData(List<Nutrient> nutrientList)
		{
			foreach (var item in nutrientList)
			{
				//add nutrient names to list to be passed
				names.Add(item.name);
				//add quantities to list to be passed
				quantities.Add(item.quantity);
			}
			getDRI();
		}

		//get all of the DRI information, put it into List to be passed
		public void getDRI()
		{
            var db = DataAccessor.getDataAccessor();
            
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_protein")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_totalCarbs")) );
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_dietaryFiber")) );
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_calcium") ));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_iron") ));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_magnesium") ));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_phosphorus") ));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_potassium") ));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_sodium") ));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_zinc") ));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_copper") ));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_manganese")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_selenium")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_vitaminA")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_vitaminC")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_thiamin")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_riboflavin")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_niacin")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_pantothenicAcid")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_vitaminB6")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_folate")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_vitaminB12")));
			dris.Add(0); //omega 6 total
			dris.Add(0); //omega 3 total
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_netCarbs")));
			dris.Add(0); //total sugars
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_vitaminE")));
			dris.Add(Convert.ToDouble(db.getSettingsItem("dri_vitaminK")));
			dris.Add(0); //omega 3/6 ratio

		}

		//when the user leaves this page...allow them to reset orientation
		protected override void OnDisappearing()
		{
			//DependencyService.Get<IVisualize>().resetOrientation();

		}
    }

}
