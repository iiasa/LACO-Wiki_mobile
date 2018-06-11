// <copyright file="ViewModelBase.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System.ComponentModel;
	using System.Threading.Tasks;
	using System.Windows.Input;
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

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			// This method will always be called, fallback for Initialize calls if not called by OnNavigating
			if (!InitializedOnce)
			{
				InitializeOnce(parameters);
			}

			if (!Initialized)
			{
				Initialize(parameters);
			}

			InitializedOnce = true;
			Initialized = false;
		}

		public void OnNavigatingTo(INavigationParameters parameters)
		{
			// This method will not be called when using Hardware Buttons, so prefer OnNavigating but fallback for OnNavigated
			if (!InitializedOnce)
			{
				InitializeOnce(parameters);
			}

			Initialize(parameters);

			Initialized = true;
			InitializedOnce = true;
		}

		protected virtual void Initialize(INavigationParameters parameters)
		{
		}

		protected virtual void InitializeOnce(INavigationParameters parameters)
		{
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