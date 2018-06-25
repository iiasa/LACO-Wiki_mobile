// <copyright file="CustomUrlSchemeInterceptorActivity.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid.Core
{
	using System;
	using Android.App;
	using Android.Content;
	using Android.Content.PM;
	using Android.OS;
	using LacoWikiMobile.App.Core.Api;
	using Prism;
	using Prism.Ioc;

	[Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataSchemes = new[] { "laco-wiki-app" }, DataPath = "/")]
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Convert Android.Net.Url to Uri
			Uri uri = new Uri(Intent.Data.ToString());

			// Load redirectUrl page
			IApiAuthentication apiAuthentication =
				((PrismApplicationBase)Xamarin.Forms.Application.Current).Container.Resolve<IApiAuthentication>();
			apiAuthentication.Authenticator.OnPageLoading(uri);

			Intent intent = new Intent(this, typeof(MainActivity));
			intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
			StartActivity(intent);

			Finish();
		}
	}
}