// <copyright file="StaticApiClient.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.Core.Api.Models;

	public class StaticApiClient : IApiClient
	{
		public Task<ValidationSessionDetailModel> GetValidationSessionByIdAsync(int id)
		{
			return Task.Run(async () =>
			{
				await Task.Delay(1000);

				ValidationSessionDetailModel validationSessionDetailModel = new ValidationSessionDetailModel();

				switch (id)
				{
					case 1:
						validationSessionDetailModel.ValidationSessionID = 1;
						validationSessionDetailModel.ValidationSessionName = "Some Validation Session";
						break;

					case 2:
						validationSessionDetailModel.ValidationSessionID = 2;
						validationSessionDetailModel.ValidationSessionName = "Another Validation Session";
						break;

					case 3:
						validationSessionDetailModel.ValidationSessionID = 3;
						validationSessionDetailModel.ValidationSessionName = "Third Session for you";
						break;

					default:
						throw new InvalidOperationException();
				}

				validationSessionDetailModel.AssociatedDataSetName = "LISA Innsbruck";
				validationSessionDetailModel.AssociatedSampleName = "100x Stratified Point Sample";
				validationSessionDetailModel.Description =
					"This is the mobile validation session of the LISA Innsbruck stratified point sample.";

				validationSessionDetailModel.ValidationMethodEnum = ValidationMethodEnum.Blind;
				validationSessionDetailModel.ValidationMethodName = validationSessionDetailModel.ValidationMethodEnum.ToString();

				validationSessionDetailModel.Progress = new ValidationSessionProgressModel()
				{
					SamplesTotal = 110,
					SamplesValidated = 30,
					SamplesValidatedByUser = 20,
				};

				validationSessionDetailModel.LegendItems = new[]
				{
					new LegendItemModel()
					{
						LegendItemID = 11,
						ClassName = "Buildings",
						Value = "1",
						Red = 219,
						Green = 0,
						Blue = 0,
					},
					new LegendItemModel()
					{
						LegendItemID = 12,
						ClassName = "Other constructed area",
						Value = "2",
						Red = 254,
						Green = 248,
						Blue = 164,
					},
					new LegendItemModel()
					{
						LegendItemID = 13,
						ClassName = "Bare soil",
						Value = "3",
						Red = 215,
						Green = 194,
						Blue = 158,
					},
				};

				return validationSessionDetailModel;
			});
		}

		public Task<IEnumerable<ValidationSessionModel>> GetValidationSessionsAsync()
		{
			return Task.Run(async () =>
			{
				await Task.Delay(1000);

				return (IEnumerable<ValidationSessionModel>)new[]
				{
					new ValidationSessionModel()
					{
						ValidationSessionID = 1,
						ValidationSessionName = "Some Validation Session",
					},
					new ValidationSessionModel()
					{
						ValidationSessionID = 2,
						ValidationSessionName = "Another Validation Session",
					},
					new ValidationSessionModel()
					{
						ValidationSessionID = 3,
						ValidationSessionName = "Third Session for you",
					},
				};
			});
		}
	}
}