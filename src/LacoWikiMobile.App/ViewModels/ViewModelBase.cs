// <copyright file="ViewModelBase.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System;
	using System.ComponentModel;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Views;
	using Microsoft.Extensions.Localization;
	using Prism.Commands;
	using Prism.Navigation;

	public class ViewModelBase : INotifyPropertyChanged, INavigatingAware, INavigatedAware
	{
		public ViewModelBase(INavigationService navigationService, IStringLocalizer stringLocalizer)
		{
			NavigationService = navigationService;
			Localizer = stringLocalizer;

			// TODO: Move to base class, rename to Primary Action Button or similar
			PrimaryActionButtonTappedCommand = new DelegateCommand(() =>
			{
#pragma warning disable 4014
				PrimaryActionButtonTappedAsync();
#pragma warning restore 4014
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsPrimaryActionButtonActive { get; set; }

		public ICommand PrimaryActionButtonTappedCommand { get; set; }

		public string Title { get; set; }

		protected IStringLocalizer Localizer { get; set; }

		protected INavigationService NavigationService { get; set; }

		private bool Initialized { get; set; }

		private bool InitializedOnce { get; set; }

		private bool WasNotAuthenticatedThrown { get; set; }

		private bool WasTokenExpiredThrown { get; set; }

		public virtual void OnNavigatedFrom(INavigationParameters parameters)
		{
		}

		public virtual void OnNavigatedTo(INavigationParameters parameters)
		{
			// This method will always be called, fallback for Initialize calls if not called by OnNavigating
			if (!InitializedOnce)
			{
#pragma warning disable 4014
				RunAndHandleExceptionsAsync(() => InitializeOnceAsync(parameters));
#pragma warning restore 4014

				InitializedOnce = true;
			}

			if (!Initialized)
			{
#pragma warning disable 4014
				RunAndHandleExceptionsAsync(() => InitializeAsync(parameters));
#pragma warning restore 4014
			}

			InitializedOnce = true;
			Initialized = false;

			if (WasNotAuthenticatedThrown)
			{
				Helper.RunOnMainThreadIfRequired(() => NavigationService.NavigateAsync(nameof(AuthenticationPage)));
			}

			if (WasTokenExpiredThrown)
			{
				Helper.RunOnMainThreadIfRequired(() => NavigationService.NavigateAsync(nameof(AuthenticationPage),
					new NavigationParameters()
					{
						{ "useLastKnownProvider", true },
					}));
			}

			WasNotAuthenticatedThrown = false;
			WasTokenExpiredThrown = false;
		}

		public virtual void OnNavigatingTo(INavigationParameters parameters)
		{
			// This method will not be called when using Hardware Buttons, so prefer OnNavigating but fallback for OnNavigated
			if (!InitializedOnce)
			{
				InitializedOnce = true;

#pragma warning disable 4014
				RunAndHandleExceptionsAsync(() => InitializeOnceAsync(parameters));
#pragma warning restore 4014
			}

			if (!Initialized)
			{
				Initialized = true;

#pragma warning disable 4014
				RunAndHandleExceptionsAsync(() => InitializeAsync(parameters));
#pragma warning restore 4014
			}
		}

		public virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual Task InitializeAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		protected virtual Task InitializeOnceAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		protected virtual async Task PrimaryActionButtonTappedAsync()
		{
			IsPrimaryActionButtonActive = true;
			await Task.Delay(10);

#pragma warning disable 4014
			Task.Run(async () =>
#pragma warning restore 4014
			{
				await Task.Delay(250);
				IsPrimaryActionButtonActive = false;
			});
		}

		protected async Task RunAndHandleExceptionsAsync(Func<Task> func)
		{
			try
			{
				await func();
			}
			catch (AggregateException e)
			{
				AggregateException aggregateException = e.Flatten();

				if (aggregateException.InnerExceptions.OfType<NotAuthenticatedException>().Any())
				{
					WasNotAuthenticatedThrown = true;
				}
				else if (aggregateException.InnerExceptions.OfType<TokenExpiredException>().Any())
				{
					WasTokenExpiredThrown = true;
				}
				else
				{
					throw;
				}
			}
			catch (NotAuthenticatedException)
			{
				WasNotAuthenticatedThrown = true;
			}
			catch (TokenExpiredException)
			{
				WasTokenExpiredThrown = true;
			}
			catch (Exception e)
			{
			}
		}
	}
}