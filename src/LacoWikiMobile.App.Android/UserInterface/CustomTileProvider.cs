// <copyright file="CustomTileProvider.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid.UserInterface
{
	using System;
	using Android.Gms.Maps.Model;
	using LacoWikiMobile.App.Core.Tile;

	public class CustomTileProvider : Java.Lang.Object, ITileProvider
	{
		public CustomTileProvider(IReadOnlyTileService tileService)
		{
			TileService = tileService;
		}

		public IReadOnlyTileService TileService { get; set; }

		protected static Lazy<Tile> NoTile { get; } = new Lazy<Tile>(() => new Tile(-1, -1, null));

		public Tile GetTile(int x, int y, int zoom)
		{
			LacoWikiMobile.App.Core.Tile.Entities.Tile tile = TileService.TryGetTile(x, (int)Math.Pow(2, zoom) - 1 - y, zoom);

			if (tile == null)
			{
				return NoTile.Value;
			}

			return new Tile(512, 512, tile.TileData);
		}
	}
}