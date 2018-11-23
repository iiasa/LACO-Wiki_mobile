﻿// <copyright file="ValidationSessionDetailPageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using AutoMapper;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data;
	using LacoWikiMobile.App.Core.Data.Entities;
	using LacoWikiMobile.App.ViewModels.Map;
	using LacoWikiMobile.App.ViewModels.ValidationSessionDetail;
	using Microsoft.Extensions.Localization;
	using Plugin.Permissions.Abstractions;
	using Prism.Navigation;
	using Xamarin.Essentials;
	using Xamarin.Forms;

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
			OnClickDownloadTilesCommand = new Command<OfflineCacheItemViewModel>(DownloadTiles);
		}

		public bool ShowDetails => !ShowLoading;

		public IApiClient ApiClient { get; set; }

		public IAppDataService AppDataService { get; set; }

		public bool ShowLoading { get; set; } = true;

		public ICommand OnClickDownloadTilesCommand { get; private set; }

		public IEnumerable<OfflineCacheItemViewModel> CacheItems { get; set; }

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

			// InitializeAsync could be called a second time if InitializeOnceAsync redirects to AuthenticationPage
			await LoadValidationSessionAsync();
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
		}

		protected async Task LoadValidationSessionAsync()
		{
			if (Connectivity.NetworkAccess == NetworkAccess.Internet)
			{
				await ApiClient.GetValidationSessionByIdAsync(ValidationSessionId)
					.ContinueWith(result =>
					{
						System.Console.WriteLine("DEBUG - Result " + result);
						if (ViewModel == null)
						{
							ViewModel = Mapper.Map<ValidationSessionDetailViewModel>(result.Result);
							ShowLoading = false;
						}
						else
						{
							// TODO: Save updated progress
							Mapper.Map(result.Result, ViewModel);
						}

						//handle offline cache
						//System.Console.WriteLine("DEBUG - Result cache " + result);
						CacheItems = ViewModel.OfflineCaches;
						foreach (OfflineCacheItemViewModel cacheModel in CacheItems)
						{
							if (FileManager.CacheFileExists(cacheModel.Name))
							{
								cacheModel.CacheButtonText = "Delete " + cacheModel.Name;
								cacheModel.ImageButton = "ic_delete";
							}
							else cacheModel.ImageButton = "ic_download";
						}

						UpdateLayers();
					});
			}


		}

		private void UpdateLayers()
		{
			LayerService.Reset();
			LayerItemViewModel layer;

			layer = LayerService.AddLayerPoints("Points", true);

			layer = LayerService.AddLayerRaster("GoogleMap", true,true);
			int cpt = 10;
			foreach (OfflineCacheItemViewModel cacheModel in CacheItems)
			{
				if (FileManager.CacheFileExists(cacheModel.Name))
				{
					layer = LayerService.AddLayerRaster(cacheModel.Name, true, false);
				}
				else
				{
					layer = LayerService.AddLayerRaster(cacheModel.Name , false, false);
				}
			}
		}

		public void DownloadTiles(OfflineCacheItemViewModel cacheButton)
		{
			if (FileManager.CacheFileExists(cacheButton.Name))
			{
				//remove files
				FileManager.DeleteCache(cacheButton.Name);
				cacheButton.CacheButtonText = "Download " + cacheButton.Name;
				cacheButton.ImageButton = "ic_download";
			}

			else TaskDownloadTiles(cacheButton);
		}

		private async Task TaskDownloadTiles(OfflineCacheItemViewModel cacheButton)
		{

			//async download
			if (Connectivity.NetworkAccess == NetworkAccess.Internet)
			{
				cacheButton.CacheButtonText = "Downloading " + cacheButton.Name;
				cacheButton.ImageButton = "ic_download";
				await ApiClient.GetCacheAsync(cacheButton.Url)
					.ContinueWith(result =>
					{
						if (!result.IsFaulted)
						{
							System.Console.WriteLine("Download tiles done");
							byte[] cacheBytes = result.Result;
							System.Console.WriteLine("size tiles " + cacheBytes.Length);
							FileManager.saveFileToDirectory(cacheButton.Name, cacheBytes);
							cacheButton.CacheButtonText = cacheButton.Name + " saved";
							cacheButton.ImageButton = "ic_delete";
						}

					});
			}
		}
	}

}