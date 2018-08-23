// <copyright file="ISelectable.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.CustomMap
{
	public interface ISelectable
	{
		bool IsSelectable { get; }

		bool Selected { get; set; }
	}
}