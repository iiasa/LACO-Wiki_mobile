// <copyright file="OfflineCache.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LacoWikiMobile.App.Core.Data.Entities
{
	public class OfflineCache
	{
		private ValidationSession validationSession;
		private ILazyLoader LazyLoader;

		public OfflineCache()
		{

		}
		private OfflineCache(ILazyLoader lazyLoader)
		{

			LazyLoader = lazyLoader;
		}

		[Required]
		public ValidationSession ValidationSession
		{
			get => LazyLoader.Load(this, ref this.validationSession);
			set => this.validationSession = value;
		}
		
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public long FileSize { get; set; }

		public string LayerName { get; set; }

		public string Url { get; set; }
	}
}