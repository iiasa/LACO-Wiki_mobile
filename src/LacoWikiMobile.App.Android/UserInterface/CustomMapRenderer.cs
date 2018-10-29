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
	using System;
	using System.ComponentModel;
	using Android.Content;
	using Android.Gms.Maps;
	using Android.Gms.Maps.Model;
	using LacoWikiMobile.App.Core.Tile;
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

		protected TileOverlay TileOverlay { get; set; }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				PointHandler.Points = null;
				PointHandler.Map = null;
				PointHandler.OnMapClicked -= OnMapClicked;
				PointHandler = null;
			}

			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				// Unsubscribe from event handlers and cleanup any resources
				// TODO: Copy Dispose logic?
			}

			if (e.NewElement != null)
			{
				// Configure the control and subscribe to event handlers
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

			if (CustomMap.ShowTileLayerProperty.PropertyName == e.PropertyName)
			{
				TileOverlay.Visible = CustomMap.ShowTileLayer;
			}
		}

		protected void OnMapClicked(object sender, EventArgs e)
		{
			CustomMap.MapClickCommand?.Execute(null);
		}

		protected override void OnMapReady(GoogleMap map)
		{
			if (IsInitialized)
			{
				return;
			}

			if (map != null)
			{
				base.OnMapReady(map);

				// Disable the zoom-in and zoom-out buttons in any case
				map.UiSettings.ZoomControlsEnabled = false;

				// Disable rotation, so that the rotation button doesn't appear
				map.UiSettings.RotateGesturesEnabled = false;

				map.MapType = GoogleMap.MapTypeNone;

				// Find a better way to get dependencies
				TileOverlayOptions options = new TileOverlayOptions().InvokeZIndex(0f)
					.InvokeTileProvider(new CustomTileProvider(
						(IReadOnlyTileService)((App)Application.Current).Container.Resolve(typeof(IReadOnlyTileService))));

				TileOverlay = map.AddTileOverlay(options);

				IsInitialized = true;

				PointHandler = new PointHandler
				{
					Map = map,
					Points = CustomMap.Points,
				};
				PointHandler.OnMapClicked += OnMapClicked;
			}

		
		}
	}
}