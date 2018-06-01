// <copyright file="Tests.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UITest
{
	using System.IO;
	using LacoWikiMobile.App.UITest.Settings;
	using Microsoft.Extensions.Configuration;
	using NUnit.Framework;
	using Xamarin.UITest;

	public class Tests
	{
		public Tests()
		{
			IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile("appsettings.override.json", optional: true)
				.AddEnvironmentVariables();

			Settings = new AppInitializerSettings();
			builder.Build().GetSection(nameof(AppInitializerSettings)).Bind(Settings);

			Settings.IsTestCloud = TestEnvironment.IsTestCloud;
		}

		protected IApp App { get; set; }

		protected AppInitializerSettings Settings { get; set; }

		[Test]
		public void AppLaunches()
		{
			App.WaitForElement(x => x.Marked("WelcomeLabel"));

			App.Screenshot("First Screen");
		}

		[SetUp]
		public void BeforeEachTest()
		{
			App = AppInitializer.StartApp(Settings);
		}
	}
}