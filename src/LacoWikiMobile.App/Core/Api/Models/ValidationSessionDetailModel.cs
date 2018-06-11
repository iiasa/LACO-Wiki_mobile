// <copyright file="ValidationSessionDetailModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api.Models
{
	using System.Collections.Generic;

	// Source: LacoWiki.Portal.Mvc.Models.Mobile
	public class ValidationSessionDetailModel : ValidationSessionModel
	{
		public string AssociatedDataSetName { get; set; }

		public string AssociatedSampleName { get; set; }

		public string Description { get; set; }

		public IEnumerable<LegendItemModel> LegendItems { get; set; }

		public ValidationSessionProgressModel Progress { get; set; }

		public ValidationSessionSettings Settings { get; set; }

		public ValidationMethodEnum ValidationMethodEnum { get; set; }

		public string ValidationMethodName { get; set; }
	}
}