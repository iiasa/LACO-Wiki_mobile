// <copyright file="CircularProgressBarRenderer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using LacoWikiMobile.App.Droid.UserInterface;
using LacoWikiMobile.App.UserInterface;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CircularProgressBar), typeof(CircularProgressBarRenderer))]

namespace LacoWikiMobile.App.Droid.UserInterface
{
	using System.ComponentModel;
	using Android.Content;
	using LacoWikiMobile.App.UserInterface;
	using Xamarin.Forms.Platform.Android;

	public class CircularProgressBarRenderer : ViewRenderer<CircularProgressBar, CircularProgressBarView>
	{
		public CircularProgressBarRenderer(Context context)
			: base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<CircularProgressBar> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				// Instantiate the native control and assign it to the Control property with the SetNativeControl method
				CircularProgressBarView view = new CircularProgressBarView(Context)
				{
					Progress = Element.Progress,
					StrokeWidth = Element.StrokeWidth,
					ProgressColor = Element.ProgressColor.ToAndroid(),
					ProgressBackgroundColor = Element.ProgressBackgroundColor.ToAndroid(),
				};

				SetNativeControl(view);
			}

			if (e.OldElement != null)
			{
				// Unsubscribe from event handlers and cleanup any resources
			}

			if (e.NewElement != null)
			{
				// Configure the control and subscribe to event handlers
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CircularProgressBar.ProgressProperty.PropertyName)
			{
				Control.Progress = Element.Progress;
			}

			if (e.PropertyName == CircularProgressBar.StrokeWidthProperty.PropertyName)
			{
				Control.StrokeWidth = Element.StrokeWidth;
			}

			if (e.PropertyName == CircularProgressBar.ProgressColorProperty.PropertyName)
			{
				Control.ProgressColor = Element.ProgressColor.ToAndroid();
			}

			if (e.PropertyName == CircularProgressBar.ProgressBackgroundColorProperty.PropertyName)
			{
				Control.ProgressBackgroundColor = Element.ProgressBackgroundColor.ToAndroid();
			}
		}
	}
}