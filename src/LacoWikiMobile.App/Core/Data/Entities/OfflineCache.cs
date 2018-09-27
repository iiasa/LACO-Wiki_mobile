using System;
namespace LacoWikiMobile.App.Core.Data.Entities
{
	public class OfflineCache
	{
		public long FileSize { get; set; }
		public string LayerName { get; set; }
		public string Url { get; set; }
		// Additional properties TBD

		public OfflineCache()
		{
		}
	}
}
