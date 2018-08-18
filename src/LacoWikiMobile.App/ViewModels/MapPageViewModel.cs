﻿// <copyright file="MapPageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using AutoMapper;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.Core.Sensor;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using LacoWikiMobile.App.ViewModels.Map;
	using LacoWikiMobile.App.Views;
	using Microsoft.Extensions.Localization;
	using Prism.AppModel;
	using Prism.Commands;
	using Prism.Events;
	using Prism.Navigation;
	using PropertyChanged;
	using Xamarin.Essentials;
	using Xamarin.Forms;

	public class MapPageViewModel : ViewModelBase, IApplicationLifecycleAware, ITargetPositionObserver
	{
		protected const double MinimumPointDistanceForValidation = 0.1;

		public MapPageViewModel(INavigationService navigationService, IPermissionService permissionService,
			IStringLocalizer<MapPageViewModel> stringLocalizer, IEventAggregator eventAggregator, IApiClient apiClient,
			IAppDataService appDataService, IMapper mapper, INotificationService notificationService, ISensorService sensorService)
			: base(navigationService, permissionService, stringLocalizer)
		{
			EventAggregator = eventAggregator;
			ApiClient = apiClient;
			AppDataService = appDataService;
			Mapper = mapper;
			NotificationService = notificationService;
			SensorService = sensorService;

			ValidationPointsViewModel = new ValidationPointsViewModel
			{
				Points = new ObservableCollection<ValidationPointViewModel>().OnObservableCollectionChildrenPropertyChanged(
					(sender, args) =>
					{
						if (args.PropertyName == nameof(ISelectable.Selected))
						{
							OnPointSelectedChanged((IPoint)sender);
						}
					}),
			};
		}

		// TODO: Pass from CSS to Element to ViewModel when custom CSS properties and runtime class changes are supported
		// See https://github.com/xamarin/Xamarin.Forms/issues/2891 and https://github.com/xamarin/Xamarin.Forms/issues/2678
		public Color NavigationPaneBackgroundColor
		{
			get
			{
				switch (NavigationState)
				{
					case NavigationStateEnum.NoPointSelected:
						return Color.FromHex("#009688");

					case NavigationStateEnum.Initializing:
						return Color.FromHex("#FBC02D");

					case NavigationStateEnum.Navigating:
						return Color.FromHex("#FBC02D");

					case NavigationStateEnum.PointReached:
						return Color.FromHex("#388E3C");

					default:
						throw new InvalidOperationException();
				}
			}
		}

		public NavigationStateEnum NavigationState
		{
			get
			{
				if (!PointSelected)
				{
					return NavigationStateEnum.NoPointSelected;
				}

				if (NavigationDistance == null)
				{
					return NavigationStateEnum.Initializing;
				}

				// TODO: Discuss threshold
				if (NavigationDistance < MapPageViewModel.MinimumPointDistanceForValidation)
				{
					return NavigationStateEnum.PointReached;
				}

				return NavigationStateEnum.Navigating;
			}
		}

		public string NavigationText
		{
			get
			{
				// TODO: Localization
				switch (NavigationState)
				{
					case NavigationStateEnum.NoPointSelected:
						return "Select Point";

					case NavigationStateEnum.Initializing:
						return "Initializing...";

					case NavigationStateEnum.Navigating:
						return NavigationDistance < 2 ? $"{NavigationDistance * 1000:F0} m" : $"{NavigationDistance:F2} km";

					case NavigationStateEnum.PointReached:
						return "Validate Point";

					default:
						throw new InvalidOperationException();
				}
			}
		}

		public bool PointSelected => SelectedPoint != null;

		[DependsOn(nameof(NavigationState))]
		public override Color PrimaryActionButtonBackgroundColor => base.PrimaryActionButtonBackgroundColor;

		public bool ShowNavigationDirection => NavigationState == NavigationStateEnum.Navigating;

		public bool ShowPrimaryActionButton =>
			NavigationState == NavigationStateEnum.Navigating || NavigationState == NavigationStateEnum.PointReached;

		public ICommand MapClickCommand { get; set; }

		public int NavigationDirection { get; set; }

		public double? NavigationDistance { get; set; }

		public override bool PrimaryActionButtonEnabled => NavigationState == NavigationStateEnum.PointReached;

		public IPoint SelectedPoint { get; set; }

		public ValidationPointsViewModel ValidationPointsViewModel { get; set; }

		protected IApiClient ApiClient { get; set; }

		protected IAppDataService AppDataService { get; set; }

		protected IEventAggregator EventAggregator { get; set; }

		protected IMapper Mapper { get; set; }

		protected INotificationService NotificationService { get; set; }

		protected ISensorService SensorService { get; set; }

		protected int ValidationSessionId { get; set; }

		public async void MapClickAsync()
		{
			foreach (ISelectable point in ValidationPointsViewModel.Points)
			{
				point.Selected = false;
			}

			await SensorService.UnsubscribeToTargetPositionEventsAsync(this);

			SelectedPoint = null;
			NavigationDistance = null;
		}

		public void OnDirectionChanged(double direction)
		{
			NavigationDirection = (int)direction;
		}

		public void OnDistanceChanged(double distance)
		{
			NavigationDistance = distance;
		}

		public override void OnNavigatedFrom(INavigationParameters parameters)
		{
			base.OnNavigatedFrom(parameters);

			SensorService.UnsubscribeToTargetPositionEventsAsync(this);
		}

		public void OnResume()
		{
			if (SelectedPoint != null)
			{
				SensorService.SubscribeToTargetPositionEventsAsync(this, new TargetPositionObserverOptions(new Position()
				{
					Longitude = SelectedPoint.Longitude,
					Latitude = SelectedPoint.Latitude,
				}));
			}
		}

		public void OnSleep()
		{
			SensorService.UnsubscribeToTargetPositionEventsAsync(this);
		}

		protected override async Task InitializeOnceAsync(INavigationParameters parameters)
		{
			await base.InitializeOnceAsync(parameters);

			ValidationSessionId = (int)parameters["id"];
			Title = (string)parameters["name"];

			ValidationSession validationSession = await AppDataService.GetValidationSessionByIdAsync(ValidationSessionId);
			Mapper.Map(validationSession, ValidationPointsViewModel);

			if (Connectivity.NetworkAccess == NetworkAccess.Internet)
			{
				// TODO: Localization
				NotificationService.Notify("Synchronizing points...");

				await ApiClient.GetValidationSessionSampleItemsByIdAsync(ValidationSessionId)
					.ContinueWith(async (result) =>
					{
						Mapper.Map(result.Result, ValidationPointsViewModel,
							opt => opt.Items[nameof(ValidationSession.Id)] = ValidationSessionId);

						Extent extent = ValidationPointsViewModel.Points.GroupBy(x => true)
							.Select(x => new
							{
								top = x.Max(y => y.Latitude),
								right = x.Max(y => y.Longitude),
								bottom = x.Min(y => y.Latitude),
								left = x.Min(y => y.Longitude),
							})
							.Select(x => new Extent(x.top, x.left, x.right, x.bottom))
							.Single();

						EventAggregator.GetEvent<ZoomToExtentEvent>().Publish(extent);

						AppDataService.DisableDetectChanges();

						Mapper.Map(ValidationPointsViewModel, validationSession, opt =>
						{
							opt.Items[nameof(ValidationSession)] = validationSession;
							opt.Items[nameof(LegendItem)] = validationSession.LegendItems.ToDictionary(x => x.Id, x => x);
						});

						AppDataService.EnableDetectChanges();
						await AppDataService.SaveChangesAsync();

						// TODO: Localization
						NotificationService.Notify("Points successfully synchronized!");
					});
			}

			MapClickCommand = new DelegateCommand(MapClickAsync);
		}

		protected void OnPointSelectedChanged(IPoint point)
		{
			if (point is ISelectable selectable)
			{
				// Ensure only one selected item
				if (selectable.Selected)
				{
					Helper.RunOnMainThreadIfRequired(async () =>
					{
						// TODO: Show message when result is false
						bool result = await SensorService.SubscribeToTargetPositionEventsAsync(this,
							new TargetPositionObserverOptions(new Position()
							{
								Longitude = point.Longitude,
								Latitude = point.Latitude,
							}));

						SelectedPoint = point;
					});

					foreach (ISelectable otherPoint in ValidationPointsViewModel.Points)
					{
						if (otherPoint == point)
						{
							continue;
						}

						otherPoint.Selected = false;
					}
				}
			}
		}

		protected override async Task PrimaryActionButtonTappedAsync()
		{
			await base.PrimaryActionButtonTappedAsync();

			Helper.RunOnMainThreadIfRequired(() => NavigationService.NavigateAsync(nameof(ValidatePage)));
		}
	}
}