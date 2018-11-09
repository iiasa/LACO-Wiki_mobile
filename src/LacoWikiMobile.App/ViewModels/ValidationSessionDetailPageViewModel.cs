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
	using System.Windows.Input;
	using LacoWikiMobile.App.ViewModels.ValidationSessionDetail;
	using Microsoft.Extensions.Localization;
	using Plugin.Permissions.Abstractions;
	using Prism.Navigation;
	using Xamarin.Essentials;
	using Xamarin.Forms;
	using System.Collections.Generic;

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
			/*List<OfflineCacheItemViewModel> listItems = new List<OfflineCacheItemViewModel>(4);

			OfflineCacheItemViewModel cacheModel = new OfflineCacheItemViewModel();

			cacheModel.Name = "Download";
			cacheModel.Size = "56MB";
			cacheModel.Id = "0d1c0773-33a4-4896-8572-62d0cb50aa4c";
			cacheModel.ParameterCacheId = cacheModel.Id;
			if(FileManager.CacheFileExists(cacheModel.Id)) {
				cacheModel.CacheButtonText = "Delete cache";
				cacheModel.isDownloaded = true;
			}
			else {
				cacheModel.CacheButtonText = cacheModel.Name + " " + cacheModel.Size;
				cacheModel.isDownloaded = false;
			}
			listItems.Add(cacheModel);
			listItems.Add(cacheModel);
			CacheItems = listItems;
			*/


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
					System.Console.WriteLine("DEBUG - Result "+result);
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
						foreach (OfflineCacheItemViewModel cacheModel in CacheItems) {
							if(FileManager.CacheFileExists(cacheModel.Name)) {
								cacheModel.CacheButtonText = "Delete " + cacheModel.Name;
								cacheModel.ImageButton = "ic_delete";
							}
							else cacheModel.ImageButton = "ic_download";
						}

					});
			}
		}

		void DownloadTiles(OfflineCacheItemViewModel cacheButton)
		{
			System.Console.WriteLine("Download tiles");
			if (FileManager.CacheFileExists(cacheButton.Name)) {
				//remove files
				FileManager.DeleteCache(cacheButton.Name);
				cacheButton.CacheButtonText = "Download "+cacheButton.Name;
			}

			else TaskDownloadTiles(cacheButton);
		}

		async Task TaskDownloadTiles(OfflineCacheItemViewModel cacheButton)
		{

			//async download
			//string cacheId = "0d1c0773-33a4-4896-8572-62d0cb50aa4c";
			if (Connectivity.NetworkAccess == NetworkAccess.Internet)
			{
				cacheButton.CacheButtonText = "Downloading " + cacheButton.Name;

				await ApiClient.GetCacheAsync(cacheButton.Url)
					.ContinueWith(result =>
					{
						System.Console.WriteLine("Download tiles done");
						byte[] cacheBytes = result.Result;
						System.Console.WriteLine("size tiles "+cacheBytes.Length);
						FileManager.saveFileToDirectory(cacheButton.Name, cacheBytes);
						cacheButton.CacheButtonText = cacheButton.Name + " saved";
						cacheButton.ImageButton = "ic_delete";
					});
			}
		}
	}

}