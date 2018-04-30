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
        IDictionary<String, Double[]> items;
        int dayMultiplier;

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

            items = new Dictionary<String, Double[]>();
            initializeItems();

            //find how many days to calculate nutrients for
            Title += " " + titleExtra;
            if (titleExtra[0] == '7'){
                dayMultiplier = 7;
            } else if(titleExtra[0] == '3'){
                dayMultiplier = 30;
            } else {
                dayMultiplier = 1;
            }

            MessagingCenter.Subscribe<IVisualize, string[]>(this, "NutrientText", (sender,values) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert(values[0], values[1], "Ok");
                });
            });

			//call to fill the names/quantities/dri lists
            getData(nutrientList);

            //force the screen to be in landscape mode
            //DependencyService.Get<IVisualize>().resetOrientation();
			
            //mainVStack.Children.Add(DependencyService.Get<IVisualize>().loadGraphs(names, quantities, dris));
            //initial progress is 20%

		}

        protected override void OnAppearing() {  
            base.OnAppearing();
            //animateBars2();
            animateBars3();
        }
        private void animateBars3()
        {
            //add the graphs to the main stack
            mainStack.Children.Add(DependencyService.Get<IVisualize>().loadBars(items,names,quantities,dris,dayMultiplier));
        }

        /*private void animateBars(){

            for (int i = 0; i < names.Count; i++)
            {
                //try
                //{

                    var ratio = quantities[i] / dris[i];
                    var progress = ratio / 2;
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
                        
                        HorizontalOptions = LayoutOptions.Center,
                        Text = '\n' + names[i]
                    };

                    var nutData = new Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "Consumed: " + Math.Round(quantities[i], 2) + "   DRI: " + dris[i] + "\nRatio: " + Math.Round((ratio * 100), 2).ToString() + "%" //.Substring(0, 5) + "%"
                    };

                    color = Color.White;
                    //progBars.Add(new ProgressBar
                    var bar = new ProgressBar
                    {
                        Progress = 0,
                        WidthRequest = 100,
                        HeightRequest = 10,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Rotation = 0,
                        Margin = 10,

                    };//);

                    var barContentView = new ContentView
                    {
                        Scale = 3,
                        Content = bar//progBars.Last()
                    };

                    layoutLowStack.Children.Add(title);
                    layoutLowStack.Children.Add(nutData);
                    layoutLowStack.Children.Add(barContentView);

                    bar.ProgressTo(progress, 1000, Easing.Linear);
                //} 
                //catch(Exception){}
            }

            foreach (var bar in progBars){
               bar.ProgressTo(0.5, 1000, Easing.Linear);
            }

            //sugarProgress.ProgressTo(0.45, 1000, Easing.Linear);
            //fatProgress.ProgressTo(0.75, 1000, Easing.Linear);
            //progress2.ProgressTo(0.33, 1000, Easing.Linear);

        }
        */

		private void getData(List<Nutrient> nutrientList)
		{
            
			foreach (var item in nutrientList)
			{
				//add nutrient names to list to be passed
				names.Add(item.name);
				//add quantities to list to be passed
				quantities.Add(item.quantity);

                try
                {
                    items[item.name][0] = item.quantity;
                } catch(Exception){}
			}
			//getDRI();
		}


		//get all of the DRI information, put it into List to be passed
		public void getDRI()
		{
            var db = DataAccessor.getDataAccessor();

                dris.Add(Convert.ToDouble(db.getDRIValue("dri_protein")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_totalCarbs")));
                //dris.Add(0); // sugar

                dris.Add(Convert.ToDouble(db.getDRIValue("dri_dietaryFiber")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_calcium")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_iron")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_magnesium")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_phosphorus")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_potassium")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_sodium")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_zinc")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_copper")));

                dris.Add(Convert.ToDouble(db.getDRIValue("dri_manganese")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_selenium")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminA")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminC")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_thiamin")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_riboflavin")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_niacin")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_pantothenicAcid")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminB6")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_folate")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminB12")));
                dris.Add(0); //omega 6 total
                dris.Add(0); //omega 3 total
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_netCarbs")));
                dris.Add(0); //total sugars
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminE")));
                dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminK")));
                dris.Add(0); //omega 3/6 ratio

		}

		protected override void OnDisappearing()
		{
		}

        private void initializeItems()
        {
            var db = DataAccessor.getDataAccessor();

            //quantity = 0, dri value = 0
            double[] QDRI1 = { 0, Convert.ToDouble(db.getDRIValue("dri_protein")) }; 
            double[] QDRI2 = { 0, Convert.ToDouble(db.getDRIValue("dri_totalCarbs")) };
            double[] QDRI3 = { 0, Convert.ToDouble(db.getDRIValue("dri_calcium")) };
            double[] QDRI4 = { 0, Convert.ToDouble(db.getDRIValue("dri_iron")) };
            double[] QDRI5 = { 0, Convert.ToDouble(db.getDRIValue("dri_magnesium")) };
            double[] QDRI6 = { 0, Convert.ToDouble(db.getDRIValue("dri_phosphorus")) };
            double[] QDRI7 = { 0, Convert.ToDouble(db.getDRIValue("dri_potassium")) };
            double[] QDRI8 = { 0, Convert.ToDouble(db.getDRIValue("dri_sodium")) };
            double[] QDRI9 = { 0, Convert.ToDouble(db.getDRIValue("dri_zinc")) };
            double[] QDRI10 = { 0, Convert.ToDouble(db.getDRIValue("dri_copper")) };
            double[] QDRI11 = { 0, Convert.ToDouble(db.getDRIValue("dri_manganese")) };

            //does not exists yet, may be spelled wrong
            //double[] QDRI12 = { 0, Convert.ToDouble(db.getDRIValue("dri_total_sugar")) };
            //double[] QDRI13 = { 0, Convert.ToDouble(db.getDRIValue("dri_fiber")) };

            items.Add("Protein(g)", QDRI1);
            items.Add("Carbohydrates(g)",QDRI2);
            items.Add("Calcium(mg)", QDRI3);
            items.Add("Iron(mg)", QDRI4);
            items.Add("Magnesium(mg)", QDRI5);
            items.Add("Phosphorus(mg)", QDRI6);
            items.Add("Potassium(mg)", QDRI7);
            items.Add("Sodium(mg)", QDRI8);
            items.Add("Zinc(mg)", QDRI9);
            items.Add("Copper(mg)", QDRI10);
            items.Add("Magnanese(mg)", QDRI11);
            //items.Add("Total Sugars(g)", QDRI12);
            //items.Add("Total Dietary Fiber(g)", QDRI13);
        }


        //private void animateBars2()
        //{
            
        //    foreach(var item in items)
        //    {
        //        try
        //        {

        //            var ratio = item.Value[0] / (item.Value[1]*dayMultiplier);
        //            var progress = ratio;
        //            String nutText = "Consumed: " + Math.Round(item.Value[0], 2) + "\nDaily Recommended Intake: " + item.Value[1] * dayMultiplier + "\nRatio: " + Math.Round((ratio * 100), 2).ToString() + "%";

        //            Button button = new Button
        //            {
        //                Text = item.Key,
        //                Font = Font.SystemFontOfSize(NamedSize.Medium),
        //                WidthRequest = 170,
        //                Margin = 15,
        //                HorizontalOptions = LayoutOptions.Center,
        //                VerticalOptions = LayoutOptions.CenterAndExpand,
        //                Style = App.Current.Resources["BtnStyle"] as Style
        //            };

        //            button.Clicked += OnClicked;

        //            void OnClicked(object sender, EventArgs ea)
        //            {
        //                DisplayAlert(item.Key, nutText, "OK");
        //            }


        //            var bar = new ProgressBar
        //            {
        //                Progress = 0,
        //                WidthRequest = 100,
        //                HeightRequest = 10,
        //                VerticalOptions = LayoutOptions.Center,
        //                HorizontalOptions = LayoutOptions.Center,
        //                Rotation = 0,
        //                Margin = 10,
        //            };

        //            var barContentView = new ContentView
        //            {
        //                Scale = 3,
        //                Content = bar
        //            };

        //            //Add the nutrient to the corresponding layout

        //            //check for nutrients to add them to special category

        //            if(item.Key.Equals("Sodium(mg)")){
        //                importantStack.Children.Add(button);
        //                importantStack.Children.Add(barContentView);
        //            }
        //            else if (ratio < .70)
        //            {
        //                layoutLowStack.Children.Add(button);
        //                layoutLowStack.Children.Add(barContentView);
        //            } else if (ratio >= .70 && ratio <= 1.30){
        //                layoutMidStack.Children.Add(button);
        //                layoutMidStack.Children.Add(barContentView);
        //            } else {
        //                layoutHighStack.Children.Add(button);
        //                layoutHighStack.Children.Add(barContentView);
        //            }


        //            bar.ProgressTo(progress, 1000, Easing.Linear);
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }

        //    //label to place if there are no nutrients in the layout category
        //    if (importantStack.Children.Count == 1)
        //    {
        //        Label nothing = new Label
        //        {
        //            Text = "...",
        //            HorizontalOptions = LayoutOptions.Center,
        //            Style = App.Current.Resources["LabelStyle"] as Style
        //        };
        //        importantStack.Children.Add(nothing);
        //    }

        //    if (layoutLowStack.Children.Count == 1)
        //    {
        //        Label nothing = new Label
        //        {
        //            Text = "...",
        //            HorizontalOptions = LayoutOptions.Center,
        //            Style = App.Current.Resources["LabelStyle"] as Style
        //        };
        //        layoutLowStack.Children.Add(nothing);
        //    }

        //    if (layoutMidStack.Children.Count == 1)
        //    {
        //        Label nothing = new Label
        //        {
        //            Text = "...",
        //            HorizontalOptions = LayoutOptions.Center,
        //            Style = App.Current.Resources["LabelStyle"] as Style
        //        };
        //        layoutMidStack.Children.Add(nothing);
        //    }

        //    if(layoutHighStack.Children.Count == 1)
        //    {
        //        Label nothing = new Label
        //        {
        //            Text = "...",
        //            HorizontalOptions = LayoutOptions.Center,
        //            Style = App.Current.Resources["LabelStyle"] as Style
        //        };
        //        layoutHighStack.Children.Add(nothing);
        //    }
        //}
    }
}
