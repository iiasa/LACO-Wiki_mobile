// <copyright file="ReadOnlyTileService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Tile
{
	using System.Linq;
	using LacoWikiMobile.App.Core.Tile.Entities;

	public class ReadOnlyTileService : IReadOnlyTileService
	{
		public ReadOnlyTileService(TileContext tileContext)
		{
			TileContext = tileContext;
		}

		protected TileContext TileContext { get; set; }

		public Tile TryGetTile(int tileColumn, int tileRow, int zoomLevel)
		{
			lock (TileContext)
			{
				return TileContext.Tiles.SingleOrDefault(tile =>
					tile.ZoomLevel == zoomLevel && tile.TileColumn == tileColumn && tile.TileRow == tileRow);
			}
		}
	}
}