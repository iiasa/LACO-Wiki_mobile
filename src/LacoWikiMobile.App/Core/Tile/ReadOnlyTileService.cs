// <copyright file="ReadOnlyTileService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Tile
{
	using System.Linq;
	using LacoWikiMobile.App.Core.Tile.Entities;
	using Microsoft.Data.Sqlite;

	public class ReadOnlyTileService : IReadOnlyTileService
	{
		public ReadOnlyTileService(TileContext tileContext)
		{
			TileContext = tileContext;
		}

		protected TileContext TileContext { get; set; }

		public Tile TryGetTile(int tileColumn, int tileRow, int zoomLevel)
		{
			Tile returnedTile = null;
			lock (TileContext)
			{
				try
				{
					returnedTile = TileContext.Tiles.SingleOrDefault(tile =>
						tile.ZoomLevel == zoomLevel && tile.TileColumn == tileColumn && tile.TileRow == tileRow);
				}
				catch (SqliteException e)
				{
					returnedTile = null;
				}

				return returnedTile;
			}
		}
	}
}