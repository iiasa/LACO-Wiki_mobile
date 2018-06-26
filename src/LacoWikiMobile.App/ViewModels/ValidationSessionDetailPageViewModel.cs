﻿// <copyright file="ValidationSessionDetailPageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System.Threading.Tasks;
	using AutoMapper;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels.ValidationSessionDetail;
	using Prism.Navigation;

	public class ValidationSessionDetailPageViewModel : ViewModelBase
	{
		public ValidationSessionDetailPageViewModel(INavigationService navigationService, IApiClient apiClient,
			IAppDataService appDataService, IMapper mapper)
			: base(navigationService)
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

		protected override async Task InitializeAsync(INavigationParameters parameters)
		{
			await base.InitializeAsync(parameters);

			Title = (string)parameters["name"];

			await ApiClient.GetValidationSessionByIdAsync((int)parameters["id"])
				.ContinueWith(result =>
				{
					ViewModel = Mapper.Map<ValidationSessionDetailViewModel>(result.Result);
					ShowLoading = false;
				});
		}

		// TODO: Disable primary action button until data is loaded
		protected override void PrimaryActionButtonTapped()
		{
			base.PrimaryActionButtonTapped();

			AppDataService.AddValidationSessionAsync(Mapper.Map<ValidationSessionDetailViewModel, ValidationSession>(ViewModel));

			NavigationService.NavigateAsync($"../../");
		}
	}
}