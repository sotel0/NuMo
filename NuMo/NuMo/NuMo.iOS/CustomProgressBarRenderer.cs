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

                Control.ProgressTintColor = Color.FromRgb(182, 231, 233).ToUIColor();// Color..FromHex("#254f5e").ToUIColor();
                Control.TrackTintColor = Color.FromRgb(188, 203, 219).ToUIColor();
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
            this.Layer.CornerRadius = 10; // This is for rounded corners.
        }
    }
}