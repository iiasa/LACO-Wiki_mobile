// <copyright file="ContainerRegistryExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using AutoMapper;
	using AutoMapper.Configuration;
	using DryIoc;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Localization;
	using Microsoft.Extensions.Logging.Abstractions;
	using Microsoft.Extensions.Options;
	using Prism.DryIoc;
	using Prism.Ioc;

	public static class ContainerRegistryExtension
	{
		public static void RegisterApiAuthentication(this IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IApiAuthentication, ApiAuthentication>();
			containerRegistry.GetContainer()
				.RegisterInitializer<IApiAuthentication>((apiAuthentication, resolver) =>
				{
					apiAuthentication.Authenticated += async (sender, args) =>
					{
						IAppDataService appDataService = resolver.Resolve<IAppDataService>();
						await appDataService.EnsureUserExistsAsync();
					};

					// Start signed out
					////Task task = Task.Run(async () =>
					////{
					////	if (await apiAuthentication.IsAuthenticatedAsync())
					////	{
					////		await apiAuthentication.SignOutAsync();
					////	}
					////});

					////task.Wait();

					// Start with expired access token
					////Task task = Task.Run(async () =>
					////{
					////	string accessToken =
					////		"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IndwWDRxblFtTzVOWG1kbExUUXd6Vk53WWlZMCIsImtpZCI6IndwWDRxblFtTzVOWG1kbExUUXd6Vk53WWlZMCJ9.eyJpc3MiOiJodHRwczovL2xhY28td2lraS5uZXQvaWRlbnRpdHkiLCJhdWQiOiJodHRwczovL2xhY28td2lraS5uZXQvaWRlbnRpdHkvcmVzb3VyY2VzIiwiZXhwIjoxNTI5OTIxMDI3LCJuYmYiOjE1Mjk5MTc0MjcsImNsaWVudF9pZCI6IndlYmFwaSIsInNjb3BlIjoid2ViYXBpIiwic3ViIjoiNDYzIiwiYXV0aF90aW1lIjoxNTI5OTA5MTQ2LCJpZHAiOiJHZW9XaWtpIiwibmFtZSI6ImRyZXNlbCIsImVtYWlsIjoiZHJlc2VsQGlpYXNhLmFjLmF0Iiwicm9sZSI6IlVzZXIiLCJhbXIiOlsiZXh0ZXJuYWwiXX0.uwMcATLLWDQ967jUFmgr6SALuUu4BJ9Gdui671skver6Mev4jGy4W8aJPXyQ-yTaw0i_qqHPw75cA2IvGAZDMhVWEbboUUyDVapS6ukwOwcRpfmpSLeBzDQi99drVTKpfvn5KuVVuWUMWHaEfVf22PD-9ohpSC2S2ZNT8AVEVYlz1U4Vp1inV0R2wB3Z9PBg-OoZhExa3gP1yYqBxlBz3dt6moEgEGOYMsw2M8g81k3HX6F1SXatV-gc4ls6T1hCxDatk1tXj-mHwVZh85YmBAb5kZJ5crWdioXCCy_0r32AD6qx5d3wz_MkK68m4JI2UG5WfthVlt8eCWPPTSGrmA";
					////	await SecureStorage.SetAsync("access_token", accessToken);
					////});

					////task.Wait();
				});
		}

		public static void RegisterAppDataContext(this IContainerRegistry containerRegistry)
		{
			containerRegistry.Register<DbContextOptionsBuilder<AppDataContext>, DbContextOptionsBuilder<AppDataContext>>();
			containerRegistry.GetContainer()
				.RegisterInitializer<DbContextOptionsBuilder<AppDataContext>>((builder, resolver) => { builder.Configure(); });

			containerRegistry.GetContainer()
				.Register<DbContextOptions>(made: Made.Of(r => ServiceInfo.Of<DbContextOptionsBuilder<AppDataContext>>(), f => f.Options));

			containerRegistry.RegisterSingleton<IAppDataContext, AppDataContext>();
			containerRegistry.GetContainer()
				.RegisterInitializer<IAppDataContext>((context, resolver) =>
				{
					// Start with a clean database
					////((AppDataContext)context).Database.EnsureDeleted();

					((AppDataContext)context).Database.EnsureCreated();
				});
		}

		public static void RegisterAutoMapper(this IContainerRegistry containerRegistry, IContainerProvider containerProvider)
		{
			containerRegistry.Register<MapperConfigurationExpression, MapperConfigurationExpression>();
			containerRegistry.GetContainer()
				.RegisterInitializer<IMapperConfigurationExpression>((expression, resolver) =>
				{
					expression.Configure(containerProvider);
				});

			containerRegistry.Register<IConfigurationProvider, MapperConfiguration>();
			containerRegistry.GetContainer()
				.RegisterInitializer<IConfigurationProvider>((provider, resolver) =>
				{
					////try
					////{
					provider.AssertConfigurationIsValid();
					////}
					////catch (Exception e)
					////{
					////}
				});

			containerRegistry.GetContainer()
				.Register<IMapper, Mapper>(reuse: Reuse.Singleton, made: Made.Of(() => new Mapper(Arg.Of<IConfigurationProvider>())));
		}

		// Based on ASP.NET Core Localization https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization
		public static void RegisterLocalization(this IContainerRegistry containerRegistry, string resourcesPath = "Resources")
		{
			containerRegistry.GetContainer()
				.RegisterDelegate<IStringLocalizerFactory>(
					x =>
					{
						return new ResourceManagerStringLocalizerFactory(
							new OptionsManager<LocalizationOptions>(new OptionsFactory<LocalizationOptions>(
								new IConfigureOptions<LocalizationOptions>[]
									{ new ConfigureOptions<LocalizationOptions>(options => options.ResourcesPath = resourcesPath), },
								new IPostConfigureOptions<LocalizationOptions>[] { })), new NullLoggerFactory());
					}, new SingletonReuse());

			containerRegistry.GetContainer().Register(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
		}
	}
}