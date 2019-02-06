// <copyright file="ValidationSession.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data.Entities
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Microsoft.EntityFrameworkCore.Infrastructure;

	public class ValidationSession
	{
		// TODO: Find out why this cannot be turned into a Property
		private ICollection<OfflineCache> offlineCaches = new List<OfflineCache>();

		private ICollection<LegendItem> legendItems = new List<LegendItem>();

		private ICollection<SampleItem> sampleItems = new List<SampleItem>();

		private User user;

		public ValidationSession()
		{
		}

		private ValidationSession(ILazyLoader lazyLoader)
		{
			LazyLoader = lazyLoader;
		}

		public string AssociatedDataSetName { get; set; }

		public string AssociatedSampleName { get; set; }

		public string Description { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public ICollection<LegendItem> LegendItems
		{
			get => LazyLoader.Load(this, ref this.legendItems);
			set => this.legendItems = value;
		}
		public ICollection<OfflineCache> OfflineCaches
		{
			get => LazyLoader.Load(this, ref this.offlineCaches);
			set => this.offlineCaches = value;
		}
		public string Name { get; set; }

		public int ProgressSamplesTotal { get; set; }

		public int ProgressSamplesValidated { get; set; }

		public ICollection<SampleItem> SampleItems
		{
			get => LazyLoader.Load(this, ref this.sampleItems);
			set => this.sampleItems = value;
		}

		[Required]
		public User User
		{
			get => LazyLoader.Load(this, ref this.user);
			set => this.user = value;
		}

		public int UserId { get; set; }

		public ValidationMethodEnum ValidationMethod { get; set; }

		private ILazyLoader LazyLoader { get; set; }
	}
}