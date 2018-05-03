using CoreGraphics;
using NuMo;
using NuMo.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarRenderer))]
namespace NuMo.iOS
{

    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(
         ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            
            base.OnElementChanged(e);

            var custBar = Element as CustomProgressBar;
            if (Control != null && custBar != null)
            {
                if (Control.Progress > custBar.highThreshold)
                {
                    Control.ProgressTintColor = Color.FromRgb(255, 0, 0).ToUIColor();

                }
                else if (Control.Progress < custBar.lowThreshold)
                {
                    Control.ProgressTintColor = Color.FromRgb(255, 255, 0).ToUIColor();
                }
                else
                {
                    Control.ProgressTintColor = Color.FromRgb(0, 255, 0).ToUIColor();
                }
                Control.TrackTintColor = Color.FromRgb(188, 203, 219).ToUIColor();
            }
        }


        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var X = 1.0f;
            var Y = 12.0f;

            CGAffineTransform transform = CGAffineTransform.MakeScale(X, Y);
            this.Transform = transform;

            this.ClipsToBounds = true;
            this.Layer.MasksToBounds = true;
            this.Layer.CornerRadius = 2; // This is for rounded corners.
        }
    }
}