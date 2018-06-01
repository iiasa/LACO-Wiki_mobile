// <copyright file="AppInitializer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UITest
{
	using System;
	using LacoWikiMobile.App.UITest.Settings;
	using Xamarin.UITest;
	using Xamarin.UITest.Configuration;

	public class AppInitializer
	{
		public static IApp StartApp(AppInitializerSettings settings)
		{
			// Local setup
			if (!settings.IsTestCloud)
			{
				switch (settings.Platform)
				{
					case Platform.Android:
						return ConfigureAndroidApp(settings);

					case Platform.iOS:
						return ConfigureiOSApp(settings);

					default:
						throw new ArgumentException(nameof(settings.Platform));
				}
			}

			// Cloud setup
			switch (settings.TestPlatform)
			{
				case TestPlatform.TestCloudAndroid:
					return ConfigureApp.Android.StartApp();

				case TestPlatform.TestCloudiOS:
					return ConfigureApp.iOS.StartApp();

				default:
					throw new ArgumentException(nameof(settings.TestPlatform));
			}
		}

		protected static IApp ConfigureAndroidApp(AppInitializerSettings settings)
		{
			AndroidAppConfigurator configurator = ConfigureApp.Android;

			if (settings.Android.Debug)
			{
				configurator = configurator.Debug();
			}

			if (settings.Android.EnableLocalScreenshots)
			{
				configurator = configurator.EnableLocalScreenshots();
			}

			configurator = configurator.ApkFile(settings.Android.ApkFile)
				.DeviceSerial(settings.Android.DeviceSerial)
				.WaitTimes(settings.Android.WaitTimes);

			return configurator.StartApp();
		}

		protected static IApp ConfigureiOSApp(AppInitializerSettings settings)
		{
			iOSAppConfigurator configurator = ConfigureApp.iOS;

			if (settings.iOS.Debug)
			{
				configurator = configurator.Debug();
			}

			if (settings.iOS.EnableLocalScreenshots)
			{
				configurator = configurator.EnableLocalScreenshots();
			}

			if (!string.IsNullOrEmpty(settings.iOS.AppBundle))
			{
				// Used for simulator tests
				configurator = configurator.AppBundle(settings.iOS.AppBundle);
			}

			if (!string.IsNullOrEmpty(settings.iOS.InstalledApp))
			{
				// Used for real device tests
				configurator = configurator.InstalledApp(settings.iOS.InstalledApp);
			}

			configurator = configurator
				.DeviceIdentifier(settings.iOS.DeviceIdentifier)
				.WaitTimes(settings.iOS.WaitTimes);

			return configurator.StartApp();
		}
	}
}