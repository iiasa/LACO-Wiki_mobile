// <copyright file="AppDataService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.Core.Api;
	using LacoWikiMobile.App.Core.Data.Entities;
	using Microsoft.EntityFrameworkCore;

	public class AppDataService : IAppDataService
	{
		public AppDataService(IApiAuthentication apiAuthentication, IAppDataContext context)
		{
			ApiAuthentication = apiAuthentication;
			Context = context;
		}

		protected IApiAuthentication ApiAuthentication { get; set; }

		protected IAppDataContext Context { get; set; }

		public async Task AddValidationSessionAsync(ValidationSession validationSession)
		{
			await ApiAuthentication.EnsureAuthenticatedAsync();

			int userId = await ApiAuthentication.GetUserIdAsync();
			User user = await Context.Users.SingleAsync(x => x.Id == userId);

			validationSession.User = user;

			await Context.ValidationSessions.AddAsync(validationSession);
			await Context.SaveChangesAsync();
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
				await Context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<ValidationSession>> GetValidationSessionsAsync()
		{
			await ApiAuthentication.EnsureAuthenticatedAsync();

			int userId = await ApiAuthentication.GetUserIdAsync();
			User user = await Context.Users.SingleAsync(x => x.Id == userId);

			return await Context.ValidationSessions.Where(x => x.User == user).ToListAsync();
		}

		public async Task RemoveValidationSessionAsync(int id)
		{
			await ApiAuthentication.EnsureAuthenticatedAsync();

			int userId = await ApiAuthentication.GetUserIdAsync();

			ValidationSession validationSession =
				await Context.ValidationSessions.SingleOrDefaultAsync(x => x.User.Id == userId && x.Id == id);

			if (validationSession != null)
			{
				Context.ValidationSessions.Remove(validationSession);
				await Context.SaveChangesAsync();
			}
		}
	}
}