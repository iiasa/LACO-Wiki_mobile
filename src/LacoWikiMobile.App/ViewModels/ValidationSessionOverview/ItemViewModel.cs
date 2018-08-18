// <copyright file="ItemViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.ValidationSessionOverview
{
	using LacoWikiMobile.App.ViewModels.Shared;

	public class ItemViewModel : ItemViewModelBase
	{
		// TODO: Pass from CSS to Element to ViewModel when custom CSS properties and runtime class changes are supported
		// See https://github.com/xamarin/Xamarin.Forms/issues/2891 and https://github.com/xamarin/Xamarin.Forms/issues/2678
		public double Opacity => Pinned ? 1 : 0.3;

		public int Id { get; set; }

		public string Name { get; set; }

		public bool Pinned { get; set; }
	}
}