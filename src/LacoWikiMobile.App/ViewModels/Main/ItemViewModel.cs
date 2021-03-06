﻿// <copyright file="ItemViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.Main
{
	using LacoWikiMobile.App.ViewModels.Shared;

	public class ItemViewModel : ItemViewModelBase
	{
		public int Id { get; set; }

		public bool IsChecked { get; set; }

		public string Name { get; set; }
	}
}