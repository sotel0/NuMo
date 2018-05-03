using System;
using CoreGraphics;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using System.Diagnostics;

/* Visualize page for iOS
 * Implements the IVisualize interface
 * Creates nutrient progress bars
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


                    //OLD
                    //using the threshold values to calculate progress bar ratio
                    //var ratio = item.Value[0] / ((minT + maxT) * dayMult);

                    var minT = item.Value[2];
                    var maxT = item.Value[3];

                    //using the dri to calculate progress bar ratio
                    //the dri to consumed nutrient ratio
                    var ratio = item.Value[0] / (item.Value[1] * dayMult);

                    //what the progress bar should be set to
                    var progress = ratio;

                    //button text
                    String nutText = "Consumed: " + Math.Round(item.Value[0], 2) + 
                            "\nDaily Recommended Intake: " +item.Value[1] * dayMult +
                            "\nMin Recommended Intake: " + Math.Round(minT,2) + 
                            "\nMax Recommended Intake: " + Math.Round(maxT,2) + 
                            "\nRatio out of DRI: " + Math.Round((ratio * 100), 2).ToString() + "%";

                    //nutrient ratio info button
                    Button button = new Button
                    {
                        Text = item.Key,
                        Font = Font.SystemFontOfSize(NamedSize.Medium),
                        WidthRequest = 185,
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
                    var lowThresh = Convert.ToSingle(minT/item.Value[1] * dayMult);
                    var maxThresh = Convert.ToSingle(maxT/item.Value[1] * dayMult);


                    var bar = new CustomProgressBar(lowThresh,maxThresh)
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
		
	}
}
