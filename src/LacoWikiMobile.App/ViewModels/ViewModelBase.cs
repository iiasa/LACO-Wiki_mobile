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
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Views;
	using Prism.Commands;
	using Prism.Navigation;

	public class ViewModelBase : INotifyPropertyChanged, INavigatingAware, INavigatedAware
	{
		public ViewModelBase(INavigationService navigationService)
		{
			NavigationService = navigationService;

			// TODO: Move to base class, rename to Primary Action Button or similar
			PrimaryActionButtonTappedCommand = new DelegateCommand(PrimaryActionButtonTapped);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsPrimaryActionButtonActive { get; set; }

		public ICommand PrimaryActionButtonTappedCommand { get; set; }

		public string Title { get; set; }

		protected INavigationService NavigationService { get; set; }

		private bool Initialized { get; set; }

		private bool InitializedOnce { get; set; }

		private bool WasNotAuthenticatedThrown { get; set; }

		private bool WasTokenExpiredThrown { get; set; }

		public virtual void OnNavigatedFrom(INavigationParameters parameters)
		{
		}

		public virtual async void OnNavigatedTo(INavigationParameters parameters)
		{
			// This method will always be called, fallback for Initialize calls if not called by OnNavigating
			if (!InitializedOnce)
			{
				await RunAndHandleExceptionsAsync(() => InitializeOnceAsync(parameters));
			}

			if (!Initialized)
			{
				await RunAndHandleExceptionsAsync(() => InitializeAsync(parameters));
			}

			if (WasNotAuthenticatedThrown)
			{
				await NavigationService.NavigateAsync(nameof(AuthenticationPage));
			}

			if (WasTokenExpiredThrown)
			{
				await NavigationService.NavigateAsync(nameof(AuthenticationPage), new NavigationParameters()
				{
					{ "useLastKnownProvider", true },
				});
			}

			InitializedOnce = true;
			Initialized = false;

			WasNotAuthenticatedThrown = false;
			WasTokenExpiredThrown = false;
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
			////catch (Exception e)
			////{
			////}
		}

		public virtual async void OnNavigatingTo(INavigationParameters parameters)
		{
			// This method will not be called when using Hardware Buttons, so prefer OnNavigating but fallback for OnNavigated
			if (!InitializedOnce)
			{
				await RunAndHandleExceptionsAsync(() => InitializeOnceAsync(parameters));
			}

			if (!Initialized)
			{
				await RunAndHandleExceptionsAsync(() => InitializeAsync(parameters));
			}

			Initialized = true;
			InitializedOnce = true;
		}

		protected virtual Task InitializeAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		protected virtual Task InitializeOnceAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		protected virtual void PrimaryActionButtonTapped()
		{
			IsPrimaryActionButtonActive = true;

			Task.Run(async () =>
			{
				await Task.Delay(250);
				IsPrimaryActionButtonActive = false;
			});
		}
	}
}