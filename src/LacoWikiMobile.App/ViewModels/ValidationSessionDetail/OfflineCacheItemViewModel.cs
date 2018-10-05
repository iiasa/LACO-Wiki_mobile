using System;
using System.Threading.Tasks;
using System.Windows.Input;
using LacoWikiMobile.App.Core.Api;
using LacoWikiMobile.App.Core.Data;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LacoWikiMobile.App.ViewModels.ValidationSessionDetail
{
	public class OfflineCacheItemViewModel
	{
		public OfflineCacheItemViewModel()
		{

		}

		public string CacheButtonText { get; set; }

		public string Id { get; set; }

		public string Name { get; set; }

		public string Size { get; set; }

		public string ParameterCacheId { get; set; }

		public bool isDownloaded { get; set; }

	}
}
