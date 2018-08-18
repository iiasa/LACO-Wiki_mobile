// <copyright file="PositionExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Sensor
{
	using System;

	public static class PositionExtension
	{
		// See https://www.movable-type.co.uk/scripts/latlong.html
		public static double GetBearing(this IPosition p1, IPosition p2)
		{
			double lat1 = ToRad(p1.Latitude);
			double lon1 = ToRad(p1.Longitude);
			double lat2 = ToRad(p2.Latitude);
			double lon2 = ToRad(p2.Longitude);

			double y = Math.Sin(lon2 - lon1) * Math.Cos(lat2);
			double x = (Math.Cos(lat1) * Math.Sin(lat2)) - (Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1));

			double bearing = ToDegrees(Math.Atan2(y, x));

			while (bearing < 0 || bearing > 360)
			{
				if (bearing < 0)
				{
					bearing += 360;
				}

				if (bearing > 360)
				{
					bearing -= 360;
				}
			}

			return bearing;
		}

		public static double ToDegrees(this double rad)
		{
			return rad / Math.PI * 180.0;
		}

		public static double ToRad(this double deg)
		{
			return deg * Math.PI / 180.0;
		}
	}
}