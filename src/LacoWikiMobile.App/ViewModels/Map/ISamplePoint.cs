// <copyright file="ISamplePoint.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.Map
{
	using LacoWikiMobile.App.UserInterface.CustomMap;

	public interface ISamplePoint : IPoint
	{
		int Id { get; }
	}
}