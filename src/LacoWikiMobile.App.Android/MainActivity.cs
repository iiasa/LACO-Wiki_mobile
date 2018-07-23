﻿// <copyright file="MainActivity.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid
{
	using Android.App;
	using Android.Content.PM;
	using Android.OS;
	using Plugin.CurrentActivity;
	using Xamarin.Forms;
	using Xamarin.Forms.Android.UITests;
	using Xamarin.Forms.Internals;
	using Xamarin.Forms.Platform.Android;

	[Activity(Label = "LACO-Wiki Mobile", Theme = "@style/MainTheme", MainLauncher = false,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		public MainActivity()
		{
			Registrar.ExtraAssemblies = new[] { typeof(StyleProperties).Assembly };
		}

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Forms.Init(this, bundle);

			Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = null;
			Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
			CrossCurrentActivity.Current.Init(this, bundle);

			LoadApplication(new App(new PlatformInitializer()));
		}
	}
}