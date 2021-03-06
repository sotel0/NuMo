using Xamarin.Forms;
using System.Collections.Generic;
using System;

/* Interface for the visualizer page, since iOS and Android need dif. implementations
 * Code to force the orientation in a certain direction:
 * 		 http://stackoverflow.com/questions/29057972/xamarin-forms-on-ios-how-to-set-screen-orientation-for-page
 */


namespace NuMo
{
    public interface IVisualize 
	{
        //load the progress bars
        StackLayout loadBars(IDictionary<String, Double[]> items, List<String> names, List<Double> quantities, List<Double> dris, int dayMult);

	}

}
