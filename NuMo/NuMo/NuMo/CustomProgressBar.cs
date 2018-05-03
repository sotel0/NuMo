using System;

namespace NuMo
{

    public class CustomProgressBar : Xamarin.Forms.ProgressBar
    {
        
        public float lowThreshold { get; set; }
        public float highThreshold { get; set; }

        public CustomProgressBar(float lowThresh, float highThresh)
        {
            lowThreshold = lowThresh;
            highThreshold = highThresh;
        }
    }
}
