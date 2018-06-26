// <copyright file="App.xaml.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace LacoWikiMobile.App
{
	using System;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Localization;
	using LacoWikiMobile.App.Views;
	using Prism;
	using Prism.DryIoc;
	using Prism.Ioc;
	using Xamarin.Forms;

	public partial class App : PrismApplication
	{
		/*
		* The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
		* This imposes a limitation in which the App class must have a default constructor.
		* App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
		*/
		public App()
			: this(null)
		{
		}

		public App(IPlatformInitializer initializer)
			: base(initializer)
		{
#if DEBUG
			// Initialize Live Reload.
			LiveReload.Init();
#endif
		}

		protected override async void OnInitialized()
		{
			InitializeComponent();

			// See https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/localization
			if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
			{
				// Determine the correct, supported .NET culture
				ILocalizer localizer = Container.Resolve<ILocalizer>();
				CultureInfo cultureInfo = localizer.GetCurrentCultureInfo();
				localizer.SetLocale(cultureInfo);
			}

			await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<NavigationPage>();

			// Register all pages
			foreach (Type type in Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(x => x.IsClass && x.Namespace == typeof(MainPage).Namespace && x.Name.EndsWith("Page")))
			{
				containerRegistry.RegisterForNavigation(type, type.Name);
			}

			// Register services
			containerRegistry.RegisterLocalization();
			containerRegistry.RegisterAutoMapper(Container);
			containerRegistry.RegisterAppDataContext();
			containerRegistry.RegisterApiAuthentication();

			containerRegistry.RegisterSingleton<IAppDataService, AppDataService>();
			containerRegistry.Register<IApiClient, ApiClient>();
		}
	}
}