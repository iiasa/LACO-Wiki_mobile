// <copyright file="ViewModelBase.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System.ComponentModel;

	public class ViewModelBase : INotifyPropertyChanged
	{
		public ViewModelBase()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string Title { get; set; }
	}
}