// <copyright file="NavigationServiceExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System.Linq;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.Views;
	using Prism.Navigation;

	public static class NavigationServiceExtension
	{
		public static Task<INavigationResult> NavigateToMapAsync(this INavigationService navigationService, int id, string name)
		{
			return navigationService.NavigateAsync($"{navigationService.ToRelativePath(nameof(MainPage))}{nameof(MapPage)}",
				new NavigationParameters()
				{
					{ nameof(id), id },
					{ nameof(name), name },
				});
		}

		public static Task<INavigationResult> NavigateToValidatePageAsync(this INavigationService navigationService, int sampleItemId, int validationSessionId)
		{
			return navigationService.NavigateAsync(nameof(ValidatePage), new NavigationParameters()
			{
				{ nameof(sampleItemId), sampleItemId },
				{ nameof(validationSessionId), validationSessionId },
			});
		}

		public static Task<INavigationResult> NavigateToValidatePageAsync(this INavigationService navigationService, int validationSessionId)
		{
			return navigationService.NavigateAsync(nameof(ValidatePage), new NavigationParameters()
			{
				{ nameof(validationSessionId), validationSessionId },
			});
		}

		public static Task<INavigationResult> NavigateToValidationSessionDetailAsync(this INavigationService navigationService, int id,
			string name)
		{
			return navigationService.NavigateAsync(nameof(ValidationSessionDetailPage), new NavigationParameters()
			{
				{ nameof(id), id },
				{ nameof(name), name },
			});
		}

		public static Task<INavigationResult> NavigateToValidationUploadAsync(this INavigationService navigationService, int id)
		{
			return navigationService.NavigateAsync(nameof(ValidationUploadPage), new NavigationParameters()
			{
				{ nameof(id), id },
			});
		}

		public static string ToRelativePath(this INavigationService navigationService, string pageName)
		{
			return string.Concat(Enumerable.Repeat("../",
				navigationService.GetNavigationUriPath().Split('/').Reverse().ToList().IndexOf(pageName)));
		}
	}
}