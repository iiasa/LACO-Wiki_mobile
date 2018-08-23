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
		Task AddLocalValidationAsync(LocalValidation localValidation);

		Task AddValidationSessionAsync(ValidationSession validationSession);

		void DisableDetectChanges();

		void EnableDetectChanges();

		Task EnsureUserExistsAsync();

		Task<LegendItem> GetLegendItemByIdAsync(int id, int validationSessionId);

		Task<LocalValidation> GetLocalValidationByIdAsync(int id, int validationSessionId);

		Task<IEnumerable<LocalValidation>> GetLocalValidationsByIdAsync(int validationSessionId);

		Task<IEnumerable<LocalValidation>> GetLocalValidationsWhereNotUploadedByIdAsync(int validationSessionId);

		Task<SampleItem> GetSampleItemByIdAsync(int id, int validationSessionId);

		Task<ValidationSession> GetValidationSessionByIdAsync(int id);

		Task<IEnumerable<ValidationSession>> GetValidationSessionsAsync();

		Task SaveChangesAsync();

		Task<ValidationSession> TryGetValidationSessionByIdAsync(int id);

		Task TryRemoveValidationSessionAsync(int id);
	}
}