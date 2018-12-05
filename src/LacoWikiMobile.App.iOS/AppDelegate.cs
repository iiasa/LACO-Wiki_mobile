// <copyright file="AppDelegate.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS
{
	using System;
	using System.IO;
	using Foundation;
	using LacoWikiMobile.App.Core.Api;
	using Plugin.DownloadManager;
	using Plugin.DownloadManager.Abstractions;
	using Prism;
	using Prism.Ioc;
	using UIKit;
	using Xamarin;
	using Xamarin.Auth.Presenters.XamarinIOS;
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
			// Download manager
			LacoWikiMobile.App.Core.Data.FileManager.SavingPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file =>
			{
				string fileName = file.Url.GetHashCode().ToString();
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
				return path;
			});

			AuthenticationConfiguration.Init();
			UIApplication.SharedApplication.StatusBarHidden = false;

			FormsMaps.Init();

			LoadApplication(new App(new PlatformInitializer()));

			return base.FinishedLaunching(app, options);
		}

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			// Convert NSUrl to Uri
			Uri uri = new Uri(url.AbsoluteString);

			// Load redirectUrl page
			IApiAuthentication apiAuthentication =
				((PrismApplicationBase)Xamarin.Forms.Application.Current).Container.Resolve<IApiAuthentication>();
			apiAuthentication.Authenticator.OnPageLoading(uri);

			return true;
		}
	}
}