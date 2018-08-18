﻿// <copyright file="ZoomToExtentEvent.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.CustomMap
{
	using Prism.Events;

	public class ZoomToExtentEvent : PubSubEvent<IExtent>
	{
	}
}