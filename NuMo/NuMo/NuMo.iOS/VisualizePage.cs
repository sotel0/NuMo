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
        public StackLayout loadBars(IDictionary<String, Double[]> items, List<String> names, List<Double> quantities, List<Double> dris, int dayMult)
        {
            StackLayout parentStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            foreach (var item in items)
            {
                try
                {
                    //encompassing stack
                    StackLayout subStack = new StackLayout
                    {
                        Padding = new Thickness(0, 0, 0, 25),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };

                    //the dri to consumed nutrient ratio
                    var ratio = item.Value[0] / (item.Value[1] * dayMult);

                    //what the progress bar should be set to
                    var progress = ratio;

                    //button text
                    String nutText = "Consumed: " + Math.Round(item.Value[0], 2) + "\nDaily Recommended Intake: " + item.Value[1] * dayMult + "\nRatio: " + Math.Round((ratio * 100), 2).ToString() + "%";

                    //nutrient ratio info button
                    Button button = new Button
                    {
                        Text = item.Key,
                        Font = Font.SystemFontOfSize(NamedSize.Medium),
                        WidthRequest = 170,
                        Margin = 15,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Style = App.Current.Resources["BtnStyle"] as Style,
                        FontAttributes = FontAttributes.Bold
                    };

                    //give click event to button
                    button.Clicked += OnClicked;

                    //printing out nutrition info
                    void OnClicked(object sender, EventArgs ea)
                    {
                        string[] dispText = { item.Key, nutText };
                        MessagingCenter.Send<IVisualize, string[]>(this, "NutrientText", dispText);
                    }

                    //create custom progress bar
                    var bar = new CustomProgressBar(.25f, .75f)
                    {
                        Progress = 0,
                        WidthRequest = 50,
                        Scale = 2,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = 10,
                    };

                    //create content view for bar
                    var barContentView = new ContentView
                    {
                        Scale = 3,
                        Content = bar
                    };

                    //add button to stack
                    subStack.Children.Add(button);

                    //add contentView/bar to stack
                    subStack.Children.Add(barContentView);

                    //add stack to parenting stack
                    parentStack.Children.Add(subStack);

                    bar.ProgressTo(progress, 1000, Easing.Linear);
                    bar.Progress = progress; //in order to re color the bar

                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: '{0}'", e);
                }
            }

            return parentStack;

        }
		//public StackLayout loadGraphs(List <String> names, List <Double> quantities, List<Double> dris)
		//{
		//	StackLayout sl = new StackLayout
		//	{
		//		HorizontalOptions = LayoutOptions.FillAndExpand,
		//		VerticalOptions = LayoutOptions.FillAndExpand,
		//		Orientation = StackOrientation.Horizontal
		//	};

		//	//create a new list of the bars
		//	var data = new List<BarModel>();

		//	//if there are some items added in the timeframe
		//	if(names.Count != 0){
		//		for (int i = 0; i < names.Count; i++)
		//		{
		//			var bar = new BarModel
		//			{
		//				Value = (float)((quantities[i] / dris[i]) * 100),
		//				Color = UIColor.FromRGB(65, 81, 84),
		//				Legend = names[i],
		//				ValueCaptionHidden = false,
		//				ValueCaption = (Math.Round((quantities[i] / dris[i]) * 100, 3)).ToString()
		//			};
		//			//if the % goes over 100, make it red
		//			if (bar.Value > 100)
		//			{
		//				bar.Color = UIColor.Red;
		//			}
		//			//they didn't want a few of the bars shown
		//			if (i != 27 && i != 25 && i != 23 && i != 22)
		//			{
		//				data.Add(bar);
		//			}
		//		}
		//	}

		//	//create the frame for the chart
		//	CGPoint cp = new CGPoint(100,2500); 
		//	CGSize cs = new CGSize(500,200); 
		//	CGRect cg2 = new CGRect(cp,cs);


		//	var chart = new BarChartView
		//	{
		//		Frame = cg2,
		//		BackgroundColor = UIColor.FromRGB(235, 248, 250),
		//		ItemsSource = data

		//	};

		//	//customization
		//	chart.MinimumValue = 0;
		//	chart.MaximumValue = 100;
		//	chart.GridHidden = true;

		//	chart.BarWidth = 50;
		//	chart.BarOffset = 7;

		//	//if they click on the chart
		//	chart.BarClick += (sender, args) =>
		//	{
		//		BarModel barC = args.Bar;
		//		Console.WriteLine("Pressed {0}", barC);

		//	};

		//	//stack layout to hold the bullseye
		//	StackLayout slB = new StackLayout
		//	{
		//		VerticalOptions = LayoutOptions.FillAndExpand,
		//		Orientation = StackOrientation.Vertical,
		//	};
		//	//title label
		//	Label label = new Label();
		//	label.Text = "Omega Ratio Target";
		//	label.Margin = new Thickness(10, 10, 0, 0);
		//	label.HorizontalOptions = LayoutOptions.Start; //problem because getting placed vertically...need sl w/ bullseye
		//	//add label to bullseye stack
		//	slB.Children.Add(label);
		//	//add bullseye graph to bullseye stack
		//	slB.Children.Add(new BullsEye(quantities[quantities.Count - 1]));
		//	sl.Children.Add(slB);
		//	//add bullseye stack layout and nutrient graphs to the main stack layout
		//	sl.Children.Add(chart.ToView());
		//	//return stack layout holding everything
		//	return sl;

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
