﻿// <copyright file="IAppDataContext.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using System.Threading;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.Core.Data.Entities;
	using Microsoft.EntityFrameworkCore;

	public interface IAppDataContext
	{
		DbSet<User> Users { get; }

		DbSet<ValidationSession> ValidationSessions { get; }

		int SaveChanges();

		int SaveChanges(bool acceptAllChangesOnSuccess);

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

		Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
	}
}