// <copyright file="ValidationSessionDetailViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.ValidationSessionDetail
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using LacoWikiMobile.App.Core.Api.Models;

	public class ValidationSessionDetailViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public double Progress => (double)ProgressSamplesValidated / ProgressSamplesTotal;

		// TODO: LocalizationService
		public string ProgressText => $"{ProgressSamplesValidated} / {ProgressSamplesTotal} validated";

		public string AssociatedDataSetName { get; set; }

		public string AssociatedSampleName { get; set; }

		public string Description { get; set; }

		public int Id { get; set; }

		public IEnumerable<ItemViewModel> LegendItems { get; set; }

		public IEnumerable<OfflineCacheItemViewModel> OfflineCaches { get; set; }

		public string Name { get; set; }

		public int ProgressSamplesTotal { get; set; }

		public int ProgressSamplesValidated { get; set; }

		public ValidationMethodEnum ValidationMethod { get; set; }
	}
}