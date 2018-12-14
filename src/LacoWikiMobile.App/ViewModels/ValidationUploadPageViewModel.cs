// <copyright file="ValidationUploadPageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using AutoMapper;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Api.Models;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels.ValidationUpload;
	using Microsoft.Extensions.Localization;
	using Prism.Navigation;

	public class ValidationUploadPageViewModel : ViewModelBase
	{
		public ValidationUploadPageViewModel(INavigationService navigationService, IPermissionService permissionService,
			IStringLocalizer<ValidationUploadPageViewModel> stringLocalizer, IAppDataService appDataService, IApiClient apiClient,
			IMapper mapper)
			: base(navigationService, permissionService, stringLocalizer)
		{
			AppDataService = appDataService;
			ApiClient = apiClient;
			Mapper = mapper;

			Items = new ObservableCollection<ItemViewModel>().OnPropertyChanged((sender, args) =>
			{
				OnPropertyChanged(nameof(ShowInfo));
				OnPropertyChanged(nameof(ShowList));
			});

			// TODO: Localization
			Title = "Validation Upload";
		}

		public bool ShowInfo => Items.Count == 0;

		public bool ShowList => Items.Any();

		public ICollection<ItemViewModel> Items { get; set; }

		public int ValidationSessionId { get; set; }

		protected IApiClient ApiClient { get; }

		protected IAppDataService AppDataService { get; }

		protected IMapper Mapper { get; }

		protected override async Task ExecutePrimaryActionAsync()
		{
			await base.ExecutePrimaryActionAsync();

			foreach (ItemViewModel item in Items)
			{
				try
				{
					if (!item.IsOpportunisticValidation)
					{
						await ApiClient.PostValidationAsync(ValidationSessionId, item.ItemId, Mapper.Map<ValidationCreateModel>(item));
						item.Uploaded = true;

						LocalValidation localValidation =
							await AppDataService.GetLocalValidationByIdAsync(item.ItemId, ValidationSessionId);
						localValidation.Uploaded = true;
					}
					else
					{
						await ApiClient.PostOpportunisticValidationAsync(ValidationSessionId, Mapper.Map<ValidationCreateModel>(item));
						item.Uploaded = true;

						LocalOpportunisticValidation localValidation =
							await AppDataService.GetGetLocalOpportunisticValidationByIdAsync(item.ItemId, ValidationSessionId);
						localValidation.Uploaded = true;
					}
				}
				catch (Exception e)
				{
					// TODO: Notify user
				}
			}

			await AppDataService.SaveChangesAsync();
		}

		protected override async Task InitializeOnceAsync(INavigationParameters parameters)
		{
			await base.InitializeOnceAsync(parameters);

			ValidationSessionId = (int)parameters["id"];

			await LoadLocalValidationsAsync(ValidationSessionId);

			await LocalOpportunisticValidation(ValidationSessionId);
		}

		protected async Task LoadLocalValidationsAsync(int validationSessionId)
		{
			List<LocalValidation> localValidations =
				(await AppDataService.GetLocalValidationsWhereNotUploadedByIdAsync(validationSessionId)).ToList();
			Mapper.Map(localValidations, Items);
		}
		protected async Task LocalOpportunisticValidation(int validationSessionId)
		{
			List<LocalOpportunisticValidation> localopportunisticValidations =
				(await AppDataService.GetLocalOpportunisticValidationWhereNotUploadedByIdAsync(validationSessionId)).ToList();
			 List<ItemViewModel> LocalOpportunisticItems=new List<ItemViewModel>() ;
			Mapper.Map(localopportunisticValidations, LocalOpportunisticItems);
			foreach (var item in LocalOpportunisticItems)
			{
				Items.Add(item);
			};

		}
	}
}