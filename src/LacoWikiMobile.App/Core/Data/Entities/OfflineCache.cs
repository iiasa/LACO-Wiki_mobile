// <copyright file="OfflineCache.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data.Entities
{
	public class OfflineCache
	{
		public long FileSize { get; set; }

		public string LayerName { get; set; }

		public string Url { get; set; }
	}
}