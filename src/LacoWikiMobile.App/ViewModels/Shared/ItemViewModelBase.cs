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
				IsActive = true;

				Task.Run(async () =>
				{
					await Task.Delay(250);
					IsActive = false;
				});

				ItemTapped?.Invoke(this, EventArgs.Empty);
			});
		}

		public event EventHandler ItemTapped;

		public event PropertyChangedEventHandler PropertyChanged;

		// Move Color to Xaml (ConditionalConverter)
		public Color BackgroundColor => IsActive ? Color.FromHex("#EEEEEE") : Color.Default;

		public bool IsActive { get; set; }

		public ICommand ItemTappedCommand { get; set; }
	}
}