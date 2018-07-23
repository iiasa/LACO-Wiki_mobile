// <copyright file="Application.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;
using Foundation;

// See https://github.com/aspnet/EntityFrameworkCore/issues/10963
[assembly: Preserve(typeof(Queryable), AllMembers = true)]

namespace LacoWikiMobile.App.iOS
{
	using SQLitePCL;
	using UIKit;
	using Xamarin.Forms.Android.UITests;
	using Xamarin.Forms.Internals;

	public class Application
	{
		// This is the main entry point of the application.
		private static void Main(string[] args)
		{
			Registrar.ExtraAssemblies = new[] { typeof(StyleProperties).Assembly };

			Batteries_V2.Init();

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}