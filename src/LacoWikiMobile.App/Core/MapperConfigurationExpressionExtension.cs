// <copyright file="MapperConfigurationExpressionExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using AutoMapper;
	using AutoMapper.EquivalencyExpression;
	using LacoWikiMobile.App.Core.Api.Models;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels.Map;
	using LacoWikiMobile.App.ViewModels.ValidationSessionDetail;
	using Prism.Ioc;
	using Wkx;

	public static class MapperConfigurationExpressionExtension
	{
		public static void Configure(this IMapperConfigurationExpression mapperConfigurationExpression,
			IContainerProvider containerProvider)
		{
			mapperConfigurationExpression.ConfigureAppDataEntities(containerProvider);
			mapperConfigurationExpression.ConfigureApiModels(containerProvider);
		}

		public static void ConfigureApiModels(this IMapperConfigurationExpression mapperConfigurationExpression,
			IContainerProvider containerProvider)
		{
			mapperConfigurationExpression.CreateMap<ValidationSessionModel, ViewModels.ValidationSessionOverview.ItemViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ValidationSessionID))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ValidationSessionName))
				.ForMember(dest => dest.Pinned, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.AfterMap(async (src, dest) =>
				{
					IAppDataService appDataService = containerProvider.Resolve<IAppDataService>();
					dest.Pinned = await appDataService.TryGetValidationSessionByIdAsync(src.ValidationSessionID) != null;
				});

			mapperConfigurationExpression.CreateMap<ValidationSessionDetailModel, ValidationSessionDetailViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ValidationSessionID))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ValidationSessionName));

			mapperConfigurationExpression.CreateMap<LegendItemModel, ItemViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LegendItemID))
				.ForMember(dest => dest.Color, opt => opt.MapFrom(src => Color.FromArgb(src.Red, src.Green, src.Blue)))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.EqualityComparison((source, dest) => dest.Id == source.LegendItemID);

			mapperConfigurationExpression.CreateMap<IEnumerable<SampleItemModel>, ValidationPointsViewModel>()
				.ForMember(dest => dest.Points, opt => opt.MapFrom(src => src));

			mapperConfigurationExpression.CreateMap<SampleItemModel, ValidationPointViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SampleItemID))
				.ForMember(dest => dest.ValidationSessionId,
					configuration =>
					{
						configuration.ResolveUsing(
							(source, destination, model, context) => (int)context.Items[nameof(ValidationSession.Id)]);
					})
				.ForMember(dest => dest.IsValidated, opt => opt.MapFrom(src => src.Validated))
				.ForMember(dest => dest.Longitude, opt => opt.Ignore())
				.ForMember(dest => dest.Latitude, opt => opt.Ignore())
				.ForMember(dest => dest.Selected, opt => opt.Ignore())
				.ForMember(dest => dest.Radius, opt => opt.Ignore())
				.ForMember(dest => dest.FillColor, opt => opt.Ignore())
				.ForMember(dest => dest.StrokeColor, opt => opt.Ignore())
				.ForMember(dest => dest.StrokeWidth, opt => opt.Ignore())
				.AfterMap((source, dest) =>
				{
					Wkx.Point geometry = Geometry.Deserialize<WktSerializer>(source.GeometryString) as Wkx.Point;

					if (geometry == null)
					{
						throw new InvalidOperationException();
					}

					dest.Longitude = geometry.X.Value;
					dest.Latitude = geometry.Y.Value;
				})
				.EqualityComparison((source, destination) => source.SampleItemID == destination.Id);
		}

		public static void ConfigureAppDataEntities(this IMapperConfigurationExpression mapperConfigurationExpression,
			IContainerProvider containerProvider)
		{
			// Entities to view models
			mapperConfigurationExpression.CreateMap<ValidationSession, ViewModels.Main.ItemViewModel>()
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.ForMember(dest => dest.IsChecked, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<ValidationSession, ViewModels.ValidationSessionOverview.ItemViewModel>()
				.ForMember(dest => dest.Pinned, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<LegendItem, ItemViewModel>()
				.ForMember(dest => dest.Color, opt => opt.MapFrom(src => Color.FromArgb(src.Red, src.Green, src.Blue)))
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.EqualityComparison((source, dest) => dest.Id == source.Id);

			mapperConfigurationExpression.CreateMap<ValidationSession, ValidationPointsViewModel>()
				.ForMember(dest => dest.Points, opt => opt.MapFrom(src => src));

			mapperConfigurationExpression.CreateMap<SampleItem, ValidationPointViewModel>()
				.ForMember(dest => dest.Longitude, opt => opt.Ignore())
				.ForMember(dest => dest.Latitude, opt => opt.Ignore())
				.ForMember(dest => dest.Selected, opt => opt.Ignore())
				.ForMember(dest => dest.Radius, opt => opt.Ignore())
				.ForMember(dest => dest.FillColor, opt => opt.Ignore())
				.ForMember(dest => dest.StrokeColor, opt => opt.Ignore())
				.ForMember(dest => dest.StrokeWidth, opt => opt.Ignore())
				.AfterMap((src, dest) =>
				{
					Wkx.Point geometry = Geometry.Deserialize<WktSerializer>(src.Geometry) as Wkx.Point;

					if (geometry == null)
					{
						throw new InvalidOperationException();
					}

					dest.Longitude = geometry.X.Value;
					dest.Latitude = geometry.Y.Value;
				})
				.EqualityComparison((source, destination) => source.Id == destination.Id);

			// View models to entities
			mapperConfigurationExpression.CreateMap<ValidationSessionDetailViewModel, ValidationSession>()
				.ForMember(dest => dest.User, opt => opt.Ignore())
				.ForMember(dest => dest.SampleItems, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<ItemViewModel, LegendItem>()
				.ForMember(dest => dest.Red, opt => opt.MapFrom(src => src.Color.R * 255.0))
				.ForMember(dest => dest.Green, opt => opt.MapFrom(src => src.Color.G * 255.0))
				.ForMember(dest => dest.Blue, opt => opt.MapFrom(src => src.Color.B * 255.0))
				.ForMember(dest => dest.ValidationSession, opt => opt.Ignore())
				.ForMember(dest => dest.SampleItems, opt => opt.Ignore())
				.EqualityComparison((source, dest) => dest.Id == source.Id);

			mapperConfigurationExpression.CreateMap<ValidationPointsViewModel, ValidationSession>(MemberList.Source)
				.ForMember(dest => dest.SampleItems, opt => opt.MapFrom(src => src.Points));

			mapperConfigurationExpression.CreateMap<ValidationPointViewModel, SampleItem>()
				.ForMember(dest => dest.Geometry, configuration =>
				{
					configuration.ResolveUsing((source, destination) =>
					{
						Wkx.Point point = new Wkx.Point(source.Longitude, source.Latitude);
						return point.SerializeString<WktSerializer>();
					});
				})
				.ForMember(dest => dest.ValidationSession, opt => opt.Ignore())
				.ForMember(dest => dest.LegendItem, opt => opt.Ignore())
				.AfterMap((src, dest, context) =>
				{
					ValidationSession validationSession = (ValidationSession)context.Items[nameof(ValidationSession)];
					IDictionary<int, LegendItem> legendItems = (IDictionary<int, LegendItem>)context.Items[nameof(LegendItem)];

					dest.ValidationSession = validationSession;
					dest.LegendItem = legendItems[src.LegendItemId];
				})
				.EqualityComparison((source, destination) => source.Id == destination.Id);
		}
	}
}