// <copyright file="NavigationService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System;
	using System.Threading.Tasks;
	using Prism.Common;
	using Prism.Navigation;
	using Xamarin.Forms;

	// Prevent calling INavigationService methods outside of UI thread to prevent bugs
	public class NavigationService : INavigationService, IPageAware
	{
		public NavigationService(INavigationService navigationService)
		{
			WrappedNavigationService = navigationService;
		}

		public Page Page
		{
			get => ((IPageAware)WrappedNavigationService).Page;
			set => ((IPageAware)WrappedNavigationService).Page = value;
		}

		protected INavigationService WrappedNavigationService { get; set; }

		public Task<INavigationResult> GoBackAsync()
		{
			EnsureIsInvokeRequiredIsFalse();

			return WrappedNavigationService.GoBackAsync();
		}

		public Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
		{
			EnsureIsInvokeRequiredIsFalse();

			return WrappedNavigationService.GoBackAsync(parameters);
		}

		public Task<INavigationResult> NavigateAsync(Uri uri)
		{
			EnsureIsInvokeRequiredIsFalse();

			return WrappedNavigationService.NavigateAsync(uri);
		}

		public Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters)
		{
			EnsureIsInvokeRequiredIsFalse();

			return WrappedNavigationService.NavigateAsync(uri, parameters);
		}

		public Task<INavigationResult> NavigateAsync(string name)
		{
			EnsureIsInvokeRequiredIsFalse();

			return WrappedNavigationService.NavigateAsync(name);
		}

		public Task<INavigationResult> NavigateAsync(string name, INavigationParameters parameters)
		{
			EnsureIsInvokeRequiredIsFalse();

			return WrappedNavigationService.NavigateAsync(name, parameters);
		}

		protected void EnsureIsInvokeRequiredIsFalse()
		{
			if (Device.IsInvokeRequired)
			{
				throw new InvalidOperationException("Navigation should be executed on Main thread.");
			}
		}
	}
}