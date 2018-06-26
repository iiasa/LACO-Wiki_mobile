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
	using System.Threading.Tasks;
	using AutoMapper;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels.Main;
	using LacoWikiMobile.App.Views;
	using Microsoft.Extensions.Localization;
	using Prism.Navigation;
	using Xamarin.Forms.Internals;

	public class MainPageViewModel : ViewModelBase
	{
		public MainPageViewModel(INavigationService navigationService, IStringLocalizer<MainPageViewModel> localizer,
			IApiAuthentication apiAuthentication, IAppDataService appDataService, IMapper mapper)
			: base(navigationService, localizer)
		{
			ApiAuthentication = apiAuthentication;
			AppDataService = appDataService;
			Mapper = mapper;

			Title = Localizer[nameof(Title)];
			Instruction = Localizer[nameof(Instruction)];
		}

		public string Instruction { get; }

		public bool ShowInstructions => !Items.Any();

		public bool ShowList => Items.Any();

		public IApiAuthentication ApiAuthentication { get; set; }

		public IAppDataService AppDataService { get; set; }

		public IEnumerable<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

		protected IMapper Mapper { get; set; }

		protected override async Task InitializeAsync(INavigationParameters parameters)
		{
			await base.InitializeAsync(parameters);

			IEnumerable<ValidationSession> validationSessions = await AppDataService.GetValidationSessionsAsync();

			Items = Mapper.Map<IEnumerable<ItemViewModel>>(validationSessions);
			Items.ForEach(x => x.ItemTapped += OnItemTapped);
		}

		protected void OnItemTapped(object sender, EventArgs args)
		{
			ItemViewModel itemViewModel = (ItemViewModel)sender;

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