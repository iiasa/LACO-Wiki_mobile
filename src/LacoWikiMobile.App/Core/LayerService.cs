// <copyright file="LayerService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System.Collections.Generic;
	using LacoWikiMobile.App.ViewModels.Map;

	public static class LayerService
	{
		/// <summary>
		/// Constante for the points layer.
		/// </summary>
		public const int LAYERPOINTS = 1;

		public static IUpdatable MapRenderer { get; set; } = null;

		/// <summary>
		/// Gets or sets list of layer loaded.
		/// </summary>
		public static ICollection<LayerItemViewModel> LayerItems { get; set; } = new List<LayerItemViewModel>();

		/// <summary>
		/// Get layer by id (return the layer with the specified id, or null if not found).
		/// </summary>
		/// <param name="id">Id of the layer to search.</param>
		/// <returns>layer or null.</returns>
		public static LayerItemViewModel GetLayerById(int id)
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

		/// <summary>
		/// Get layer by name (return the first layer with the specified name, or null if not found).
		/// </summary>
		/// <param name="name">Name of the layer to search.</param>
		/// <returns>layer or null.</returns>
		public static LayerItemViewModel GetLayerByName(string name)
		{
			foreach (var item in LayerItems)
			{
				if (item.Name == name)
				{
					return item;
				}
			}

			return null;
		}

		/// <summary>
		/// Add a layer if not exists.
		/// </summary>
		/// <param name="name">Name of layer to add.</param>
		/// <param name="isEnabled">Set if layer can be activate or not.</param>
		/// <returns>void.</returns>
		public static LayerItemViewModel AddLayerPoints(string name, bool isEnabled)
		{
			LayerItemViewModel currentItem = new LayerItemViewModel
			{
				Id = LAYERPOINTS,
				IsChecked = isEnabled,
				Name = name,
				Icon = "ic_pin_white_24dp",
				IsEnabled = isEnabled,
			};
			LayerItems.Add(currentItem);

			return currentItem;
		}

		public static LayerItemViewModel AddLayerRaster(string name, bool isEnabled, bool isChecked)
		{
			LayerItemViewModel currentItem = new LayerItemViewModel
			{
				Id = GetMaxId() + 1,
				IsChecked = isChecked,
				Name = name,
				Icon = "ic_layers_white_24dp",
				IsEnabled = isEnabled,
			};
			LayerItems.Add(currentItem);
			return currentItem;
		}

		/// <summary>
		/// Reset the list of layers.
		/// </summary>
		public static void Reset()
		{
			LayerItems.Clear();
		}

		/// <summary>
		/// Update the visibility of a layer.
		/// </summary>
		/// <param name="id">Id of layer.</param>
		/// <param name="isChecked">Visibility of layer.</param>
		public static void UpdateIsChecked(int id, bool isChecked)
		{
			LayerItemViewModel layer = GetLayerById(id);
			if (layer != null)
			{
				layer.IsChecked = isChecked;
				if (layer.Id != LAYERPOINTS)
				{
					foreach (LayerItemViewModel lay in LayerItems)
					{
						if ( (lay.Id != layer.Id) && ( lay.Id != LAYERPOINTS))
						{
							lay.IsChecked = false;
						}
					}
				}
			}
			

			if (MapRenderer != null)
			{
				MapRenderer.Update();
			}
		}

		/// <summary>
		/// Return the current raster layer Id selected.
		/// </summary>
		/// <returns>Id of the current raster layer selected.</returns>
		public static int GetCurrentRasterId()
		{
			foreach (LayerItemViewModel lay in LayerItems)
			{
				if (lay.Id != LAYERPOINTS)
				{
					if (lay.IsChecked)
					{
						return lay.Id;
					}
				}
			}

			return -1;
		}

		/// <summary>
		/// Get the visiblity of a layer by its Id.
		/// </summary>
		/// <param name="id">Id of layer.</param>
		/// <returns>Visibility.</returns>
		public static bool GetIsCheckedById(int id)
		{
			LayerItemViewModel layer = GetLayerById(id);
			if (layer != null)
			{
				return layer.IsChecked;
			}

			return false;
		}

		private static int GetMaxId()
		{
			int max = -1;
			foreach (LayerItemViewModel layer in LayerService.LayerItems)
			{
				if (layer.Id > max)
				{
					max = layer.Id;
				}
			}

			return max;
		}
	}
}