// <copyright file="CircularProgressBarView.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS.UserInterface
{
	using System;
	using CoreGraphics;
	using UIKit;

	public class CircularProgressBarView : UIView
	{
		private double progress;

		private UIColor progressBackgroundColor;

		private UIColor progressColor;

		private double strokeWidth;

		public CircularProgressBarView()
			: base()
		{
			BackgroundColor = UIColor.Clear;
		}

		public double Progress
		{
			get => this.progress;
			set
			{
				this.progress = value;
				SetNeedsDisplay();
			}
		}

		public UIColor ProgressBackgroundColor
		{
			get => this.progressBackgroundColor;
			set
			{
				this.progressBackgroundColor = value;
				SetNeedsDisplay();
			}
		}

		public UIColor ProgressColor
		{
			get => this.progressColor;
			set
			{
				this.progressColor = value;
				SetNeedsDisplay();
			}
		}

		public double StrokeWidth
		{
			get => this.strokeWidth;
			set
			{
				this.strokeWidth = value;
				SetNeedsDisplay();
			}
		}

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);

			float halfWidth = (float)Bounds.Width / 2.0f;
			float halfHeight = (float)Bounds.Height / 2.0f;
			float halfStrokeWidth = (float)StrokeWidth / 2.0f;

			float radius = halfWidth < halfHeight ? halfWidth : halfHeight;

			using (CGContext context = UIGraphics.GetCurrentContext())
			{
				context.SetLineWidth((float)StrokeWidth);

				// Draw progress background circle
				CGPath path = new CGPath();
				ProgressBackgroundColor.SetStroke();

				path.AddArc(halfWidth, halfHeight, radius - halfStrokeWidth, 0, (float)(2.0 * Math.PI), true);
				context.AddPath(path);
				context.DrawPath(CGPathDrawingMode.Stroke);

				// Draw progress circle
				CGPath pathProgress = new CGPath();
				ProgressColor.SetStroke();
				pathProgress.AddArc(halfWidth, halfHeight, radius - halfStrokeWidth, (float)(-0.5 * Math.PI), (float)((-0.5 * Math.PI) + (Progress * Math.PI * 2)), false);
				context.AddPath(pathProgress);
				context.DrawPath(CGPathDrawingMode.Stroke);
			}
		}
	}
}