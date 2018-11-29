// <copyright file="ValidationSessionDetail.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.ValidationSessionDetail
{
	using System.ComponentModel;

	public class OfflineCacheItemViewModel : INotifyPropertyChanged
	{
		public OfflineCacheItemViewModel()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string CacheButtonText { get; set; }

		public string Name { get; set; }

		public string Size { get; set; }

		public string Url { get; set; }

		public string ImageButton { get; set; }

		public string Path { get; set; }
	}
}