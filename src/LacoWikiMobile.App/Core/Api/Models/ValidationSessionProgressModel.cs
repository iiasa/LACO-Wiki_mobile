// <copyright file="ValidationSessionProgressModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api.Models
{
	// Source: LacoWiki.Portal.Mvc.Models.Mobile
	public class ValidationSessionProgressModel
	{
		public int SamplesUnvalidated => SamplesTotal - SamplesValidated;

		public int SamplesTotal { get; set; }

		public int SamplesValidated { get; set; }

		public int SamplesValidatedByUser { get; set; }
	}
}