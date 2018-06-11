// <copyright file="ItemViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.ValidationSessionOverview
{
	using LacoWikiMobile.App.ViewModels.Shared;

	public class ItemViewModel : ItemViewModelBase
	{
		// Move Opacity to Xaml (ConditionalConverter)
		public double Opacity => Pinned ? 1 : 0.3;

		public int Id { get; set; }

		public string Name { get; set; }

		public bool Pinned { get; set; }
	}
}