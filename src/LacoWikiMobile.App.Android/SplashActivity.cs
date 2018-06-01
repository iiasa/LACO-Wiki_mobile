// <copyright file="SplashActivity.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid
{
	using Android.App;
	using Android.Content;
	using Android.Support.V7.App;

	[Activity(Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
		public override void OnBackPressed()
		{
		}

		protected override void OnResume()
		{
			base.OnResume();

			StartActivity(new Intent(Application.Context, typeof(MainActivity)));
		}
	}
}