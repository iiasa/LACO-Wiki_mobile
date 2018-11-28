using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace LacoWikiMobile.App.ViewModels.ValidationSessionDetail
{
	public class OfflineCacheItemViewModel : INotifyPropertyChanged
	{
		public OfflineCacheItemViewModel()
		{

		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string CacheButtonText { get; set; }

		public string Name { get; set; }

		public string Size { get; set; }

		public string Url { get; set; }

		public string ImageButton { get; set; }

		public string Path { get; set; }

		public bool isDownloaded { get; set; }

	}
}
