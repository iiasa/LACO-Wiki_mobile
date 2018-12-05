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
	using Xamarin.Forms;

	public class ViewModelBase : INotifyPropertyChanged, INavigatingAware, INavigatedAware
	{

		public ViewModelBase(INavigationService navigationService, IPermissionService permissionService, IStringLocalizer stringLocalizer)
		{
			NavigationService = navigationService;
			PermissionService = permissionService;
			Localizer = stringLocalizer;

			// TODO: Move to base class, rename to Primary Action Button or similar
			PrimaryActionButtonTappedCommand = new DelegateCommand(() =>
			{
#pragma warning disable 4014
				PrimaryActionButtonTappedAsync();
#pragma warning restore 4014
			}, () => PrimaryActionButtonEnabled).ObservesProperty(() => PrimaryActionButtonEnabled);

		}

		public event PropertyChangedEventHandler PropertyChanged;

		// TODO: Pass from CSS to Element to ViewModel when custom CSS properties and runtime class changes are supported
		// See https://github.com/xamarin/Xamarin.Forms/issues/2891 and https://github.com/xamarin/Xamarin.Forms/issues/2678
		public virtual Color PrimaryActionButtonBackgroundColor
		{
			get
			{
				if (!PrimaryActionButtonEnabled)
				{
					return Color.FromHex("#E0E0E0");
				}

				if (!IsPrimaryActionButtonActive)
				{
					return Color.FromHex("#673AB7");
				}

				return Color.FromHex("#311B92");
			}
		}

		public virtual bool IsPrimaryActionButtonActive { get; set; }

		public virtual bool PrimaryActionButtonEnabled { get; set; } = true;

		public ICommand PrimaryActionButtonTappedCommand { get; set; }

		public string Title { get; set; }

		protected IStringLocalizer Localizer { get; set; }

		protected INavigationService NavigationService { get; set; }

		protected IPermissionService PermissionService { get; set; }

		private bool Initialized { get; set; }

		private bool InitializedOnce { get; set; }

		private bool OnNavigatedToCalled { get; set; }

		private bool WasNotAuthenticatedThrown { get; set; }

		private bool WasTokenExpiredThrown { get; set; }

		public virtual void OnNavigatedFrom(INavigationParameters parameters)
		{
		}

		public virtual void OnNavigatedTo(INavigationParameters parameters)
		{
			OnNavigatedToCalled = true;

			Task<bool> task = Task.FromResult(true);

			// This method will not be called when using Hardware Buttons, so prefer OnNavigating but fallback for OnNavigated
			if (!InitializedOnce)
			{
				InitializedOnce = true;
				task = task.ContinueWith(async (r) => await RunAndHandleExceptionsAsync(InitializeOnceAsync(parameters))).Unwrap();
			}

			if (!Initialized)
			{
				Initialized = true;
				task = task.ContinueIfTrueWith(async () => await RunAndHandleExceptionsAsync(InitializeAsync(parameters)));
			}

			InitializedOnce = true;
			Initialized = false;

			task.ContinueWith(r => HandleExceptions());
		}

		public virtual void OnNavigatingTo(INavigationParameters parameters)
		{
			OnNavigatedToCalled = false;

			Task<bool> task = Task.FromResult(true);

			// This method will not be called when using Hardware Buttons, so prefer OnNavigating but fallback for OnNavigated
			if (!InitializedOnce)
			{
				InitializedOnce = true;
				task = task.ContinueWith(async (r) => await RunAndHandleExceptionsAsync(InitializeOnceAsync(parameters))).Unwrap();
			}

			if (!Initialized)
			{
				Initialized = true;
				task = task.ContinueIfTrueWith(async () => await RunAndHandleExceptionsAsync(InitializeAsync(parameters)));
			}

			task.ContinueWith(r =>
			{
				// If OnNavigatedTo wasn't called, let OnNavigatedTo handle the exceptions
				if (OnNavigatedToCalled)
				{
					HandleExceptions();
				}
			});
		}

		public virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual Task ExecutePrimaryActionAsync()
		{
			return Task.CompletedTask;
		}

		protected void HandleExceptions()
		{
			if (WasNotAuthenticatedThrown)
			{
				WasNotAuthenticatedThrown = false;

				Helper.RunOnMainThreadIfRequired(() => NavigationService.NavigateAsync(nameof(AuthenticationPage)));
			}

			if (WasTokenExpiredThrown)
			{
				WasTokenExpiredThrown = false;

				Helper.RunOnMainThreadIfRequired(() => NavigationService.NavigateAsync(nameof(AuthenticationPage),
					new NavigationParameters()
					{
						{ "useLastKnownProvider", true },
					}));
			}
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

			try
			{
				await ExecutePrimaryActionAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);

				// TODO: Swallow and ignore?
				Helper.RunOnMainThreadIfRequired(() => throw e);
			}
		}

		protected async Task<bool> RunAndHandleExceptionsAsync(Task task)
		{
			try
			{
				await task;
			}
			catch (AggregateException e)
			{
				AggregateException aggregateException = e.Flatten();

				if (aggregateException.InnerExceptions.OfType<NotAuthenticatedException>().Any())
				{
					WasNotAuthenticatedThrown = true;
					return false;
				}
				else if (aggregateException.InnerExceptions.OfType<TokenExpiredException>().Any())
				{
					WasTokenExpiredThrown = true;
					return false;
				}
				else
				{
					Console.WriteLine(e.ToString());

					// TODO: Swallow and ignore?
						Helper.RunOnMainThreadIfRequired(() => throw e);
				}
			}
			catch (NotAuthenticatedException)
			{
				WasNotAuthenticatedThrown = true;
				return false;
			}
			catch (TokenExpiredException)
			{
				WasTokenExpiredThrown = true;
				return false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());

				// TODO: Swallow and ignore?
				Helper.RunOnMainThreadIfRequired(() => throw e);
			}

			return true;
		}
	}
}