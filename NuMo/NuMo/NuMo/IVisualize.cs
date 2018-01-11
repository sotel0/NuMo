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
		//load the bullseye and nutrient graphs
		StackLayout loadGraphs(List<String> names, List<Double> quantities, List<Double> dris);
		//force the screen to go sideways
		void forceLandscape();
		//reset the orientation when you leave graph page
		void resetOrientation();
	}

}
