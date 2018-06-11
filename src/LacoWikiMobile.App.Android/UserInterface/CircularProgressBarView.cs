// <copyright file="CircularProgressBarView.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid.UserInterface
{
	using Android.Content;
	using Android.Graphics;
	using Android.Views;

	public class CircularProgressBarView : View
	{
		private double progress;

		private Color progressBackgroundColor;

		private Color progressColor;

		private double strokeWidth;

		public CircularProgressBarView(Context context)
			: base(context)
		{
		}

		public double Progress
		{
			get => this.progress;
			set
			{
				this.progress = value;
				Invalidate();
			}
		}

		public Color ProgressBackgroundColor
		{
			get => this.progressBackgroundColor;
			set
			{
				this.progressBackgroundColor = value;
				Invalidate();
			}
		}

		public Color ProgressColor
		{
			get => this.progressColor;
			set
			{
				this.progressColor = value;
				Invalidate();
			}
		}

		public double StrokeWidth
		{
			get => this.strokeWidth;
			set
			{
				this.strokeWidth = value;
				Invalidate();
			}
		}

		public override void Draw(Canvas canvas)
		{
			base.Draw(canvas);

			int halfWidth = Width / 2;
			int halfHeight = Height / 2;
			double halfStrokeWidth = StrokeWidth / 2;

			int radius = halfWidth < halfHeight ? halfWidth : halfHeight;

			Paint paint = new Paint
			{
				Dither = true,
				Flags = PaintFlags.AntiAlias,
				AntiAlias = true,
				StrokeWidth = (float)StrokeWidth,
			};

			paint.SetStyle(Paint.Style.Stroke);

			// Draw progress background circle
			paint.Color = ProgressBackgroundColor;
			canvas.DrawCircle(halfWidth, halfHeight, (float)(radius - halfStrokeWidth), paint);

			// Draw progress circle
			RectF rectF = new RectF();
			paint.Color = ProgressColor;

			rectF.Top = (float)(halfHeight - radius + halfStrokeWidth);
			rectF.Bottom = (float)(halfHeight + radius - halfStrokeWidth);
			rectF.Left = (float)(halfWidth - radius + halfStrokeWidth);
			rectF.Right = (float)(halfWidth + radius - halfStrokeWidth);

			canvas.DrawArc(rectF, -90, (float)(Progress * 360), false, paint);
			canvas.Save();
		}
	}
}