// <copyright file="AppDataService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.Core.Data.Entities;
	using Microsoft.EntityFrameworkCore;

	public class AppDataService : IAppDataService
	{
		public AppDataService(IAppDataContext context)
		{
			Context = context;
		}

		protected IAppDataContext Context { get; set; }

		public async Task AddValidationSessionAsync(ValidationSession validationSession)
		{
			await Context.ValidationSessions.AddAsync(validationSession);
			await Context.SaveChangesAsync();
		}

		public async Task<IEnumerable<ValidationSession>> GetValidationSessionsAsync()
		{
			return await Context.ValidationSessions.ToListAsync();
		}
	}
}