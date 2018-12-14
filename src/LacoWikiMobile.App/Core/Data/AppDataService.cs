// <copyright file="AppDataService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data.Entities;
	using Microsoft.EntityFrameworkCore;

	// TODO: Add thread safety
	public class AppDataService : IAppDataService
	{
		public AppDataService(IApiAuthentication apiAuthentication, IAppDataContext context)
		{
			ApiAuthentication = apiAuthentication;
			Context = context;
		}

		protected IApiAuthentication ApiAuthentication { get; set; }

		protected IAppDataContext Context { get; set; }

		public async Task AddLocalOpportunisticValidation(LocalOpportunisticValidation localOpportunisticValidation)
		{
				await Context.LocalOpportunisticValidations.AddAsync(localOpportunisticValidation);
				await SaveChangesAsync();
		}

		public async Task AddLocalValidationAsync(LocalValidation localValidation)
		{
				await Context.LocalValidations.AddAsync(localValidation);
				await SaveChangesAsync();
		}

		public async Task AddValidationSessionAsync(ValidationSession validationSession)
		{
			await ApiAuthentication.EnsureAuthenticatedAsync();

			int userId = await ApiAuthentication.GetUserIdAsync();
			User user = await Context.Users.SingleAsync(x => x.Id == userId);

			validationSession.User = user;

			await Context.ValidationSessions.AddAsync(validationSession);
			await SaveChangesAsync();
		}

		public void DisableDetectChanges()
		{
			Context.DisableDetectChanges();
		}

		public void EnableDetectChanges()
		{
			Context.EnableDetectChanges();
		}

		public async Task EnsureUserExistsAsync()
		{
			await ApiAuthentication.EnsureAuthenticatedAsync();

			int userId = await ApiAuthentication.GetUserIdAsync();
			User user = await Context.Users.SingleOrDefaultAsync(x => x.Id == userId);

			if (user == null)
			{
				user = new User
				{
					Id = userId,
					Name = await ApiAuthentication.GetUserNameAsync(),
				};

				await Context.Users.AddAsync(user);
				await SaveChangesAsync();
			}
		}

		public async Task<LocalOpportunisticValidation> GetGetLocalOpportunisticValidationByIdAsync(int id, int validationSessionId)
		{
			ValidationSession validationSession = await GetValidationSessionByIdAsync(validationSessionId);
			return await Context.LocalOpportunisticValidations.SingleAsync(x =>
				x.LegendItem.Id == id && x.LegendItem.ValidationSession == validationSession);
		}

		public async Task<LegendItem> GetLegendItemByIdAsync(int id, int validationSessionId)
		{
			ValidationSession validationSession = await GetValidationSessionByIdAsync(validationSessionId);
			return await Context.LegendItems.SingleAsync(x => x.Id == id && x.ValidationSession == validationSession);
		}

		public async Task<IEnumerable<LocalOpportunisticValidation>> GetLocalOpportunisticValidationsByIdAsync(int validationSessionId)
		{
			ValidationSession validationSession = await GetValidationSessionByIdAsync(validationSessionId);
			return await Context.LocalOpportunisticValidations.Where(x => x.LegendItem.ValidationSession == validationSession).ToListAsync();
		}

		public async Task<IEnumerable<LocalOpportunisticValidation>> GetLocalOpportunisticValidationWhereNotUploadedByIdAsync(int validationSessionId)
		{
			ValidationSession validationSession = await GetValidationSessionByIdAsync(validationSessionId);
			return await Context.LocalOpportunisticValidations.Where(x => x.LegendItem.ValidationSession == validationSession && x.Uploaded == false)
				.ToListAsync();
		}

		public async Task<LocalValidation> GetLocalValidationByIdAsync(int id, int validationSessionId)
		{
			ValidationSession validationSession = await GetValidationSessionByIdAsync(validationSessionId);
			return await Context.LocalValidations.SingleAsync(x =>
				x.SampleItem.Id == id && x.SampleItem.ValidationSession == validationSession);
		}

		public async Task<IEnumerable<LocalValidation>> GetLocalValidationsByIdAsync(int validationSessionId)
		{
			ValidationSession validationSession = await GetValidationSessionByIdAsync(validationSessionId);
			return await Context.LocalValidations.Where(x => x.SampleItem.ValidationSession == validationSession).ToListAsync();
		}

		public async Task<IEnumerable<LocalValidation>> GetLocalValidationsWhereNotUploadedByIdAsync(int validationSessionId)
		{
			ValidationSession validationSession = await GetValidationSessionByIdAsync(validationSessionId);
			return await Context.LocalValidations.Where(x => x.SampleItem.ValidationSession == validationSession && x.Uploaded == false)
				.ToListAsync();
		}

		public async Task<SampleItem> GetSampleItemByIdAsync(int id, int validationSessionId)
		{
			ValidationSession validationSession = await GetValidationSessionByIdAsync(validationSessionId);
			return await Context.SampleItems.SingleAsync(x => x.Id == id && x.ValidationSession == validationSession);
		}

		public async Task<ValidationSession> GetValidationSessionByIdAsync(int id)
		{
			await ApiAuthentication.EnsureAuthenticatedAsync();

			int userId = await ApiAuthentication.GetUserIdAsync();
			User user = await Context.Users.SingleAsync(x => x.Id == userId);

			return await Context.ValidationSessions.SingleAsync(x => x.User == user && x.Id == id);
		}

		public async Task<IEnumerable<ValidationSession>> GetValidationSessionsAsync()
		{
			await ApiAuthentication.EnsureAuthenticatedAsync();

			int userId = await ApiAuthentication.GetUserIdAsync();
			User user = await Context.Users.SingleAsync(x => x.Id == userId);

			return await Context.ValidationSessions.Where(x => x.User == user).ToListAsync();
		}

		public Task SaveChangesAsync()
		{
			return Context.SaveChangesAsync();
		}

		public async Task<ValidationSession> TryGetValidationSessionByIdAsync(int id)
		{
			await ApiAuthentication.EnsureAuthenticatedAsync();

			int userId = await ApiAuthentication.GetUserIdAsync();
			User user = await Context.Users.SingleAsync(x => x.Id == userId);

			return await Context.ValidationSessions.SingleOrDefaultAsync(x => x.User == user && x.Id == id);
		}

		public async Task TryRemoveValidationSessionAsync(int id)
		{
			ValidationSession validationSession = await TryGetValidationSessionByIdAsync(id);

			if (validationSession != null)
			{
				Context.ValidationSessions.Remove(validationSession);
				await SaveChangesAsync();
			}
		}

	}
}