// <copyright file="Application.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS
{
	using UIKit;
	using Xamarin.Forms.Android.UITests;
	using Xamarin.Forms.Internals;

	public class Application
	{
		// This is the main entry point of the application.
		private static void Main(string[] args)
		{
			Registrar.ExtraAssemblies = new[] { typeof(StyleProperties).Assembly };

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}