// <copyright file="PlatformInitializer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid
{
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Droid.Core;
	using Prism;
	using Prism.Ioc;
	using Xamarin.Forms;

	public class PlatformInitializer : IPlatformInitializer
	{
		public void RegisterTypes(IContainerRegistry container)
		{
			// Register any platform specific implementations
			container.Register<INotificationService, NotificationService>();

			// Remove after https://github.com/PrismLibrary/Prism/issues/1443 is fixed
			container.RegisterInstance(Forms.Context);
		}
	}
}