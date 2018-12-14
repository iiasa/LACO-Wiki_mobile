// <copyright file="LegendItem.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data.Entities
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Microsoft.EntityFrameworkCore.Infrastructure;

	public class LegendItem
	{
		private ICollection<SampleItem> sampleItems = new List<SampleItem>();

		private ValidationSession validationSession;

		LocalOpportunisticValidation localOpportunisticValidation;

		public LegendItem()
		{
		}

		private LegendItem(ILazyLoader lazyLoader)
		{
			LazyLoader = lazyLoader;
		}

		public int Blue { get; set; }

		public int Green { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Name { get; set; }

		public int Red { get; set; }

		public ICollection<SampleItem> SampleItems
		{
			get => LazyLoader.Load(this, ref this.sampleItems);
			set => this.sampleItems = value;
		}

		[Required]
		public ValidationSession ValidationSession
		{
			get => LazyLoader.Load(this, ref this.validationSession);
			set => this.validationSession = value;
		}

		public LocalOpportunisticValidation LocalOpportunisticValidation
		{
			get => LazyLoader.Load(this, ref this.localOpportunisticValidation);
			set => this.localOpportunisticValidation = value;
		}

		public string Value { get; set; }

		private ILazyLoader LazyLoader { get; set; }
	}
}