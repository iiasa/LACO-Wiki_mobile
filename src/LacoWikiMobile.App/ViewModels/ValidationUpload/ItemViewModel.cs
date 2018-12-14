// <copyright file="ItemViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.ValidationUpload
{
	using LacoWikiMobile.App.ViewModels.Shared;
	using Xamarin.Forms;

	public class ItemViewModel : ItemViewModelBase
	{
		// TODO: Pass from CSS to Element to ViewModel when custom CSS properties and runtime class changes are supported
		// See https://github.com/xamarin/Xamarin.Forms/issues/2891 and https://github.com/xamarin/Xamarin.Forms/issues/2678
		public Color Color => Uploaded ? Color.FromHex("#009688") : Color.FromHex("#757575");

		public ImageSource ImageSource =>
			Uploaded ? ImageSource.FromFile("ic_check_all_white_24dp") : ImageSource.FromFile("ic_check_white_24dp");

		public int ItemId { get; set; }

		public bool Uploaded { get; set; }

		public bool IsOpportunisticValidation { get; set; }

		// TODO: Localization
		public string Name => IsOpportunisticValidation? $"Opportunistic #{ItemId}" : $"Sample #{ItemId}";

		public bool? Correct { get; set; }

		public int? LegendItemId { get; set; }
	}
}