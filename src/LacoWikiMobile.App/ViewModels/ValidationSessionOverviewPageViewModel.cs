// <copyright file="ValidationSessionOverviewPageViewModel.cs" company="IIASA">
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
	using LacoWikiMobile.App.ViewModels.ValidationSessionOverview;
	using Microsoft.Extensions.Localization;
	using Prism.Navigation;
	using Xamarin.Forms.Internals;

	public class ValidationSessionOverviewPageViewModel : ViewModelBase
	{
		public ValidationSessionOverviewPageViewModel(INavigationService navigationService, IPermissionService permissionService,
			IStringLocalizer<ValidationSessionOverviewPageViewModel> localizer, IApiClient apiClient, IMapper mapper)
			: base(navigationService, permissionService, localizer)
		{
			ApiClient = apiClient;
			Mapper = mapper;

			// TODO: LocalizationService
			Title = "Overview";
		}

		public bool ShowInstructions => !ShowLoading && !Items.Any();

		public bool ShowList => !ShowLoading && Items.Any();

		public IApiClient ApiClient { get; set; }

		public IEnumerable<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

		public bool ShowLoading { get; set; } = true;

		protected IMapper Mapper { get; set; }

		protected override async Task InitializeAsync(INavigationParameters parameters)
		{
			await base.InitializeAsync(parameters);

			await ApiClient.GetValidationSessionsAsync()
				.ContinueWith(result =>
				{
					Items = Mapper.Map<IEnumerable<ItemViewModel>>(result.Result);
					ShowLoading = false;

					Items.ForEach(x => x.ItemTapped += OnItemTapped);
				});
		}

		protected void OnItemTapped(object sender, EventArgs args)
		{
			ItemViewModel itemViewModel = (ItemViewModel)sender;

			NavigationService.NavigateToValidationSessionDetail(itemViewModel.Id, itemViewModel.Name);
		}
	}
}