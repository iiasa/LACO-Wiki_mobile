// <copyright file="CircularProgressBar.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface
{
	using Xamarin.Forms;

	public class CircularProgressBar : View
	{
		public static readonly BindableProperty ProgressBackgroundColorProperty = BindableProperty.Create(nameof(ProgressBackgroundColor),
			typeof(Color), typeof(CircularProgressBar), default(Color));

		public static readonly BindableProperty ProgressColorProperty =
			BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(CircularProgressBar), default(Color));

		public static readonly BindableProperty ProgressProperty =
			BindableProperty.Create(nameof(Progress), typeof(double), typeof(CircularProgressBar), default(double));

		public static readonly BindableProperty StrokeWidthProperty =
			BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(CircularProgressBar), default(double));

		public double Progress
		{
			get => (double)GetValue(CircularProgressBar.ProgressProperty);
			set => SetValue(CircularProgressBar.ProgressProperty, value);
		}

		public Color ProgressBackgroundColor
		{
			get => (Color)GetValue(CircularProgressBar.ProgressBackgroundColorProperty);
			set => SetValue(CircularProgressBar.ProgressBackgroundColorProperty, value);
		}

		public Color ProgressColor
		{
			get => (Color)GetValue(CircularProgressBar.ProgressColorProperty);
			set => SetValue(CircularProgressBar.ProgressColorProperty, value);
		}

		public double StrokeWidth
		{
			get => (double)GetValue(CircularProgressBar.StrokeWidthProperty);
			set => SetValue(CircularProgressBar.StrokeWidthProperty, value);
		}
	}
}