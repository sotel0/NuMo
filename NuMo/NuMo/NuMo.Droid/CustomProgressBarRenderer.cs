using System;
using NuMo;
using NuMo.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarRenderer))]
namespace NuMo.Droid
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        public CustomProgressBarRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            var custBar = Element as CustomProgressBar;

            //control.progress was too large
            //get threshold for each nutrient progress bar, just make individual progress bars
            if (Control.Progress > custBar.highThreshold)
            {
                Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.FromRgb(255, 0, 0).ToAndroid());
                //Control.ProgressDrawable.SetColorFilter(Color.FromRgb(255, 0, 0).ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);

            }
            else if(Control.Progress < custBar.lowThreshold)
            {
                Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.FromRgb(255, 255, 0).ToAndroid());
                //Control.ProgressDrawable.SetColorFilter(Color.FromRgb(182, 231, 233).ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);
            } else {
                
                Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.FromRgb(0, 255, 0).ToAndroid());
                //Control.ProgressDrawable.SetColorFilter(Color.FromRgb(182, 231, 233).ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);
            }

            /////Control.ProgressTintListColor.FromRgb(182, 231, 233).ToAndroid();


            Control.ScaleY = 12;

        }
    }
}