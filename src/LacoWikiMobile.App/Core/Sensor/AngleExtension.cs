// <copyright file="AngleExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Sensor
{
	public static class AngleExtension
	{
		public static double DifferenceTo(this double angle1, double angle2)
		{
			double difference = angle1 - angle2;

			if (difference > 180)
			{
				difference -= 360;
			}

			if (difference < -180)
			{
				difference += 360;
			}

			return difference;
		}

		public static double Normalize(this double angle)
		{
			return ((angle % 360) + 360) % 360;
		}
	}
}