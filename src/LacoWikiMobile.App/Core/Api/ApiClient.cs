// <copyright file="ApiClient.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Threading.Tasks;
	using Flurl.Http;
	using LacoWikiMobile.App.Core.Api.Models;

	public class ApiClient : IApiClient
	{
		public ApiClient(IApiAuthentication apiAuthentication)
		{
			ApiAuthentication = apiAuthentication;
		}

		protected string BaseUrl => "https://dev.laco-wiki.net/api/mobile";// //https://laco-wiki.net/api/mobile
		protected string BaseCacheUrl => "https://tilecache.dev.geo-wiki.org/v1/";

		protected IApiAuthentication ApiAuthentication { get; set; }

		public async Task<ValidationSessionDetailModel> GetValidationSessionByIdAsync(int id)
		{
			try
			{
				string res = await BaseUrl.WithHeader("Authorization", "Bearer " + await ApiAuthentication.GetAccessTokenAsync())
					.AppendPathSegment($"validationsessions/{id}")
					.GetStringAsync();

				return await BaseUrl.WithHeader("Authorization", "Bearer " + await ApiAuthentication.GetAccessTokenAsync())
					.AppendPathSegment($"validationsessions/{id}")
					.GetJsonAsync<ValidationSessionDetailModel>();
			}
			catch (FlurlHttpException e)
			{
				throw TokenExpiredOrOriginalException(e);
			}
		}

		public async Task<IEnumerable<SampleItemModel>> GetValidationSessionSampleItemsByIdAsync(int id)
		{
			try
			{
				return await BaseUrl.WithHeader("Authorization", "Bearer " + await ApiAuthentication.GetAccessTokenAsync())
					.AppendPathSegment($"validationsessions/{id}/sampleitems")
					.GetJsonAsync<IEnumerable<SampleItemModel>>();
			}
			catch (FlurlHttpException e)
			{
				throw TokenExpiredOrOriginalException(e);
			}
		}

		public async Task<IEnumerable<ValidationSessionModel>> GetValidationSessionsAsync()
		{
			try
			{
				return await BaseUrl.WithHeader("Authorization", "Bearer " + await ApiAuthentication.GetAccessTokenAsync())
					.AppendPathSegment("validationsessions")
					.GetJsonAsync<IEnumerable<ValidationSessionModel>>();
			}
			catch (FlurlHttpException e)
			{
				throw TokenExpiredOrOriginalException(e);
			}
		}

		public async Task PostValidationAsync(int validationSessionId, int sampleItemId, ValidationCreateModel model)
		{
			try
			{
				await BaseUrl.WithHeader("Authorization", "Bearer " + await ApiAuthentication.GetAccessTokenAsync())
					.AppendPathSegment($"validationsessions/{validationSessionId}/sampleitems/{sampleItemId}/validations")
					.PostJsonAsync(model);
			}
			catch (FlurlHttpException e)
			{
				throw TokenExpiredOrOriginalException(e);
			}
		}

		public async Task<byte[]> GetCacheAsync(string url)
		{
			try
			{
				//return await BaseCacheUrl.WithHeader("Authorization", "Bearer " + await ApiAuthentication.GetAccessTokenAsync())
				//                  .AppendPathSegment($"tilecaches/{cacheId}/download")

				return await url.GetBytesAsync();
			}
			catch (FlurlHttpException e)
			{
				System.Console.WriteLine("DEBUG - Error " + e.Message);
				throw TokenExpiredOrOriginalException(e);
			}
		}

		protected Exception TokenExpiredOrOriginalException(FlurlHttpException e)
		{
			// Assume token expired
			if (e.Call.HttpStatus == HttpStatusCode.Unauthorized)
			{
				return new TokenExpiredException();
			}

			return e;
		}
	}
}