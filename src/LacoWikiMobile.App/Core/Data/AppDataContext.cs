// <copyright file="AppDataContext.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using LacoWikiMobile.App.Core.Data.Entities;
	using Microsoft.EntityFrameworkCore;

	public class AppDataContext : DbContext, IAppDataContext
	{
		public AppDataContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<LegendItem> LegendItems { get; set; }

		public DbSet<SampleItem> SampleItems { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<ValidationSession> ValidationSessions { get; set; }

		public void DisableDetectChanges()
		{
			ChangeTracker.AutoDetectChangesEnabled = false;
		}

		public void EnableDetectChanges()
		{
			ChangeTracker.AutoDetectChangesEnabled = true;
			ChangeTracker.DetectChanges();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ValidationSession>().Property<int>($"{nameof(ValidationSession.User)}Id");

			modelBuilder.Entity<ValidationSession>().HasKey(nameof(ValidationSession.Id), $"{nameof(ValidationSession.User)}Id");

			modelBuilder.Entity<LegendItem>()
				.HasKey(nameof(LegendItem.Id), $"{nameof(LegendItem.ValidationSession)}Id",
					$"{nameof(LegendItem.ValidationSession)}{nameof(ValidationSession.User)}Id");

			modelBuilder.Entity<SampleItem>()
				.HasKey(nameof(LegendItem.Id), $"{nameof(LegendItem.ValidationSession)}Id",
					$"{nameof(LegendItem.ValidationSession)}{nameof(ValidationSession.User)}Id");
		}
	}
}