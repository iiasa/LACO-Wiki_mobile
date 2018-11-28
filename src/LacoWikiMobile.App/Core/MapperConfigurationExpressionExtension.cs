// <copyright file="MapperConfigurationExpressionExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using AutoMapper;
	using AutoMapper.EquivalencyExpression;
	using LacoWikiMobile.App.Core.Api.Models;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels;
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
			// Api models to view models
			mapperConfigurationExpression.CreateMap<ValidationSessionModel, ViewModels.ValidationSessionOverview.ItemViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ValidationSessionID))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ValidationSessionName))
				.ForMember(dest => dest.Pinned, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.ForMember(dest => dest.IsSelected, opt => opt.Ignore())
				.ForMember(dest => dest.ItemSelectedCommand, opt => opt.Ignore())
				.AfterMap(async (src, dest) =>
				{
					IAppDataService appDataService = containerProvider.Resolve<IAppDataService>();
					dest.Pinned = await appDataService.TryGetValidationSessionByIdAsync(src.ValidationSessionID) != null;
				});

			mapperConfigurationExpression.CreateMap<ValidationSessionDetailModel, ValidationSessionDetailViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ValidationSessionID))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ValidationSessionName))
				.ForMember(dest => dest.OfflineCaches, opt => opt.MapFrom(src => src.OfflineCaches))
				.ForMember(dest => dest.ValidationMethod, opt => opt.MapFrom(src => src.ValidationMethodEnum));

			mapperConfigurationExpression.CreateMap<LegendItemModel, ItemViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LegendItemID))
				.ForMember(dest => dest.Color, opt => opt.MapFrom(src => Color.FromArgb(src.Red, src.Green, src.Blue)))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.ForMember(dest => dest.IsSelected, opt => opt.Ignore())
				.ForMember(dest => dest.ItemSelectedCommand, opt => opt.Ignore())
				.EqualityComparison((source, dest) => dest.Id == source.LegendItemID);


			mapperConfigurationExpression.CreateMap<OfflineCacheModel, OfflineCacheItemViewModel>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LayerName))
				.ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.FileSize))
				.ForMember(dest => dest.CacheButtonText, opt => opt.MapFrom(src => src.LayerName+" ("+String.Format("{0:0.00}",src.FileSize/1000000.0)+"MB)"))
				.ForMember(dest => dest.isDownloaded, opt => opt.Ignore())
				.ForMember(dest => dest.ImageButton, opt => opt.Ignore())
				.ForMember(dest => dest.Path, opt => opt.Ignore())
				.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));

			mapperConfigurationExpression.CreateMap<IEnumerable<SampleItemModel>, SamplePointsViewModel>()
				.ForMember(dest => dest.Points, opt => opt.MapFrom(src => src));

			mapperConfigurationExpression.CreateMap<SampleItemModel, SamplePointViewModel>()
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

			// View models to api models
			mapperConfigurationExpression.CreateMap<ViewModels.ValidationUpload.ItemViewModel, ValidationCreateModel>();
		}

		public static void ConfigureAppDataEntities(this IMapperConfigurationExpression mapperConfigurationExpression,
			IContainerProvider containerProvider)
		{
			// Entities to view models
			mapperConfigurationExpression.CreateMap<ValidationSession, ViewModels.Main.ItemViewModel>()
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.ForMember(dest => dest.IsChecked, opt => opt.Ignore())
				.ForMember(dest => dest.IsSelected, opt => opt.Ignore())
				.ForMember(dest => dest.ItemSelectedCommand, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<ValidationSession, ViewModels.ValidationSessionOverview.ItemViewModel>()
				.ForMember(dest => dest.Pinned, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.ForMember(dest => dest.IsSelected, opt => opt.Ignore())
				.ForMember(dest => dest.ItemSelectedCommand, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<LegendItem, ItemViewModel>()
				.ForMember(dest => dest.Color, opt => opt.MapFrom(src => Color.FromArgb(src.Red, src.Green, src.Blue)))
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.ForMember(dest => dest.IsSelected, opt => opt.Ignore())
				.ForMember(dest => dest.ItemSelectedCommand, opt => opt.Ignore())
				.EqualityComparison((source, dest) => dest.Id == source.Id);


			mapperConfigurationExpression.CreateMap<OfflineCache, OfflineCacheItemViewModel>()
				.ForMember(dest => dest.Name, opt => opt.Ignore())
				.ForMember(dest => dest.Size, opt => opt.Ignore())
				.ForMember(dest => dest.Url, opt => opt.Ignore())
				.ForMember(dest => dest.CacheButtonText, opt => opt.Ignore())
				.ForMember(dest => dest.ImageButton, opt => opt.Ignore())
				.ForMember(dest => dest.Path, opt => opt.Ignore())
				.ForMember(dest => dest.isDownloaded, opt => opt.Ignore());



			mapperConfigurationExpression.CreateMap<ValidationSession, SamplePointsViewModel>()
				.ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.SampleItems));

			mapperConfigurationExpression.CreateMap<SampleItem, SamplePointViewModel>()
				.ForMember(dest => dest.Longitude, opt => opt.Ignore())
				.ForMember(dest => dest.Latitude, opt => opt.Ignore())
				.ForMember(dest => dest.Selected, opt => opt.Ignore())
				.ForMember(dest => dest.Radius, opt => opt.Ignore())
				.ForMember(dest => dest.FillColor, opt => opt.Ignore())
				.ForMember(dest => dest.StrokeColor, opt => opt.Ignore())
				.ForMember(dest => dest.StrokeWidth, opt => opt.Ignore())
				.ForMember(dest => dest.ValidationSessionId, opt => opt.Ignore())
				.ForMember(dest => dest.LegendItemId, opt => opt.Ignore())
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

			mapperConfigurationExpression.CreateMap<LocalValidation, ViewModels.ValidationUpload.ItemViewModel>()
				.ForMember(dest => dest.LegendItemId, opt => opt.MapFrom(src => src.LegendItem == null ? null : (int?)src.LegendItem.Id))
				.ForMember(dest => dest.Uploaded, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, opt => opt.Ignore())
				.ForMember(dest => dest.ItemTappedCommand, opt => opt.Ignore())
				.ForMember(dest => dest.IsSelected, opt => opt.Ignore())
				.ForMember(dest => dest.ItemSelectedCommand, opt => opt.Ignore())
				.EqualityComparison((source, destination) => source.SampleItem.Id == destination.SampleItemId);

			// View models to entities
			mapperConfigurationExpression.CreateMap<ValidationSessionDetailViewModel, ValidationSession>()
				.ForMember(dest => dest.UserId, opt => opt.Ignore())
				.ForMember(dest => dest.User, opt => opt.Ignore())
				.ForMember(dest => dest.OfflineCaches, opt => opt.Ignore())
				.ForMember(dest => dest.SampleItems, opt => opt.Ignore());

			mapperConfigurationExpression.CreateMap<ItemViewModel, LegendItem>()
				.ForMember(dest => dest.Red, opt => opt.MapFrom(src => src.Color.R * 255.0))
				.ForMember(dest => dest.Green, opt => opt.MapFrom(src => src.Color.G * 255.0))
				.ForMember(dest => dest.Blue, opt => opt.MapFrom(src => src.Color.B * 255.0))
				.ForMember(dest => dest.ValidationSession, opt => opt.Ignore())
				.ForMember(dest => dest.SampleItems, opt => opt.Ignore())
				.EqualityComparison((source, dest) => dest.Id == source.Id);

			mapperConfigurationExpression.CreateMap<OfflineCacheItemViewModel, OfflineCache>()
			.ForMember(dest => dest.LayerName, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.Size))
			.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
			.EqualityComparison((source, dest) => dest.LayerName == source.Name);

			mapperConfigurationExpression.CreateMap<SamplePointsViewModel, ValidationSession>(MemberList.Source)
				.ForMember(dest => dest.SampleItems, opt => opt.MapFrom(src => src.Points));

			mapperConfigurationExpression.CreateMap<SamplePointViewModel, SampleItem>()
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
				.ForMember(dest => dest.LocalValidation, opt => opt.Ignore())
				.AfterMap((src, dest, context) =>
				{
					ValidationSession validationSession = (ValidationSession)context.Items[nameof(ValidationSession)];
					IDictionary<int, LegendItem> legendItems = (IDictionary<int, LegendItem>)context.Items[nameof(LegendItem)];

					dest.ValidationSession = validationSession;
					dest.LegendItem = legendItems[src.LegendItemId];
				})
				.EqualityComparison((source, destination) => source.Id == destination.Id);

			mapperConfigurationExpression.CreateMap<ValidatePageViewModel, LocalValidation>()
				.ForMember(dest => dest.LegendItem, opt => opt.Ignore())
				.ForMember(dest => dest.SampleItem, opt => opt.Ignore())
				.ForMember(dest => dest.Uploaded, opt => opt.Ignore())
				.AfterMap(async (src, dest, context) =>
				{
					IAppDataService appDataService = containerProvider.Resolve<IAppDataService>();

					dest.SampleItem = await appDataService.GetSampleItemByIdAsync(src.SampleItemId, src.ValidationSessionId);
					dest.LegendItem = src.SelectedLegendItem != null
						? await appDataService.GetLegendItemByIdAsync(src.SelectedLegendItem.Id, src.ValidationSessionId)
						: null;
				});
		}
	}
}