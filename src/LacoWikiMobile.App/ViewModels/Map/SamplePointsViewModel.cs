// <copyright file="SamplePointsViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.Map
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;

	public class SamplePointsViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ICollection<SamplePointViewModel> Points { get; set; } = new ObservableCollection<SamplePointViewModel>();
	}
}