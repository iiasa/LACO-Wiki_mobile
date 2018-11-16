// <copyright file="LayerService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System;
	using System.Collections.Generic;
	using LacoWikiMobile.App.ViewModels.Map;
	using Prism.Common;
	using Xamarin.Forms;

	// Prevent calling INavigationService methods outside of UI thread to prevent bugs
	public static class LayerService
	{
		public static ICollection<LayerItemViewModel> LayerItems { get; set; } = new List<LayerItemViewModel>();

		public static LayerItemViewModel getLayerById(int id)
		{
			foreach (var item in LayerItems)
			{
				if (item.Id == id)
				{
					return item;
				}
			}
			return null;
		}

		public static LayerItemViewModel AddLayer(int id, string name, string icon)
		{
			LayerItemViewModel currentItem = getLayerById(id);
			if (currentItem == null)
			{
				currentItem = new LayerItemViewModel
				{
					Id = id,
					IsChecked = true,
					Name = name,
					Icon = icon,
				};
				LayerItems.Add(currentItem);
			}
			return currentItem;
		}

		public static void Reset()
		{
			LayerItems.Clear();
		}
	}
}