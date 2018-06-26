﻿// <copyright file="NavigationServiceExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using LacoWikiMobile.App.Views;
	using Prism.Navigation;

	public static class NavigationServiceExtension
	{
		public static void NavigateToValidationSessionDetail(this INavigationService navigationService, int id, string name)
		{
			navigationService.NavigateAsync(nameof(ValidationSessionDetailPage), new NavigationParameters()
			{
				{ "id", id },
				{ "name", name },
			});
		}
	}
}