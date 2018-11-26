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
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Tile;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using Xamarin.Forms.Maps;
	using Xamarin.Forms.Maps.Android;
	using Xamarin.Forms.Platform.Android;

	public class CustomMapRenderer : MapRenderer, IUpdatable
	{
		public CustomMapRenderer(Context context)
			: base(context)
		{
			LayerService.MapRenderer = this;
		}

		protected CustomMap CustomMap => Element as CustomMap;

		protected bool IsInitialized { get; set; }

		protected PointHandler PointHandler { get; set; }

		protected TileOverlay TileOverlay { get; set; }

		private bool OldVisibility { get; set; } = true;

		private int OldRasterId { get; set; } = 1;

		public void Update()
		{
			SynchronizeWithLayerService();
		}

		public void SynchronizeWithLayerService()
		{
			// Check point layers
			bool pointsVisibles = LayerService.GetIsCheckedById(LayerService.LAYERPOINTS);
			if (pointsVisibles != OldVisibility)
			{
				PointHandler.ChangeVisibility(pointsVisibles);
				OldVisibility = pointsVisibles;
			}

			// Check raster layer
			int rasterId = LayerService.GetCurrentRasterId();
			if (rasterId != OldRasterId)
			{
				// Make the chance
				OldRasterId = rasterId;
				UpdateRasterLayer(rasterId);
			}
		}

		/// <summary>
		/// Set the mbtiles layer filename specified in parameter as background raster layer.
		/// </summary>
		/// <param name="mbtilesFileName">FileName of the mbtiles.</param>
		public void SetMbTilesAsBackground(string mbtilesFileName)
		{
			string connectionString = string.Format("Data Source={0}", mbtilesFileName);
			MBTileProvider mbTileProvider = new MBTileProvider(connectionString);

			if (TileOverlay != null)
			{
				TileOverlay.Remove();
			}

			TileOverlayOptions options = new TileOverlayOptions().InvokeZIndex(0f)
				.InvokeTileProvider(mbTileProvider);

			GoogleMap map = (GoogleMap)LayerService.CurrentMap;
			TileOverlay = map.AddTileOverlay(options);
		}

		/// <summary>
		/// Set Google Map as background raster layer.
		/// </summary>
		public void SetGoogleMapAsBackground()
		{
			if (TileOverlay != null)
			{
				TileOverlay.Remove();
			}

			// Find a better way to get dependencies
			TileOverlayOptions options = new TileOverlayOptions().InvokeZIndex(0f)
				.InvokeTileProvider(new CustomTileProvider(
					(IReadOnlyTileService)((App)Application.Current).Container.Resolve(typeof(IReadOnlyTileService))));

			GoogleMap map = (GoogleMap)LayerService.CurrentMap;
			TileOverlay = map.AddTileOverlay(options);
		}

		private void UpdateRasterLayer(int rasterId)
		{
			if (LayerService.IsGoogleMap(rasterId))
			{
				SetGoogleMapAsBackground();
			} else
			{
				SetMbTilesAsBackground(LayerService.GetMBTileFileName(rasterId));
			}
		}

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
			LayerService.CurrentMap = map;

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

				SetGoogleMapAsBackground();

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