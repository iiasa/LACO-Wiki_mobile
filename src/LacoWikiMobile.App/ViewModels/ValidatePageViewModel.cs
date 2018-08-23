// <copyright file="ValidatePageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using AutoMapper;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels.ValidationSessionDetail;
	using Microsoft.Extensions.Localization;
	using Prism.Commands;
	using Prism.Events;
	using Prism.Navigation;
	using PropertyChanged;
	using Xamarin.Forms;

	public class ValidatePageViewModel : ViewModelBase
	{
		public ValidatePageViewModel(INavigationService navigationService, IPermissionService permissionService,
			IStringLocalizer<ValidatePageViewModel> stringLocalizer, IEventAggregator eventAggregator, IApiClient apiClient,
			IAppDataService appDataService, IMapper mapper, INotificationService notificationService)
			: base(navigationService, permissionService, stringLocalizer)
		{
			EventAggregator = eventAggregator;
			ApiClient = apiClient;
			AppDataService = appDataService;
			Mapper = mapper;
			NotificationService = notificationService;

			LegendItems = new ObservableCollection<ItemViewModel>();
			((INotifyCollectionChanged)LegendItems).CollectionChanged += ObservableCollectionExtension.OnItemsAddedOrRemovedEventHandler(
				(sender, itemsAdded) =>
				{
					foreach (ItemViewModel item in itemsAdded)
					{
						item.ItemSelected += ItemOnItemSelected;
					}
				}, (sender, itemsRemoved) =>
				{
					foreach (ItemViewModel item in itemsRemoved)
					{
						item.ItemSelected -= ItemOnItemSelected;
					}
				}, (sender) => { Helper.RunOnMainThreadIfRequired(() => throw new InvalidOperationException()); });

			IsCorrectTappedCommand = new DelegateCommand(() =>
			{
				if (Correct != true)
				{
					SelectedLegendItem = null;

					foreach (ItemViewModel itemViewModel in LegendItems)
					{
						itemViewModel.IsSelected = false;
					}
				}

				Correct = Correct != true ? (bool?)true : null;
			});
			IsNotCorrectTappedCommand = new DelegateCommand(() =>
			{
				if (Correct == false)
				{
					SelectedLegendItem = null;

					foreach (ItemViewModel itemViewModel in LegendItems)
					{
						itemViewModel.IsSelected = false;
					}
				}

				Correct = Correct != false ? (bool?)false : null;
			});
		}

		// TODO: Pass from CSS to Element to ViewModel when runtime class changes are supported
		// See https://github.com/xamarin/Xamarin.Forms/issues/2678
		public Color CorrectButtonBackgroundColor => Correct == true ? Color.FromHex("#43A047") : Color.FromHex("#C8E6C9");

		public Color CorrectButtonBorderColor => Correct == true ? Color.FromHex("#1B5E20") : Color.FromHex("C8E6C9");

		public Thickness CorrectButtonMargin => Correct == true ? new Thickness(0) : new Thickness(2);

		public Color CorrectButtonTextColor => Correct == true ? Color.White : Color.Default;

		public Color NotCorrectButtonBackgroundColor => Correct == false ? Color.FromHex("#EF5350") : Color.FromHex("#FFEBEE");

		public Color NotCorrectButtonBorderColor => Correct == false ? Color.FromHex("#B71C1C") : Color.FromHex("#FFEBEE");

		public Thickness NotCorrectButtonMargin => Correct == false ? new Thickness(0) : new Thickness(2);

		public Color NotCorrectButtonTextColor => Correct == false ? Color.White : Color.Default;

		[DependsOn(nameof(PrimaryActionButtonEnabled))]
		public override Color PrimaryActionButtonBackgroundColor => base.PrimaryActionButtonBackgroundColor;

		public bool ShowCorrectSection => ValidationMethod != ValidationMethodEnum.Blind;

		public bool ShowLegendSection =>
			ValidationMethod == ValidationMethodEnum.Blind ||
			(ValidationMethod == ValidationMethodEnum.EnhancedPlausibility && Correct == false);

		public bool? Correct { get; set; }

		public ICommand IsCorrectTappedCommand { get; set; }

		public ICommand IsNotCorrectTappedCommand { get; set; }

		public ItemViewModel LegendItem { get; set; }

		public ICollection<ItemViewModel> LegendItems { get; set; }

		public override bool PrimaryActionButtonEnabled
		{
			get
			{
				if (ValidationMethod == ValidationMethodEnum.Blind && SelectedLegendItem != null)
				{
					return true;
				}

				if (ValidationMethod == ValidationMethodEnum.Plausibility && Correct != null)
				{
					return true;
				}

				if (ValidationMethod == ValidationMethodEnum.EnhancedPlausibility &&
					(Correct == true || (Correct == false && SelectedLegendItem != null)))
				{
					return true;
				}

				return false;
			}
		}

		public int SampleItemId { get; protected set; }

		public ItemViewModel SelectedLegendItem { get; set; }

		public ValidationMethodEnum ValidationMethod { get; set; }

		public int ValidationSessionId { get; protected set; }

		protected IApiClient ApiClient { get; }

		protected IAppDataService AppDataService { get; }

		protected IEventAggregator EventAggregator { get; }

		protected IMapper Mapper { get; }

		protected INotificationService NotificationService { get; }

		protected override async Task InitializeOnceAsync(INavigationParameters parameters)
		{
			await base.InitializeOnceAsync(parameters);

			ValidationSessionId = (int)parameters["validationSessionId"];
			SampleItemId = (int)parameters["sampleItemId"];

			// TODO: Localization
			Title = $"Sample #{SampleItemId}";

			SampleItem sampleItem = await AppDataService.GetSampleItemByIdAsync(SampleItemId, ValidationSessionId);

			ValidationMethod = sampleItem.ValidationSession.ValidationMethod;
			LegendItem = Mapper.Map<ItemViewModel>(sampleItem.LegendItem);
			Mapper.Map(sampleItem.ValidationSession.LegendItems, LegendItems);
		}

		protected void ItemOnItemSelected(object sender, EventArgs e)
		{
			ItemViewModel viewModel = (ItemViewModel)sender;

			SelectedLegendItem = null;

			if (viewModel.IsSelected == false)
			{
				return;
			}

			SelectedLegendItem = viewModel;

			foreach (ItemViewModel itemViewModel in LegendItems.Where(x => !ReferenceEquals(x, viewModel)))
			{
				itemViewModel.IsSelected = false;
			}
		}

		protected override async Task PrimaryActionButtonTappedAsync()
		{
			await base.PrimaryActionButtonTappedAsync();

			await AppDataService.AddLocalValidationAsync(Mapper.Map<LocalValidation>(this));

			// TODO: Localization
			NotificationService.Notify("Validation saved locally.");
			await NavigationService.GoBackAsync();
		}
	}
}