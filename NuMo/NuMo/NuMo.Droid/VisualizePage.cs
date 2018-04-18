using BarChart;
using System.Collections.Generic;
using Xamarin.Forms;
using System;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.App;

/* Visualize page for android
 * Implements the IVisualize interface
 * Creates nutrient bar graphs, and calls to get the bullseye
 */

[assembly: Xamarin.Forms.Dependency(typeof(NuMo.Droid.VisualizePage))]

namespace NuMo.Droid
{
	class VisualizePage: IVisualize
	{
        public StackLayout loadBars()
        {
            StackLayout sl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            var cpbr = new CustomProgressBar(.45f,.55f)
            {
                Progress = .4,
                WidthRequest = 175,
                Scale = 2,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            sl.Children.Add(cpbr);

            return sl;
        }

		public StackLayout loadGraphs(List<String> names, List<Double> quantities,List<Double> dris)
		{
			//stack layout to hold the graphs
			StackLayout sl = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal
			};

			//create a new list of the bars
			var data = new List<BarModel>();
			//if there are some items added in the timeframe
			if (names.Count != 0)
			{
				for (int i = 0; i < names.Count; i++)
				{
					//create a new bar for each
					var bar = new BarModel
					{
						Value = (float)((quantities[i] / dris[i]) * 100),
						Color = Android.Graphics.Color.Argb(255,65, 81, 84),
						Legend = names[i],
						ValueCaptionHidden = false,
						ValueCaption = (Math.Round((quantities[i] / dris[i]) * 100, 3)).ToString()
					};
					//if the % goes over 100, make it red
					if (bar.Value > 100)
					{
						bar.Color = Android.Graphics.Color.Red;
					}
					//they didn't want a few of the bars shown
					if (i != 27 && i != 25 && i != 23 && i != 22)
					{
						data.Add(bar);
					}
				}
			}
					//if there are some bars
			         if (data.Count > 0)
			         {
						//set the chart's source to be the bars
			             var chart = new BarChartView(Android.App.Application.Context)
			             {
			                 ItemsSource = data
						
			             };
						
						//chart customization
			             chart.MinimumValue = 0;
			             chart.MaximumValue = 100;
						 chart.GridHidden = true;
						 chart.BarWidth = 100;
			             chart.BarOffset = 200;
						
				//make the chart into a view
				Xamarin.Forms.View chart1 = chart.ToView();
				chart1.HorizontalOptions = LayoutOptions.FillAndExpand;

				//chart background
				chart.SetBackgroundColor(Android.Graphics.Color.Argb(255,235, 248, 250));
				//words underneath axis
				chart.LegendColor = Android.Graphics.Color.Black;
				//numbers on top of bars
				chart.BarCaptionOuterColor = Android.Graphics.Color.Black;
						//if they were to click on the bars
			             chart.BarClick += (sender, args) =>
			             {
			                 BarModel barC = args.Bar;
			                 Console.WriteLine("Pressed {0}", barC);

			             };
						

				//stack layout to hold the bullseye
				StackLayout slB = new StackLayout
				{
					VerticalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Vertical,
				};
				//title label
				Label label = new Label();
				label.Text = "Omega Ratio Target";
				label.Margin = new Thickness(10, 10, 0, 0);
				label.HorizontalOptions = LayoutOptions.Start; //problem because getting placed vertically...need sl w/ bullseye
				//add label to bullseye stack
				slB.Children.Add(label);
				//add bullseye graph to bullseye stack
				slB.Children.Add(new BullsEye(Android.App.Application.Context, quantities[quantities.Count - 1]));
				//add bullseye sstack layout and the nutrient graphs to the main stack layout
				sl.Children.Add(slB);
				sl.Children.Add(chart1);

			}
			//return stack layout holding everything
			return sl;

		}

		//force screen to be sideways
		public void forceLandscape()
		{
			var activity = (Activity)Forms.Context;
			activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
		}

		//resets orientation to be set by however user is holding the phone
		public void resetOrientation()
		{
			var activity = (Activity)Forms.Context;
			activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.FullSensor;

		}

	}
}
