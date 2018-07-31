// <copyright file="PlatformInitializer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS
{
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Localization;
	using LacoWikiMobile.App.iOS.Core;
	using Prism;
	using Prism.Ioc;

	public class PlatformInitializer : IPlatformInitializer
	{
		public void RegisterTypes(IContainerRegistry container)
		{
			// Register any platform specific implementations
			container.RegisterSingleton<ILocalizer, Localizer>();
			container.RegisterSingleton<INotificationService, TTGSnackbarNotificationService>();
		}
	}
}