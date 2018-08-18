// <copyright file="IApiClient.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.Core.Api.Models;

	public interface IApiClient
	{
		Task<ValidationSessionDetailModel> GetValidationSessionByIdAsync(int id);

		Task<IEnumerable<SampleItemModel>> GetValidationSessionSampleItemsByIdAsync(int id);

		Task<IEnumerable<ValidationSessionModel>> GetValidationSessionsAsync();
	}
}