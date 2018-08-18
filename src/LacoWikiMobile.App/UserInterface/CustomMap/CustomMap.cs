// <copyright file="CustomMap.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.CustomMap
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows.Input;
	using LacoWikiMobile.App.Core;
	using Prism;
	using Prism.Events;
	using Prism.Ioc;
	using Xamarin.Forms;
	using Xamarin.Forms.Maps;

	public class CustomMap : Map
	{
		public static readonly BindableProperty MapClickCommandProperty =
			BindableProperty.Create(nameof(MapClickCommand), typeof(ICommand), typeof(CustomMap), null);

		public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points), typeof(IEnumerable<IPoint>),
			typeof(CustomMap), new ObservableCollection<IPoint>());

		public CustomMap()
		{
			IEventAggregator eventAggregator = ((PrismApplicationBase)Application.Current).Container.Resolve<IEventAggregator>();
			eventAggregator.GetEvent<ZoomToExtentEvent>().Subscribe(ZoomToExtent);
		}

		~CustomMap()
		{
			IEventAggregator eventAggregator = ((PrismApplicationBase)Application.Current).Container.Resolve<IEventAggregator>();
			eventAggregator.GetEvent<ZoomToExtentEvent>().Unsubscribe(ZoomToExtent);
		}

		public ICommand MapClickCommand
		{
			get => (ICommand)GetValue(CustomMap.MapClickCommandProperty);
			set => SetValue(CustomMap.MapClickCommandProperty, value);
		}

		public IEnumerable<IPoint> Points
		{
			get => (IEnumerable<IPoint>)GetValue(CustomMap.PointsProperty);
			set => SetValue(CustomMap.PointsProperty, value);
		}

		protected void ZoomToExtent(IExtent extent)
		{
			Position position = new Position((extent.Top + extent.Bottom) / 2, (extent.Right + extent.Left) / 2);

			double distanceY = (extent.Top - extent.Bottom) * 1.5;
			double distanceX = (extent.Right - extent.Left) * 1.5;

			Helper.RunOnMainThreadIfRequired(() =>
			{
				MoveToRegion(new MapSpan(position, distanceY, distanceX));
			});
		}
	}
}