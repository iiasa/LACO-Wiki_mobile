// <copyright file="ValidationSessionDetailPageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System.Threading.Tasks;
	using AutoMapper;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels.ValidationSessionDetail;
	using Microsoft.Extensions.Localization;
	using Plugin.Permissions.Abstractions;
	using Prism.Navigation;

	public class ValidationSessionDetailPageViewModel : ViewModelBase
	{
		public ValidationSessionDetailPageViewModel(INavigationService navigationService, IPermissionService permissionService,
			IStringLocalizer<ValidationSessionDetailPageViewModel> localizer, IApiClient apiClient, IAppDataService appDataService,
			IMapper mapper)
			: base(navigationService, permissionService, localizer)
		{
			ApiClient = apiClient;
			AppDataService = appDataService;
			Mapper = mapper;
		}

		public bool ShowDetails => !ShowLoading;

		public IApiClient ApiClient { get; set; }

		public IAppDataService AppDataService { get; set; }

		public bool ShowLoading { get; set; } = true;

		public ValidationSessionDetailViewModel ViewModel { get; set; }

		protected IMapper Mapper { get; set; }

		protected int ValidationSessionId { get; set; }

		// TODO: Disable primary action button until data is loaded
		protected override async Task ExecutePrimaryActionAsync()
		{
			await base.ExecutePrimaryActionAsync();

			if (await AppDataService.TryGetValidationSessionByIdAsync(ViewModel.Id) == null)
			{
				await AppDataService.AddValidationSessionAsync(Mapper.Map<ValidationSessionDetailViewModel, ValidationSession>(ViewModel));
			}

			await PermissionService.CheckAndRequestPermissionIfRequiredAsync(Permission.Location)
				.ContinueIfTrueWith(() =>
				{
					Helper.RunOnMainThreadIfRequired(() => { NavigationService.NavigateToMapAsync(ViewModel.Id, ViewModel.Name); });
				});
		}

		protected override async Task InitializeAsync(INavigationParameters parameters)
		{
			await base.InitializeAsync(parameters);

			// Could be null if InitializeOnceAsync redirects to AuthenticationPage
			if (ViewModel == null)
			{
				await LoadValidationSessionAsync();
			}
		}

		protected override async Task InitializeOnceAsync(INavigationParameters parameters)
		{
			await base.InitializeOnceAsync(parameters);

			Title = (string)parameters["name"];
			ValidationSessionId = (int)parameters["id"];

			ValidationSession validationSession = await AppDataService.TryGetValidationSessionByIdAsync((int)parameters["id"]);

			if (validationSession != null)
			{
				ViewModel = Mapper.Map<ValidationSessionDetailViewModel>(validationSession);
				ShowLoading = false;
			}

			await LoadValidationSessionAsync();
		}

		protected async Task LoadValidationSessionAsync()
		{
			await ApiClient.GetValidationSessionByIdAsync(ValidationSessionId)
				.ContinueWith(result =>
				{
					if (ViewModel == null)
					{
						ViewModel = Mapper.Map<ValidationSessionDetailViewModel>(result.Result);
						ShowLoading = false;
					}
					else
					{
						Mapper.Map(result.Result, ViewModel);
					}
				});
		}
	}
}