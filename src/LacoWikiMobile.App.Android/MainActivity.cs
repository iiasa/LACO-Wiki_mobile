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
	using Plugin.Permissions;
	using Xamarin;
	using Xamarin.Auth;
	using Xamarin.Auth.Presenters.XamarinAndroid;
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

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Forms.Init(this, bundle);

			CustomTabsConfiguration.CustomTabsClosingMessage = null;
			AuthenticationConfiguration.Init(this, bundle);
			CrossCurrentActivity.Current.Init(this, bundle);

			FormsMaps.Init(this, bundle);

			LoadApplication(new App(new PlatformInitializer()));
		}
	}
}