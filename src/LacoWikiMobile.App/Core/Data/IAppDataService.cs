// <copyright file="IAppDataService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.Core.Data.Entities;

	public interface IAppDataService
	{
		Task AddValidationSessionAsync(ValidationSession validationSession);

		Task EnsureUserExistsAsync();

		Task<IEnumerable<ValidationSession>> GetValidationSessionsAsync();

		Task TryRemoveValidationSessionAsync(int id);

		Task<ValidationSession> TryGetValidationSessionByIdAsync(int id);
	}
}