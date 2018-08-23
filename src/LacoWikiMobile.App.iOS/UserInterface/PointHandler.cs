// <copyright file="PointHandler.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS.UserInterface
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq;
	using CoreGraphics;
	using CoreLocation;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using MapKit;
	using ObjCRuntime;
	using UIKit;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	public class PointHandler
	{
		private MKMapView map;

		private IEnumerable<IPoint> points;

		public PointHandler()
		{
			PointsOnCollectionChanged = ObservableCollectionExtension.OnItemsAddedOrRemovedEventHandler((sender, itemsAdded) =>
			{
				Helper.RunOnMainThreadIfRequired(() =>
				{
					// Race condition that could occur if you set a new points collection
					if (!ReferenceEquals(sender, Points))
					{
						return;
					}

					foreach (IPoint point in itemsAdded.OfType<IPoint>())
					{
						AddPoint(point);
					}
				});
			}, (sender, itemsRemoved) =>
			{
				Helper.RunOnMainThreadIfRequired(() =>
				{
					// Race condition that could occur if you set a new points collection
					if (!ReferenceEquals(sender, Points))
					{
						return;
					}

					foreach (IPoint point in itemsRemoved.OfType<IPoint>())
					{
						RemovePoint(point);
					}
				});
			}, (sender) =>
			{
				Helper.RunOnMainThreadIfRequired(() =>
				{
					// Race condition that could occur if you set a new points collection
					if (!ReferenceEquals(sender, Points))
					{
						return;
					}

					RemovePoints();
				});
			});

			ClickGestureRecognizer = new UITapGestureRecognizer(recognizer =>
			{
				CGPoint tapPoint = recognizer.LocationInView(Map);
				CLLocationCoordinate2D tapCoordinate = Map.ConvertPoint(tapPoint, Map);
				MKMapPoint mapPoint = MKMapPoint.FromCoordinate(tapCoordinate);

				// TODO: Optimize access if necessary (use R tree, k-d tree or similar)
				MKCircle firstOrDefault = PointsToCirclesMapping.Values.FirstOrDefault(x => x.BoundingMapRect.Contains(mapPoint));

				if (firstOrDefault != null)
				{
					if (PointsToCirclesMapping[firstOrDefault] is ISelectable selectable)
					{
						if (selectable.IsSelectable)
						{
							selectable.Selected = !selectable.Selected;
						}
					}
				}
				else
				{
					OnMapClicked?.Invoke(this, EventArgs.Empty);
				}
			});
		}

		public event EventHandler<EventArgs> OnMapClicked;

		public MKMapView Map
		{
			get => this.map;
			set
			{
				if (this.map != null)
				{
					this.map.OverlayRenderer = null;
					this.map.RemoveGestureRecognizer(ClickGestureRecognizer);
				}

				this.map = value;

				if (this.map != null)
				{
					this.map.OverlayRenderer = OverlayRenderer;
					this.map.AddGestureRecognizer(ClickGestureRecognizer);
				}
			}
		}

		public IEnumerable<IPoint> Points
		{
			get => this.points;
			set
			{
				Helper.RunOnMainThreadIfRequired(() =>
				{
					if (this.points != null)
					{
						RemovePoints();
						UnsubscribePoints();
					}

					this.points = value;

					if (this.points != null)
					{
						AddPoints();
						SubscribePoints();
					}
				});
			}
		}

		protected IDictionary<IPoint, MKCircleRenderer> CircleRenderers { get; set; } = new Dictionary<IPoint, MKCircleRenderer>();

		protected UITapGestureRecognizer ClickGestureRecognizer { get; set; }

		protected NotifyCollectionChangedEventHandler PointsOnCollectionChanged { get; set; }

		protected IBictionary<IPoint, MKCircle> PointsToCirclesMapping { get; set; } = new Bictionary<IPoint, MKCircle>();

		protected void AddPoint(IPoint point)
		{
			Helper.EnsureOnMainThread();

			if (Map == null)
			{
				return;
			}

			double radius;

			if (point is IStyleable styleable)
			{
				radius = styleable.Radius;
			}
			else
			{
				radius = 100;
			}

			MKCircle circle = MKCircle.Circle(new CLLocationCoordinate2D(point.Latitude, point.Longitude), radius);
			PointsToCirclesMapping[point] = circle;

			Map.AddOverlay(circle);

			if (point is INotifyPropertyChanged notifyPropertyChanged)
			{
				notifyPropertyChanged.PropertyChanged += PointPropertyChanged;
			}
		}

		protected void AddPoints()
		{
			foreach (IPoint point in Points)
			{
				AddPoint(point);
			}
		}

		protected MKOverlayRenderer OverlayRenderer(MKMapView mapview, IMKOverlay overlay)
		{
			MKCircle circle = Runtime.GetNSObject(overlay.Handle) as MKCircle;
			IPoint point = PointsToCirclesMapping[circle];

			if (CircleRenderers.ContainsKey(point))
			{
				return CircleRenderers[point];
			}

			MKCircleRenderer circleRenderer = new MKCircleRenderer(overlay as MKCircle);

			if (point is IStyleable styleable)
			{
				circleRenderer.FillColor = styleable.FillColor.ToUIColor();
				circleRenderer.StrokeColor = styleable.StrokeColor.ToUIColor();
				circleRenderer.LineWidth = (float)styleable.StrokeWidth;
			}
			else
			{
				circleRenderer.FillColor = Color.DodgerBlue.ToUIColor();
				circleRenderer.StrokeColor = Color.DodgerBlue.AddLuminosity(-0.5).ToUIColor();
				circleRenderer.LineWidth = 1f;
			}

			CircleRenderers[point] = circleRenderer;

			return circleRenderer;
		}

		protected void PointPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Helper.RunOnMainThreadIfRequired(() =>
			{
				IPoint point = (IPoint)sender;

				if (!PointsToCirclesMapping.Contains(point))
				{
					return;
				}

				if (e.PropertyName == nameof(IPoint.Latitude) || e.PropertyName == nameof(IPoint.Longitude) ||
					e.PropertyName == nameof(IStyleable.Radius))
				{
					// See https://stackoverflow.com/questions/4759317/moving-mkcircle-in-mkmapview
					RemovePoint(point);
					AddPoint(point);
				}

				if (sender is IStyleable styleable)
				{
					if (e.PropertyName == nameof(IStyleable.FillColor) || e.PropertyName == nameof(IStyleable.StrokeColor) ||
						e.PropertyName == nameof(IStyleable.StrokeWidth))
					{
						CircleRenderers.Remove(point);
					}
				}
			});
		}

		protected void RemovePoint(IPoint point)
		{
			Helper.EnsureOnMainThread();

			if (Map == null)
			{
				return;
			}

			Map.RemoveOverlay(PointsToCirclesMapping[point]);
			PointsToCirclesMapping.Remove(point);
			CircleRenderers.Remove(point);

			if (point is INotifyPropertyChanged notifyPropertyChanged)
			{
				notifyPropertyChanged.PropertyChanged -= PointPropertyChanged;
			}
		}

		protected void RemovePoints()
		{
			foreach (IPoint point in PointsToCirclesMapping.Keys.ToList())
			{
				RemovePoint(point);
			}
		}

		protected void SubscribePoints()
		{
			if (Points is INotifyCollectionChanged notifyCollectionChanged)
			{
				notifyCollectionChanged.CollectionChanged += PointsOnCollectionChanged;
			}
		}

		protected void UnsubscribePoints()
		{
			if (Points is INotifyCollectionChanged notifyCollectionChanged)
			{
				notifyCollectionChanged.CollectionChanged -= PointsOnCollectionChanged;
			}
		}
	}
}