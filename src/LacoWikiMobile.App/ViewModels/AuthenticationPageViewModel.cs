// <copyright file="AuthenticationPageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using Microsoft.Extensions.Localization;
	using Prism.Commands;
	using Prism.Navigation;

	public class AuthenticationPageViewModel : ViewModelBase
	{
		public AuthenticationPageViewModel(INavigationService navigationService, IStringLocalizer<AuthenticationPageViewModel> localizer,
			IApiAuthentication apiAuthentication, INotificationService notificationService)
			: base(navigationService, localizer)
		{
			NavigationService = navigationService;
			ApiAuthentication = apiAuthentication;
			NotificationService = notificationService;

			ApiAuthentication.Authenticated += async (sender, args) =>
			{
				await NavigationService.GoBackAsync();

				// TODO: LocalizationService
				NotificationService.Notify("Login successful");
			};

			// TODO: LocalizationService
			Title = "Login";

			AuthenticateWithGeoWikiCommand = new DelegateCommand(() => { ApiAuthentication.AuthenticateWithGeoWiki(); });

			AuthenticateWithGoogleCommand = new DelegateCommand(() => { ApiAuthentication.AuthenticateWithGoogle(); });

			AuthenticateWithFacebookCommand = new DelegateCommand(() => { ApiAuthentication.AuthenticateWithFacebook(); });
		}

		public ICommand AuthenticateWithFacebookCommand { get; set; }

		public ICommand AuthenticateWithGeoWikiCommand { get; set; }

		public ICommand AuthenticateWithGoogleCommand { get; set; }

		protected IApiAuthentication ApiAuthentication { get; set; }

		protected INotificationService NotificationService { get; set; }

		protected override async Task InitializeAsync(INavigationParameters parameters)
		{
			await base.InitializeAsync(parameters);

			if (parameters.ContainsKey("useLastKnownProvider") && (bool)parameters["useLastKnownProvider"])
			{
				string providerName = await ApiAuthentication.GetProviderNameAsync();

				switch (providerName)
				{
					case "GeoWiki":
						ApiAuthentication.AuthenticateWithGeoWiki();
						break;

					case "Google":
						ApiAuthentication.AuthenticateWithGoogle();
						break;

					case "Facebook":
						ApiAuthentication.AuthenticateWithFacebook();
						break;

					default:
						throw new ArgumentException(nameof(providerName));
				}
			}
		}
	}
}