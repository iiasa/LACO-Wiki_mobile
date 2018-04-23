// <copyright file="App.xaml.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace LacoWikiMobile.App
{
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
		}

		protected override async void OnInitialized()
		{
			InitializeComponent();

			await NavigationService.NavigateAsync("NavigationPage/MainPage");
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<MainPage>();
		}
	}
}