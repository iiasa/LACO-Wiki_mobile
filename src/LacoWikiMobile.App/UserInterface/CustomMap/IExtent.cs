// <copyright file="IExtent.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.CustomMap
{
	public interface IExtent
	{
		double Bottom { get; }

		double Left { get; }

		double Right { get; }

		double Top { get; }
	}
}