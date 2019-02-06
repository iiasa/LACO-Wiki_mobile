// <copyright file="SensorService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Sensor
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Plugin.Geolocator.Abstractions;
	using Xamarin.Essentials;

	public class SensorService : ISensorService
	{
		// TODO: Abstract compass and inject
		public SensorService(IGeolocator geolocator)
		{
			Geolocator = geolocator;
		}

		protected double? CurrentHeading { get; set; }

		protected IPosition CurrentPosition { get; set; }

		// TODO: Use Xamarin.Essentials when foreground location updates are supported
		// See https://github.com/xamarin/Essentials/issues/290
		protected IGeolocator Geolocator { get; set; }

		protected IDictionary<ITargetPositionObserver, TargetPositionObserverOptions> TargetPositionObservers { get; set; } =
			new Dictionary<ITargetPositionObserver, TargetPositionObserverOptions>();

		public async Task<bool> SubscribeToTargetPositionEventsAsync(ITargetPositionObserver observer,
			TargetPositionObserverOptions options)
		{
			// TODO: Add thread safety
			if (TargetPositionObservers.ContainsKey(observer))
			{
				TargetPositionObservers[observer] = options;
				return true;
			}

			Task<bool>[] tasks = { StartGeolocatorAsync(), StartCompassAsync() };
			bool[] results = await Task.WhenAll(tasks);

			if (!results.All(x => x))
			{
#pragma warning disable 4014
				StopCompassIfUnusedAsync();
				StopGeolocatorIfUnusedAsync();
#pragma warning restore 4014

				return false;
			}

			TargetPositionObservers.Add(observer, options);

			return true;
		}

		public async Task<bool> UnsubscribeToTargetPositionEventsAsync(ITargetPositionObserver observer)
		{
			// TODO: Add thread safety
			if (!TargetPositionObservers.ContainsKey(observer))
			{
				return true;
			}

			TargetPositionObservers.Remove(observer);

			Task<bool>[] tasks = { StopGeolocatorIfUnusedAsync(), StopCompassIfUnusedAsync() };
			bool[] results = await Task.WhenAll(tasks);

			return results.All(x => x);
		}

		protected void CompassOnReadingChanged(object sender, CompassChangedEventArgs args)
		{
			CurrentHeading = (args.Reading.HeadingMagneticNorth - GetDeviceOrientationDirection() + 360) % 360;

			NotifyTargetPositionObservers();
		}

		protected void GeolocatorOnPositionChanged(object sender, PositionEventArgs args)
		{
			CurrentPosition = new Position()
			{
				Longitude = args.Position.Longitude,
				Latitude = args.Position.Latitude,
			};

			NotifyTargetPositionObservers();
		}

		protected int GetDeviceOrientationDirection()
		{
			switch (DeviceDisplay.ScreenMetrics.Rotation)
			{
				case ScreenRotation.Rotation0:
					return 0;

				case ScreenRotation.Rotation90:
					return -90;

				case ScreenRotation.Rotation180:
					return 180;

				case ScreenRotation.Rotation270:
					return 90;

				default:
					return 0;
			}
		}

		protected void NotifyTargetPositionObserver(ITargetPositionObserver observer, TargetPositionObserverOptions options)
		{
			IPosition targetPosition = options.TargetPosition;

			double distance = Location.CalculateDistance(new Location(CurrentPosition.Latitude, CurrentPosition.Longitude),
				new Location(targetPosition.Latitude, targetPosition.Longitude), DistanceUnits.Kilometers);
			double direction = (CurrentPosition.GetBearing(targetPosition) - CurrentHeading.Value + 360) % 360;

			observer.OnDistanceChanged(distance);
			observer.OnDirectionChanged(direction);
		}

		protected void NotifyTargetPositionObservers()
		{
			if (CurrentPosition == null || CurrentHeading == null)
			{
				return;
			}

			foreach (KeyValuePair<ITargetPositionObserver, TargetPositionObserverOptions> targetPositionObserver in TargetPositionObservers)
			{
				NotifyTargetPositionObserver(targetPositionObserver.Key, targetPositionObserver.Value);
			}
		}

		protected Task<bool> StartCompassAsync()
		{
			if (!Compass.IsMonitoring)
			{
				Compass.ReadingChanged += CompassOnReadingChanged;
				Compass.Start(SensorSpeed.UI);
			}

			return Task.FromResult(true);
		}

		protected async Task<bool> StartGeolocatorAsync()
		{
			if (!Geolocator.IsGeolocationAvailable)
			{
				return false;
			}

			if (Geolocator.IsListening)
			{
				return true;
			}

			Geolocator.PositionChanged += GeolocatorOnPositionChanged;

			return await Geolocator.StartListeningAsync(TimeSpan.FromMilliseconds(10000), 1 /* Meters */);
		}

		protected Task<bool> StopCompassIfUnusedAsync()
		{
			if (TargetPositionObservers.Any())
			{
				return Task.FromResult(true);
			}

			Compass.ReadingChanged -= CompassOnReadingChanged;

			if (Compass.IsMonitoring)
			{
				Compass.Stop();
			}

			return Task.FromResult(true);
		}

		protected async Task<bool> StopGeolocatorIfUnusedAsync()
		{
			if (TargetPositionObservers.Any())
			{
				return true;
			}

			Geolocator.PositionChanged -= GeolocatorOnPositionChanged;

			if (Geolocator.IsListening)
			{
				return await Geolocator.StopListeningAsync();
			}

			return true;
		}
	}
}