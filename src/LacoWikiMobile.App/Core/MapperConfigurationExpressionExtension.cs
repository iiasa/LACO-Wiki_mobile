// <copyright file="MapperConfigurationExpressionExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Threading.Tasks;
	using AutoMapper;
	using LacoWikiMobile.App.Core.Api.Models;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels.ValidationSessionDetail;
	using Prism.Ioc;

	public static class MapperConfigurationExpressionExtension
	{
		public static void Configure(this IMapperConfigurationExpression mapperConfigurationExpression,
			IContainerProvider containerProvider)
		{
			mapperConfigurationExpression.ConfigureAppDataEntities();
			mapperConfigurationExpression.ConfigureApiModels(containerProvider);
		}

		public static void ConfigureApiModels(this IMapperConfigurationExpression mapperConfigurationExpression,
			IContainerProvider containerProvider)
		{
			mapperConfigurationExpression.CreateMap<ValidationSessionModel, ViewModels.ValidationSessionOverview.ItemViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ValidationSessionID))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ValidationSessionName))
				.ForMember(dest => dest.Pinned, configuration =>
				{
					configuration.ResolveUsing((source, destination, model, context) =>
					{
						IAppDataService appDataService = containerProvider.Resolve<IAppDataService>();

						Task<IEnumerable<ValidationSession>> task = Task.Run(async () => await appDataService.GetValidationSessionsAsync());
						task.Wait();

						IEnumerable<ValidationSession> result = task.Result;

						return result.Any(x => x.Id == source.ValidationSessionID);
					});
				})
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<LegendItemModel, ViewModels.ValidationSessionDetail.ItemViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LegendItemID))
				.ForMember(dest => dest.Color, opt => opt.MapFrom(src => Color.FromArgb(src.Red, src.Green, src.Blue)))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<ValidationSessionDetailModel, ValidationSessionDetailViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ValidationSessionID))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ValidationSessionName));
		}

		public static void ConfigureAppDataEntities(this IMapperConfigurationExpression mapperConfigurationExpression)
		{
			mapperConfigurationExpression.CreateMap<ValidationSession, ViewModels.Main.ItemViewModel>()
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.ForMember(dest => dest.IsChecked, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<ValidationSession, ViewModels.ValidationSessionOverview.ItemViewModel>()
				.ForMember(dest => dest.Pinned, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<ValidationSessionDetailViewModel, ValidationSession>()
				.ForMember(dest => dest.User, opt => opt.Ignore());
		}
	}
}