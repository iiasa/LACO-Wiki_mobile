// <copyright file="CustomTileProvider.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS.UserInterface
{
	using System;
	using Foundation;
	using LacoWikiMobile.App.Core.Tile;
	using LacoWikiMobile.App.Core.Tile.Entities;
	using MapKit;

	public class CustomTileProvider : MKTileOverlay
	{
		public CustomTileProvider(IReadOnlyTileService tileService)
		{
			TileService = tileService;
		}

		public IReadOnlyTileService TileService { get; set; }

		public override void LoadTileAtPath(MKTileOverlayPath path, MKTileOverlayLoadTileCompletionHandler result)
		{
			if (TileService != null)
			{
				Tile tile = TileService.TryGetTile((int)path.X, (int)((int)Math.Pow(2, path.Z) - 1 - path.Y), (int)path.Z);
				if (tile != null)
				{
					NSData tileData = NSData.FromArray(tile.TileData);
					result.Invoke(tileData, null);
				}
			}
		}
	}
}