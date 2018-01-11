using System;
using BarChart;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using System.Diagnostics;


/* Visualize page for iOS
 * Implements the IVisualize interface
 * Creates nutrient bar graphs, and calls to get the bullseye
 */


[assembly: Xamarin.Forms.Dependency(typeof(NuMo.iOS.VisualizePage))]
namespace NuMo.iOS
{
	class VisualizePage : IVisualize
	{

		//stack layout to hold the graphs
		public StackLayout loadGraphs(List <String> names, List <Double> quantities, List<Double> dris)
		{
			StackLayout sl = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal
				         	                                 
			};

			//create a new list of the bars
			var data = new List<BarModel>();

			//if there are some items added in the timeframe
			if(names.Count != 0){
				for (int i = 0; i < names.Count; i++)
				{
					var bar = new BarModel
					{
						Value = (float)((quantities[i] / dris[i]) * 100),
						Color = UIColor.FromRGB(65, 81, 84),
						Legend = names[i],
						ValueCaptionHidden = false,
						ValueCaption = (Math.Round((quantities[i] / dris[i]) * 100, 3)).ToString()
					};
					//if the % goes over 100, make it red
					if (bar.Value > 100)
					{
						bar.Color = UIColor.Red;
					}
					//they didn't want a few of the bars shown
					if (i != 27 && i != 25 && i != 23 && i != 22)
					{
						data.Add(bar);
					}
				}
			}

			//create the frame for the chart
			CGPoint cp = new CGPoint(100,2500); 
			CGSize cs = new CGSize(500,200); 
			CGRect cg2 = new CGRect(cp,cs);


			var chart = new BarChartView
			{
				Frame = cg2,
				BackgroundColor = UIColor.FromRGB(235, 248, 250),
				ItemsSource = data

			};

			//customization
			chart.MinimumValue = 0;
			chart.MaximumValue = 100;
			chart.GridHidden = true;

			chart.BarWidth = 50;
			chart.BarOffset = 7;

			//if they click on the chart
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
			slB.Children.Add(new BullsEye(quantities[quantities.Count - 1]));
			sl.Children.Add(slB);
			//add bullseye stack layout and nutrient graphs to the main stack layout
			sl.Children.Add(chart.ToView());
			//return stack layout holding everything
			return sl;

		}

		//force screen to be sideways
		public void forceLandscape()
		{
			UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
		}

		public void resetOrientation()
		{
			//works? needs to be tested on device
			UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientationMask.All), new NSString("orientation"));
		}

	}
}
