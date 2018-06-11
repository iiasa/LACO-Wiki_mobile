// <copyright file="ValidationSessionValidateResultModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api.Models
{
	// Source: LacoWiki.Portal.Mvc.Models.Mobile
	public class ValidationSessionValidateResultModel
	{
		public string ValidationResultString => ValidationResultEnum.ToString();

		public ValidationResultEnum ValidationResultEnum { get; set; }
	}
}