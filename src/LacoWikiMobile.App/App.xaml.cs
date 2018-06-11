// <copyright file="App.xaml.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace LacoWikiMobile.App
{
	using System;
	using System.Linq;
	using System.Reflection;
	using AutoMapper;
	using AutoMapper.Configuration;
	using DryIoc;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Views;
	using Microsoft.EntityFrameworkCore;
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
			containerRegistry.Register<MapperConfigurationExpression, MapperConfigurationExpression>();
			containerRegistry.GetContainer()
				.RegisterInitializer<IMapperConfigurationExpression>((expression, resolver) => { expression.Configure(Container); });

			containerRegistry.Register<IConfigurationProvider, MapperConfiguration>();
			containerRegistry.GetContainer()
				.RegisterInitializer<IConfigurationProvider>((provider, resolver) =>
				{
					try
					{
						provider.AssertConfigurationIsValid();
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						throw;
					}
				});

			containerRegistry.GetContainer()
				.Register<IMapper, Mapper>(reuse: Reuse.Singleton, made: Made.Of(() => new Mapper(Arg.Of<IConfigurationProvider>())));

			containerRegistry.Register<DbContextOptionsBuilder<AppDataContext>, DbContextOptionsBuilder<AppDataContext>>();
			containerRegistry.GetContainer()
				.RegisterInitializer<DbContextOptionsBuilder<AppDataContext>>((builder, resolver) => { builder.Configure(); });

			containerRegistry.GetContainer()
				.Register<DbContextOptions>(made: Made.Of(r => ServiceInfo.Of<DbContextOptionsBuilder<AppDataContext>>(), f => f.Options));

			containerRegistry.RegisterSingleton<IAppDataContext, AppDataContext>();
			containerRegistry.GetContainer()
				.RegisterInitializer<IAppDataContext>((context, resolver) =>
				{
					((AppDataContext)context).Database.EnsureDeleted();
					((AppDataContext)context).Database.EnsureCreated();
				});

			containerRegistry.RegisterSingleton<IAppDataService, AppDataService>();
			containerRegistry.Register<IApiClient, StaticApiClient>();
		}
	}
}