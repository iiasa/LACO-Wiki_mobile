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
	using LacoWikiMobile.App.Core.Tile;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using MapKit;
	using ObjCRuntime;
	using Xamarin.Forms;
	using Xamarin.Forms.Maps.iOS;
	using Xamarin.Forms.Platform.iOS;

	public class CustomMapRenderer : MapRenderer
	{
		private MKMapView map;

		protected CustomMap CustomMap => Element as CustomMap;

		protected PointHandler PointHandler { get; set; }

		protected MKTileOverlay TileOverlay { get; set; }

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