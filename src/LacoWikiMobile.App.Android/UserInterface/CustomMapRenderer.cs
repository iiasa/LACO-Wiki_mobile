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
	using Microsoft.EntityFrameworkCore;
	using Xamarin.Forms.Maps;
	using Xamarin.Forms.Maps.Android;
	using Xamarin.Forms.Platform.Android;

	public class CustomMapRenderer : MapRenderer, IUpdatable
	{
		public CustomMapRenderer(Context context)
			: base(context)
		{
			// Store the current Map Renderer into LayerService
			LayerService.MapRenderer = this;
		}

		protected CustomMap CustomMap => Element as CustomMap;

		protected bool IsInitialized { get; set; }

		protected PointHandler PointHandler { get; set; }

		protected TileOverlay TileOverlay { get; set; }

		// Keep trace of old visiblity
		private bool OldVisibility { get; set; } = true;

		// Keep trace of old raster Id
		private int OldRasterId { get; set; } = 1;

		// Apply switch layer changes into this map renderer
		public void Update()
		{
			SynchronizeWithLayerService();
		}

		// Adapt the map with changes into switch layer
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
			// Remove previous TileOverlay if created
			if (TileOverlay != null)
			{
				TileOverlay.Remove();
			}

			// Make Db Context Options Builder to create sqlite db builder
			DbContextOptionsBuilder<TileContext> myContextBuilder = new DbContextOptionsBuilder<TileContext>();
			myContextBuilder.UseSqlite($"Filename={mbtilesFileName}");

			// Initialize TileContext with this builder
			TileContext tileContext = new TileContext(myContextBuilder.Options);

			// Create TileService with this TileConext
			ReadOnlyTileService readOnlyTileService = new ReadOnlyTileService(tileContext);

			// Create CustomTileProvider with this TileService
			CustomTileProvider customTileProvider = new CustomTileProvider(readOnlyTileService);

			// And finally, create the TileOverlayOptions
			TileOverlayOptions options = new TileOverlayOptions().InvokeZIndex(0f)
				.InvokeTileProvider(customTileProvider);

			// And TileOverlay
			GoogleMap map = (GoogleMap)LayerService.CurrentMap;
			TileOverlay = map.AddTileOverlay(options);
		}

		/// <summary>
		/// Set Google Map as background raster layer.
		/// TODO : remove it, not useful, the GoogleMap do not need an overlay raster.
		/// DEPRECATED.
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

		/// <summary>
		/// Dispose the map renderer.
		/// </summary>
		/// <param name="disposing">Disposing.</param>
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
				if (TileOverlay != null)
				{
					TileOverlay.Visible = CustomMap.ShowTileLayer;
				}
			}
		}

		protected void OnMapClicked(object sender, EventArgs e)
		{
			CustomMap.MapClickCommand?.Execute(null);
		}

		protected override void OnMapReady(GoogleMap map)
		{
			base.OnMapReady(map);

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

				LayerService.CurrentMap = map;

				IsInitialized = true;

				PointHandler = new PointHandler
				{
					Map = map,
					Points = CustomMap.Points,
				};
				PointHandler.OnMapClicked += OnMapClicked;
			}
		}

		/// <summary>
		/// Update a raster layer.
		/// </summary>
		/// <param name="rasterId">Layer id.</param>
		private void UpdateRasterLayer(int rasterId)
		{
			// Check if google map backgroup layer is displayed or not
			GoogleMap map = (GoogleMap)LayerService.CurrentMap;
			if (LayerService.IsGoogleMapChecked())
			{
				// Displayed !
				map.MapType = GoogleMap.MapTypeSatellite;
			}
			else
			{
				// Not displayed !
				map.MapType = GoogleMap.MapTypeNone;
			}

			if (!LayerService.IsGoogleMap(rasterId))
			{
				// We need to display a mbtiles layer
				SetMbTilesAsBackground(LayerService.GetMBTileFileName(rasterId));
			}
		}
	}

}