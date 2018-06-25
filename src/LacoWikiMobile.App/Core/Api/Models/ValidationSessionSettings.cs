// <copyright file="ValidationSessionSettings.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api.Models
{
	// Source: LacoWiki.Portal.Mvc.Models.Mobile
	public class ValidationSessionSettings
	{
		public bool CardinalDirectionPhotosOptional { get; set; }

		// Maximum distance in meters to allow the validation, null => no limitation
		public int? Distance { get; set; }

		public bool PointPhotoOptional { get; set; }

		public bool TakeCardinalDirectionPhotos { get; set; }

		public bool TakePointPhoto { get; set; }
	}
}