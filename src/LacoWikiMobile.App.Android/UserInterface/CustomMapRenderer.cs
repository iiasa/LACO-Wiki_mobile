// <copyright file="CustomMapRenderer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using LacoWikiMobile.App.Droid.UserInterface;
using LacoWikiMobile.App.UserInterface.CustomMap;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]

namespace LacoWikiMobile.App.Droid.UserInterface
{
	using System.ComponentModel;
	using Android.Content;
	using Android.Gms.Maps;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using Xamarin.Forms.Maps;
	using Xamarin.Forms.Maps.Android;
	using Xamarin.Forms.Platform.Android;

	public class CustomMapRenderer : MapRenderer
	{
		public CustomMapRenderer(Context context)
			: base(context)
		{
		}

		protected CustomMap CustomMap => Element as CustomMap;

		protected bool IsInitialized { get; set; }

		protected PointHandler PointHandler { get; set; }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				PointHandler.GoogleMap = null;
				PointHandler.Points = null;
			}

			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				// Unsubscribe from event handlers and cleanup any resources
			}

			if (e.NewElement != null)
			{
				// Configure the control and subscribe to event handlers
				CustomMap formsMap = (CustomMap)e.NewElement;
				Control.GetMapAsync(this);
			}

			if (Control == null)
			{
				// Instantiate the native control and assign it to the Control property with the SetNativeControl method
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (CustomMap.PointsProperty.PropertyName == e.PropertyName)
			{
				if (PointHandler != null)
				{
					PointHandler.Points = CustomMap.Points;
				}
			}
		}

		protected override void OnMapReady(GoogleMap map)
		{
			base.OnMapReady(map);

			if (IsInitialized)
			{
				return;
			}

			IsInitialized = true;

			// Disable the zoom-in and zoom-out buttons in any case
			map.UiSettings.ZoomControlsEnabled = false;

			// Disable rotation, so that the rotation button doesn't appear
			map.UiSettings.RotateGesturesEnabled = false;

			map.MapType = GoogleMap.MapTypeTerrain;

			PointHandler = new PointHandler()
			{
				GoogleMap = map,
				Points = CustomMap.Points,
			};
		}
	}
}