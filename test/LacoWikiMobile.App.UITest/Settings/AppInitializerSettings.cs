// <copyright file="AppInitializerSettings.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UITest.Settings
{
	using Xamarin.UITest;

	public class AppInitializerSettings
	{
		public AppInitializerSettingsAndroid Android { get; set; }

#pragma warning disable SA1300 // Element should begin with upper-case letter
		public AppInitializerSettingsiOS iOS { get; set; }
#pragma warning restore SA1300 // Element should begin with upper-case letter

		public bool IsTestCloud { get; set; }

		public Platform Platform { get; set; }

		public TestPlatform TestPlatform { get; set; }
	}
}