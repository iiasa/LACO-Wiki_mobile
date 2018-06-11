// <copyright file="AppContext.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using System.Linq;
	using System.Text.RegularExpressions;
	using LacoWikiMobile.App.Core.Data.Entities;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata;

	public class AppDataContext : DbContext, IAppDataContext
	{
		public AppDataContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<ValidationSession> ValidationSessions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Formatting the table and attribute names in sqlite (snake_case)
			foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
			{
				Regex underscoreRegex = new Regex(@"(?<=.)([A-Z])");

				entity.Relational().TableName = underscoreRegex.Replace(entity.Relational().TableName, @"_$0").ToLower();

				entity.GetProperties()
					.ToList()
					.ForEach(x => x.Relational().ColumnName = underscoreRegex.Replace(x.Relational().ColumnName, @"_$0").ToLower());

				if (entity.FindPrimaryKey() != null)
				{
					entity.FindPrimaryKey().Relational().Name = entity.FindPrimaryKey().Relational().Name.ToLower();
				}

				entity.GetForeignKeys().ToList().ForEach(x => x.Relational().Name = x.Relational().Name.ToLower());
				entity.GetIndexes().ToList().ForEach(x => x.Relational().Name = x.Relational().Name.ToLower());
			}

			modelBuilder.Entity<ValidationSession>().Property(x => x.Id).ValueGeneratedNever();
		}
	}
}