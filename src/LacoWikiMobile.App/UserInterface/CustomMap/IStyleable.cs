// <copyright file="IStyleable.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.CustomMap
{
	using Xamarin.Forms;

	public interface IStyleable
	{
		Color FillColor { get; }

		double Radius { get; }

		Color StrokeColor { get; }

		double StrokeWidth { get; }
	}
}