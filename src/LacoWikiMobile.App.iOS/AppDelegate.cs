// <copyright file="AppDelegate.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS
{
	using Foundation;
	using UIKit;
	using Xamarin;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : FormsApplicationDelegate
	{
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
#if DEBUG
			Calabash.Start();
#endif

			Forms.Init();
			LoadApplication(new App(new PlatformInitializer()));

			UIApplication.SharedApplication.StatusBarHidden = false;
			return base.FinishedLaunching(app, options);
		}
	}
}