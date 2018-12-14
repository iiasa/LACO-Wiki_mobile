using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LacoWikiMobile.App.Core.Data.Entities
{
	public class LocalOpportunisticValidation
	{

		private LegendItem legendItem;

		private SampleItem sampleItem;

		private ValidationSession validationSession;

		public LocalOpportunisticValidation()
		{
		}

		private LocalOpportunisticValidation(ILazyLoader lazyLoader)
		{
			LazyLoader = lazyLoader;
		}
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ValidationId { get; set; }

		public string Geometry
		{
			get; set;
		}
		public bool? Correct { get; set; }

		public LegendItem LegendItem
		{
			get => LazyLoader.Load(this, ref this.legendItem);
			set => this.legendItem = value;
		}
		public bool Uploaded { get; set; }

		private ILazyLoader LazyLoader { get; set; }
	}
}
