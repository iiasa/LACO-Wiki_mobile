// <copyright file="LocalValidation.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data.Entities
{
	using System.ComponentModel.DataAnnotations;
	using Microsoft.EntityFrameworkCore.Infrastructure;

	public class LocalValidation
	{
		private LegendItem legendItem;

		private SampleItem sampleItem;

		public LocalValidation()
		{
		}

		private LocalValidation(ILazyLoader lazyLoader)
		{
			LazyLoader = lazyLoader;
		}

		public bool? Correct { get; set; }

		public LegendItem LegendItem
		{
			get => LazyLoader.Load(this, ref this.legendItem);
			set => this.legendItem = value;
		}

		[Required]
		public SampleItem SampleItem
		{
			get => LazyLoader.Load(this, ref this.sampleItem);
			set => this.sampleItem = value;
		}

		public bool Uploaded { get; set; }

		private ILazyLoader LazyLoader { get; set; }
	}
}