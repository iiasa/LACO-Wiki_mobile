// <copyright file="ValidationModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api.Models
{
	// Source: LacoWiki.Portal.Mvc.Models.Mobile
	public class ValidationModel
	{
		public bool? Correct { get; set; }

		public bool IsValidator { get; set; }

		public int? LegendItemID { get; set; }

		public int ValidationID { get; set; }

		public string Validator { get; set; }
	}
}