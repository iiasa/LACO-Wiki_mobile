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
		public MainPageViewModel(INavigationService navigationService, IPermissionService permissionService,
			IStringLocalizer<MainPageViewModel> localizer, IApiAuthentication apiAuthentication, IAppDataService appDataService,
			IMapper mapper)
			: base(navigationService, permissionService, localizer)
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

		public bool ShowSecondaryAction => !ShowPrimaryAction;

		public IApiAuthentication ApiAuthentication { get; set; }

		public IAppDataService AppDataService { get; set; }

		public ICollection<ItemViewModel> Items { get; set; } = new List<ItemViewModel>();

		public bool ShowPrimaryAction { get; set; } = true;

		protected IMapper Mapper { get; set; }

		protected override async Task ExecutePrimaryActionAsync()
		{
			await base.ExecutePrimaryActionAsync();

			if (ShowPrimaryAction)
			{
				// TODO: Block Navigation
				await NavigationService.NavigateAsync(nameof(ValidationSessionOverviewPage));
			}
			else
			{
				foreach (ItemViewModel itemViewModel in Items.Where(x => x.IsChecked).ToList())
				{
					await AppDataService.TryRemoveValidationSessionAsync(itemViewModel.Id);
					Items.Remove(itemViewModel);
				}
			}
		}

		protected override async Task InitializeAsync(INavigationParameters parameters)
		{
			await base.InitializeAsync(parameters);
			await LoadValidationSessionsAsync();
		}

		protected void ItemTapped(object sender, EventArgs args)
		{
			ItemViewModel itemViewModel = (ItemViewModel)sender;

			NavigationService.NavigateToValidationSessionDetailAsync(itemViewModel.Id, itemViewModel.Name);
		}

		protected async Task LoadValidationSessionsAsync()
		{
			if (await ApiAuthentication.IsAuthenticatedAsync())
			{
				await AppDataService.EnsureUserExistsAsync();
			}

			IEnumerable<ValidationSession> validationSessions = await AppDataService.GetValidationSessionsAsync();

			Items = new ObservableCollection<ItemViewModel>(Mapper.Map<IEnumerable<ItemViewModel>>(validationSessions)).OnPropertyChanged(
					(sender, args) =>
					{
						OnPropertyChanged(nameof(ShowInstructions));
						OnPropertyChanged(nameof(ShowList));

						ShowPrimaryAction = !Items.Any(x => x.IsChecked);
					})
				.OnChildrenPropertyChanged((sender, args) => { ShowPrimaryAction = !Items.Any(x => x.IsChecked); });

			Items.ForEach(x => { x.ItemTapped += ItemTapped; });
		}
	}
}