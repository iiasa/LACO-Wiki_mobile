// <copyright file="NullNotificationService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS.Core
{
	using LacoWikiMobile.App.Core;

	public class NullNotificationService : INotificationService
	{
		public void Notify(string message)
		{
		}
	}
}