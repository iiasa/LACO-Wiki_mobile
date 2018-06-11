// <copyright file="MainPageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using AutoMapper;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.ViewModels.Main;
	using LacoWikiMobile.App.Views;
	using Prism.Navigation;
	using Xamarin.Forms.Internals;

	public class MainPageViewModel : ViewModelBase
	{
		public MainPageViewModel(INavigationService navigationService, IAppDataService appDataService, IMapper mapper)
			: base(navigationService)
		{
			AppDataService = appDataService;
			Mapper = mapper;

			// TODO: LocalizationService
			Title = "Home";
		}

		public bool ShowInstructions => !Items.Any();

		public bool ShowList => Items.Any();

		public IAppDataService AppDataService { get; set; }

		// TODO: LocalizationService
		public string Instruction { get; set; } = "There is no active validation session, please add one and start validating!";

		public IEnumerable<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

		protected IMapper Mapper { get; set; }

		protected override void Initialize(INavigationParameters parameters)
		{
			AppDataService.GetValidationSessionsAsync()
				.ContinueWith(result =>
				{
					Items = Mapper.Map<IEnumerable<ItemViewModel>>(result.Result);
					Items.ForEach(x => x.ItemTapped += OnItemTapped);
				});
		}

		protected void OnItemTapped(object sender, EventArgs args)
		{
			ItemViewModel itemViewModel = ((ItemViewModel)sender);

			NavigationService.NavigateToValidationSessionDetail(itemViewModel.Id, itemViewModel.Name);
		}

		protected override void PrimaryActionButtonTapped()
		{
			base.PrimaryActionButtonTapped();

			// TODO: Block Navigation
			NavigationService.NavigateAsync(nameof(ValidationSessionOverviewPage));
		}
	}
}