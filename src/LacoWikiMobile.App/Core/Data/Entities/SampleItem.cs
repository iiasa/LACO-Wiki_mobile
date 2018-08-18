// <copyright file="SampleItem.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data.Entities
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Microsoft.EntityFrameworkCore.Infrastructure;

	public class SampleItem
	{
		private LegendItem legendItem;

		private ValidationSession validationSession;

		public SampleItem()
		{
		}

		private SampleItem(ILazyLoader lazyLoader)
		{
			LazyLoader = lazyLoader;
		}

		public string Geometry { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public bool IsValidated { get; set; }

		[Required]
		public LegendItem LegendItem
		{
			get => LazyLoader.Load(this, ref this.legendItem);
			set => this.legendItem = value;
		}

		[Required]
		public ValidationSession ValidationSession
		{
			get => LazyLoader.Load(this, ref this.validationSession);
			set => this.validationSession = value;
		}

		private ILazyLoader LazyLoader { get; set; }
	}
}