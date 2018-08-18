// <copyright file="Extent.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.CustomMap
{
	public class Extent : IExtent
	{
		public Extent(double top, double left, double right, double bottom)
		{
			Top = top;
			Left = left;
			Right = right;
			Bottom = bottom;
		}

		public double Bottom { get; }

		public double Left { get; }

		public double Right { get; }

		public double Top { get; }
	}
}