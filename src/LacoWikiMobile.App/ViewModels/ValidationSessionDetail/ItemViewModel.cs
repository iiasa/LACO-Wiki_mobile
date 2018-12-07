// <copyright file="ItemViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.ValidationSessionDetail
{
	using LacoWikiMobile.App.ViewModels.Map;
	using Xamarin.Forms;

	public class ItemViewModel : ItemViewModelBase
	{
		// TODO: LocalizationService
		public string ValueText => string.Format("Code: {0}", Value);

		public Color Color { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }
	}
}