// <copyright file="ItemViewModelBase.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.Shared
{
	using System;
	using System.ComponentModel;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using Prism.Commands;
	using Xamarin.Forms;

	public class ItemViewModelBase : INotifyPropertyChanged
	{
		public ItemViewModelBase()
		{
			ItemTappedCommand = new DelegateCommand(() =>
			{
#pragma warning disable 4014
				TapItem();
#pragma warning restore 4014
			});
		}

		public event EventHandler ItemTapped;

		public event PropertyChangedEventHandler PropertyChanged;

		// Move Color to Xaml (ConditionalConverter)
		public Color BackgroundColor => IsActive ? Color.FromHex("#EEEEEE") : Color.Default;

		public bool IsActive { get; set; }

		public ICommand ItemTappedCommand { get; set; }

		protected async Task TapItem()
		{
			IsActive = true;
			await Task.Delay(10);

#pragma warning disable 4014
			Task.Run(async () =>
#pragma warning restore 4014
			{
				await Task.Delay(250);
				IsActive = false;
			});

			ItemTapped?.Invoke(this, EventArgs.Empty);
		}
	}
}