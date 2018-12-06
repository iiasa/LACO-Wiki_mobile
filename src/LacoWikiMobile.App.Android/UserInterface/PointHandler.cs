// <copyright file="PointHandler.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid.UserInterface
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq;
	using Android.Gms.Maps;
	using Android.Gms.Maps.Model;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	public class PointHandler
	{
		private GoogleMap map;

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
		}

		public event EventHandler<EventArgs> OnMapClicked;

		public GoogleMap Map
		{
			get => this.map;
			set
			{
				if (this.map != null)
				{
					this.map.MapClick -= MapClick;
					this.map.CircleClick -= OnCircleClick;
				}

				this.map = value;

				if (this.map != null)
				{
					this.map.MapClick += MapClick;
					this.map.CircleClick += OnCircleClick;
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

		protected NotifyCollectionChangedEventHandler PointsOnCollectionChanged { get; set; }

		protected IBictionary<IPoint, Circle> PointsToCirclesMapping { get; set; } = new CircleBictionary<IPoint>();

		public void ChangeVisibility(bool visible)
		{
			if (visible == false)
			{
				RemovePoints();
			} else
			{
				AddPoints();
			}
		}

		protected void AddPoint(IPoint point)
		{
			Helper.EnsureOnMainThread();

			if (Map == null)
			{
				return;
			}

			CircleOptions circleOptions = new CircleOptions();
			circleOptions.InvokeZIndex(1.0f);

			if (point is ISelectable selectable)
			{
				circleOptions.Clickable(true);
			}

			if (point is IStyleable styleable)
			{
				circleOptions.InvokeFillColor(styleable.FillColor.ToAndroid());
				circleOptions.InvokeStrokeColor(styleable.StrokeColor.ToAndroid());
				circleOptions.InvokeStrokeWidth((float)styleable.StrokeWidth);
				circleOptions.InvokeRadius((float)styleable.Radius);
			}
			else
			{
				circleOptions.InvokeFillColor(Color.DodgerBlue.ToAndroid());
				circleOptions.InvokeStrokeColor(Color.DodgerBlue.AddLuminosity(-0.5).ToAndroid());
				circleOptions.InvokeStrokeWidth(1);
				circleOptions.InvokeRadius(100);
			}

			circleOptions.InvokeCenter(new LatLng(point.Latitude, point.Longitude));

			Circle circle = Map.AddCircle(circleOptions);
			PointsToCirclesMapping[point] = circle;

			if (point is INotifyPropertyChanged notifyPropertyChanged)
			{
				notifyPropertyChanged.PropertyChanged += PointPropertyChanged;
			}
		}

		protected void AddPoints()
		{
			// crash here
			foreach (IPoint point in Points.ToList()) 
			{
				AddPoint(point);
			}
		}

		protected void MapClick(object sender, GoogleMap.MapClickEventArgs e)
		{
			OnMapClicked?.Invoke(this, EventArgs.Empty);
		}

		protected void OnCircleClick(object sender, GoogleMap.CircleClickEventArgs e)
		{
			if (PointsToCirclesMapping[e.Circle] is ISelectable selectable)
			{
				if (selectable.IsSelectable)
				{
					selectable.Selected = !selectable.Selected;
				}
			}
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

				Circle circle = PointsToCirclesMapping[point];

				if (e.PropertyName == nameof(IPoint.Latitude) || e.PropertyName == nameof(IPoint.Longitude))
				{
					circle.Center = new LatLng(point.Latitude, point.Longitude);
				}

				if (sender is IStyleable styleable)
				{
					if (e.PropertyName == nameof(IStyleable.FillColor))
					{
						circle.FillColor = styleable.FillColor.ToAndroid();
					}

					if (e.PropertyName == nameof(IStyleable.StrokeColor))
					{
						circle.StrokeColor = styleable.StrokeColor.ToAndroid();
					}

					if (e.PropertyName == nameof(IStyleable.StrokeWidth))
					{
						circle.StrokeWidth = (float)styleable.StrokeWidth;
					}

					if (e.PropertyName == nameof(IStyleable.Radius))
					{
						circle.Radius = styleable.Radius;
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

			PointsToCirclesMapping[point].Remove();
			PointsToCirclesMapping.Remove(point);

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