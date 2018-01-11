using UIKit;
using CoreGraphics;
using System;

/* View that creates the Bullseye graph for iOS
 */

namespace NuMo.iOS
{
	public class BullsEye: UIView
	{
		//end angle -> 2pi for full circle
		public float endAngle;
		public double OmegaRatio;

		public BullsEye(Double ratio)
		{
			//omega ratio passed in 
			this.OmegaRatio = ratio;

			//view
			this.Frame = new CGRect(50, 50, 225, 225);
			endAngle = (float)(System.Math.PI * 2.0);
			this.BackgroundColor = UIColor.White;

			//to recognize when user has tapped on the bullseye
			var tap = new UITapGestureRecognizer(tapped);
			this.AddGestureRecognizer(tap);


		}

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			//get graphics context
			using (CGContext g = UIGraphics.GetCurrentContext())
			{
				g.BeginPath();

				//outer circle
				UIColor.FromRGB(65, 81, 84).SetStroke();
				g.SetLineWidth(50);
				g.AddArc(110, 110, 80, 0, endAngle, true);
				g.StrokePath();

				//second circle
				UIColor.FromRGB(123, 199, 193).SetStroke();
				g.SetLineWidth(50);
				g.AddArc(110, 110, 50, 0, endAngle, true); //x,y,radius,start angle, end angle, clockwise
				g.StrokePath();

				//inner goal circle
				UIColor.FromRGB(65, 81, 84).SetStroke();
				g.SetLineWidth(50);
				g.AddArc(110, 110, 25, 0, endAngle, true);
				g.StrokePath();

				//middle circle
				UIColor.FromRGB(186, 227, 221).SetStroke();
				g.SetLineWidth(70);
				g.AddArc(110, 110, 15, 0, endAngle, true);
				g.StrokePath();

				if (OmegaRatio <= 4)
				{
					//goal arrow
					UIColor.Red.SetStroke();
					g.SetLineWidth(12);

					CGPoint p = new CGPoint(10, 10);
					CGPoint p2 = new CGPoint(100, 100);
					CGPoint[] points = { p, p2 };
					g.StrokeLineSegments(points);
					g.ClosePath();

					//for the triangle arrowhead
					g.SetLineWidth(4);
					UIColor.Red.SetFill();
					CGPath path = new CGPath();

					path.AddLines(new CGPoint[]{
					new CGPoint(95,110), //95,125
					new CGPoint(105,93), //105,108
					new CGPoint(115,110)}); //115,125

					path.CloseSubpath();
					g.AddPath(path);
					g.DrawPath(CGPathDrawingMode.FillStroke);
				}
				//lightest circle, one out from bullseye
				else if (OmegaRatio > 4 && OmegaRatio <= 15)
				{
					//goal arrow
					UIColor.Red.SetStroke();
					g.SetLineWidth(12);

					CGPoint p = new CGPoint(10, 10);
					CGPoint p2 = new CGPoint(78, 78);
					CGPoint[] points = { p, p2 };
					g.StrokeLineSegments(points);
					g.ClosePath();

					//for the triangle arrowhead
					g.SetLineWidth(4);
					UIColor.Red.SetFill();
					CGPath path = new CGPath();

					path.AddLines(new CGPoint[]{
					new CGPoint(74,86), //95,125
					new CGPoint(82,72.5), //105,108
					new CGPoint(90,85.8)}); //115,125

					path.CloseSubpath();
					g.AddPath(path);
					g.DrawPath(CGPathDrawingMode.FillStroke);

				}
				//second from outer
				else if (OmegaRatio > 15 && OmegaRatio <= 30)
				{
					//goal arrow
					UIColor.Red.SetStroke();
					g.SetLineWidth(10);

					CGPoint p = new CGPoint(10, 10);
					CGPoint p2 = new CGPoint(60, 60);
					CGPoint[] points = { p, p2 };
					g.StrokeLineSegments(points);
					g.ClosePath();

					//for the triangle arrowhead
					g.SetLineWidth(4);
					UIColor.Red.SetFill();
					CGPath path = new CGPath();

					path.AddLines(new CGPoint[]{
						new CGPoint(57,66), //95,110
						new CGPoint(63,55.8), //105,93
						new CGPoint(69,66)}); //115,110

					path.CloseSubpath();
					g.AddPath(path);
					g.DrawPath(CGPathDrawingMode.FillStroke);


				}
				//outer circle
				else if (OmegaRatio > 30)
				{
					//goal arrow
					UIColor.Red.SetStroke();
					g.SetLineWidth(8);

					CGPoint p = new CGPoint(10, 10);
					CGPoint p2 = new CGPoint(40, 40);
					CGPoint[] points = { p, p2 };
					g.StrokeLineSegments(points);
					g.ClosePath();

					//for the triangle arrowhead
					g.SetLineWidth(4);
					UIColor.Red.SetFill();
					CGPath path = new CGPath();

					path.AddLines(new CGPoint[]{
					new CGPoint(38,44), //95,110
					new CGPoint(42,37.2), //105,93
					new CGPoint(46,44)}); //115,110

					path.CloseSubpath();
					g.AddPath(path);
					g.DrawPath(CGPathDrawingMode.FillStroke);

				}

			}
			
		}
		//user taps on the bullseye
		public void tapped()
		{
			BullsEyeHelper bs = new BullsEyeHelper(OmegaRatio);
			//not working in iOS?
				//bs.loadPopup();

			Double[] values = bs.getPopupInfo();

			UIAlertView alert = new UIAlertView()
			{
				Title = "Omega 6/3 Ratio",
				Message = "Your ratio is: " +values[0]+ "\nThe desired ratio is: 4\nYou have acheived " +values[1]+ "% of the desired ratio."
			};
			alert.AddButton("OK");
			alert.Show();
		}
	}
}
