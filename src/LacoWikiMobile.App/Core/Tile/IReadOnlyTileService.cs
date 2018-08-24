// <copyright file="IReadOnlyTileService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Tile
{
	using LacoWikiMobile.App.Core.Tile.Entities;

	public interface IReadOnlyTileService
	{
		Tile TryGetTile(int tileColumn, int tileRow, int zoomLevel);
	}
}