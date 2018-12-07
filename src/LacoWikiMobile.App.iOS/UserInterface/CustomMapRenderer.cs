// <copyright file="CustomMapRenderer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using LacoWikiMobile.App.iOS.UserInterface;
using LacoWikiMobile.App.UserInterface.CustomMap;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]

namespace LacoWikiMobile.App.iOS.UserInterface
{
	using System;
	using System.ComponentModel;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Tile;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using MapKit;
	using ObjCRuntime;
	using Xamarin.Forms;
	using Xamarin.Forms.Maps.iOS;
	using Xamarin.Forms.Platform.iOS;

	public class CustomMapRenderer : MapRenderer, IUpdatable
	{
		public CustomMapRenderer()
			: base()
		{
			// Store the current Map Renderer into LayerService
			LayerService.MapRenderer = this;
		}

		private MKMapView map;

		protected CustomMap CustomMap => Element as CustomMap;

		protected PointHandler PointHandler { get; set; }

		protected MKTileOverlay TileOverlay { get; set; }

		// Keep trace of old visiblity
		private bool OldVisibility { get; set; } = true;

		// Keep trace of old Google map visibility
		private bool OldGoogleMapVisibility { get; set; } = true;

		// Keep trace of old raster Id
		private int OldOfflineRasterId { get; set; } = 1;

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

			// Check Google map layer
			bool googleMapVisible = LayerService.GetIsCheckedById(LayerService.LAYERGOOGLEMAP);
			if (googleMapVisible != OldGoogleMapVisibility)
			{
				UpdateGoogleMapLayer(googleMapVisible);
				OldGoogleMapVisibility = googleMapVisible;
			}

			// Check offline raster layer
			int offlineRasterId = LayerService.GetCurrentOfflineRasterId();
			if (offlineRasterId != OldOfflineRasterId)
			{
				// Make the chance
				OldOfflineRasterId = offlineRasterId;
				UpdateRasterLayer(offlineRasterId);
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

			TileOverlay = customTileProvider;

			if (this.map != null)
			{
				this.map.AddOverlay(TileOverlay);
			}
		}

		/// <summary>
		/// Update the google map visibility.
		/// </summary>
		/// <param name="visible">Visiblity.</param>
		private void UpdateGoogleMapLayer(bool visible)
		{
			MKMapView map = (MKMapView)LayerService.CurrentMap;
			if (visible == true)
			{
				// Displayed !
				map.MapType = MKMapType.Standard;
			}
			else
			{
				// Not displayed ! (check if it's work, maybe it's not possible)
				map.MapType = MKMapType.MutedStandard;
			}
		}

		/// <summary>
		/// Update a raster layer.
		/// </summary>
		/// <param name="rasterId">Layer id.</param>
		private void UpdateRasterLayer(int rasterId)
		{
			// We need to display a mbtiles layer
			SetMbTilesAsBackground(LayerService.GetMBTileFileName(rasterId));
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

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null)
			{
				this.map = Control as MKMapView;
				LayerService.CurrentMap = this.map;

				TileOverlay = new CustomTileProvider(
					(IReadOnlyTileService)((App)Application.Current).Container.Resolve(typeof(IReadOnlyTileService)));

				if (this.map != null)
				{
					this.map.AddOverlay(TileOverlay);
					PointHandler = new PointHandler
					{
						Map = this.map,
						Points = CustomMap.Points,
					};
					this.map.OverlayRenderer = OverlayRenderer;
					PointHandler.OnMapClicked += OnMapClicked;
				}
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
				if (this.map == null)
				{
					return;
				}

				if (CustomMap.ShowTileLayer)
				{
					this.map.AddOverlay(TileOverlay, MKOverlayLevel.AboveRoads);
				}
				else
				{
					this.map.RemoveOverlay(TileOverlay);
				}
			}
		}

		protected void OnMapClicked(object sender, EventArgs e)
		{
			CustomMap.MapClickCommand?.Execute(null);
		}

		protected MKOverlayRenderer OverlayRenderer(MKMapView mapview, IMKOverlay overlay)
		{
			if (!(Runtime.GetNSObject(overlay.Handle) is MKCircle circle))
			{
				return new MKTileOverlayRenderer((MKTileOverlay)overlay);
			}

			return PointHandler.GetCircleRenderer(circle);
		}
	}
}