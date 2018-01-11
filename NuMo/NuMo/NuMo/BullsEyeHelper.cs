using System;
using Xamarin.Forms;

/* Helper for the Bullseye pages, code reuse for the similarities
 */

namespace NuMo
{
	public class BullsEyeHelper: ContentPage
	{
		public double OmegaRatio;

		public BullsEyeHelper(double OmegaRatio)
		{
			//pass in the omega ratio
			this.OmegaRatio = OmegaRatio;
		}

		//load informational popup about the omega6/3 ratio, based on your values.
		public async void loadPopup()
		{
			Double [] values = getPopupInfo();
			await DisplayAlert("Omega 6/3 Ratio", "Your ratio is: "+values[0]+"\nThe desired ratio is: 4\nYou have acheived "+values[1]+"% of the desired ratio.", "OK");

		}

		//calculate information that you want to display in the popup
		public double [] getPopupInfo()
		{
			double[] values = new double[2];
			Double percentage = (OmegaRatio / 4) * 100;
			percentage = Math.Round(percentage, 3);

			OmegaRatio = Math.Round(OmegaRatio, 3);
			values[0] = OmegaRatio;
			values[1] = percentage;

			return values;

		}
	}
}
