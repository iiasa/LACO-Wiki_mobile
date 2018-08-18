// <copyright file="Position.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Sensor
{
	public class Position : IPosition
	{
		public double Latitude { get; set; }

		public double Longitude { get; set; }
	}
}