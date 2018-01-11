using System;
using Android.Views;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Graphics.Drawables.Shapes;

/* View that creates the Bullseye graph for Android
 */

namespace NuMo.Droid
{


	public class BullsEye : View, Android.Views.View.IOnClickListener
	{

		//second circle (from outside)
		public ShapeDrawable shape;
		public Paint paint;

		//third circle (from outside)
		public ShapeDrawable shape2;
		public Paint paint2;

		//inner circle
		public ShapeDrawable shape3;
		public Paint paint3;

		//arrow line
		public ShapeDrawable shape4;
		public Paint paint4;

		//outer circle
		public ShapeDrawable shape5;
		public Paint paint5;

		//arrowhead
		public ShapeDrawable shape6;
		public Paint paint6;


		public Double OmegaRatio;

		public BullsEye(Context context, Double ratio) : base(context)
		{
			this.OmegaRatio = ratio;
			//the view
			SetMinimumHeight(500);
			SetMinimumWidth(500);
			Clickable = true;
			SetOnClickListener(this);

			//the outer circle
			paint5 = new Paint();
			paint5.SetARGB(255, 65, 81, 84);
			paint5.SetStyle(Paint.Style.Stroke);
			paint5.StrokeWidth = 50;

			shape5 = new ShapeDrawable(new OvalShape());
			shape5.Paint.Set(paint5);

			//the second circle
			paint = new Paint();
			paint.SetARGB(255, 123, 199, 193);
			paint.SetStyle(Paint.Style.Stroke);
			paint.StrokeWidth = 50;

			shape = new ShapeDrawable(new OvalShape());
			shape.Paint.Set(paint);

			//the third circle
			paint2 = new Paint();
			paint2.SetARGB(255, 186, 227, 221);
			paint2.SetStyle(Paint.Style.Stroke);
			paint2.StrokeWidth = 50;

			shape2 = new ShapeDrawable(new OvalShape());
			shape2.Paint.Set(paint2);

			//the inner goal circle
			paint3 = new Paint();
			paint3.SetARGB(255, 65, 81, 84);
			paint3.SetStyle(Paint.Style.Stroke);
			paint3.StrokeWidth = 40;

			shape3 = new ShapeDrawable(new OvalShape());
			shape3.Paint.Set(paint3);

			//line for where the user is at
			paint4 = new Paint();
			//paint4.SetARGB(255, 235, 248, 250); //light blue color...but looks really light w/ white background
			paint4.SetARGB(255, 244, 66, 69);
			paint4.SetStyle(Paint.Style.Stroke);
			paint4.StrokeWidth = 30;

			shape4 = new ShapeDrawable(new OvalShape());
			shape4.Paint.Set(paint4);

			//draw a triangle at the end of the line
			paint6 = new Paint();
			//paint4.SetARGB(255, 235, 248, 250);
			paint6.SetARGB(255, 244, 66, 69);
			paint6.SetStyle(Paint.Style.Stroke);
			paint6.StrokeWidth = 30;

			shape6 = new ShapeDrawable(new OvalShape());
			shape.Paint.Set(paint6);
		}


		protected override void OnDraw(Canvas canvas)
		{
			//draw the circles
			canvas.DrawCircle(225, 225, 150, paint5);  //150
			canvas.DrawCircle(225, 225, 100, paint);  //100
			canvas.DrawCircle(225, 225, 50, paint2);  //50
			canvas.DrawCircle(225, 225, 15, paint3);  //15

			//triangle
			Path path = new Path();
			path.MoveTo(200, 190); //val, -10
			path.LineTo(205, 200); //+5, val
			path.LineTo(195, 200); //-5, val
			path.Close();

			if (OmegaRatio <= 4)
			{
				canvas.DrawLine(10, 10, 200, 200, paint4);
				canvas.DrawPath(path, paint6);
			}
			else if (OmegaRatio > 4 && OmegaRatio <=15)
			{
				canvas.DrawLine(10, 10, 170, 170, paint4);
				path.Offset(-30, -30);
				canvas.DrawPath(path, paint6);
			}
			else if (OmegaRatio > 15 && OmegaRatio <= 30)
			{
				canvas.DrawLine(10, 10, 125, 125, paint4);
				path.Offset(-65, -65);
				canvas.DrawPath(path, paint6);
			}

			else if (OmegaRatio > 30)
			{
				canvas.DrawLine(10, 10, 100, 100, paint4);
				path.Offset(-100, -100);
				canvas.DrawPath(path, paint6);

			}
		}

		//when they click on the graph...popup with more info
		public void OnClick(View v)
		{
			BullsEyeHelper bs = new BullsEyeHelper(OmegaRatio);
			bs.loadPopup();
		}
	}
}

