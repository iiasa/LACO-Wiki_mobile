// <copyright file="TileContext.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Tile
{
	using LacoWikiMobile.App.Core.Tile.Entities;
	using LacoWikiMobile.App.Core.Tile.NamingConfiguration;
	using Microsoft.EntityFrameworkCore;

	public class TileContext : DbContext
	{
		public TileContext(DbContextOptions<TileContext> options)
			: base(options)
		{
		}

		public DbSet<Metadata> Metadata { get; set; }

		public DbSet<Tile> Tiles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Tile>()
				.HasKey(x => new
				{
					x.ZoomLevel,
					x.TileColumn,
					x.TileRow,
				});

			modelBuilder.ConfigureNames(NamingOptions.Default.SetNamingScheme(NamingScheme.SnakeCase).SetTableNamingSource(From.DbSet));
		}
	}
}