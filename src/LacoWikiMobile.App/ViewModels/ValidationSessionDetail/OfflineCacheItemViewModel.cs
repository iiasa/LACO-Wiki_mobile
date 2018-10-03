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
			OnClickDownloadTilesCommand = new Command(DownloadTiles);
		}
		public ICommand OnClickDownloadTilesCommand { get; private set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public string Size { get; set; }

		void DownloadTiles()
		{
			System.Console.WriteLine("Download tiles");
			TaskDownloadTiles();
		}

		async Task TaskDownloadTiles()
		{
			System.Console.WriteLine("Download tiles");
			//async download
			string cacheId = "f825fc85-b4e2-41ac-851e-11d00d96f33b";
			/*
			if (Connectivity.NetworkAccess == NetworkAccess.Internet)
			{
				await ApiClient.GetCacheAsync(cacheId)
					.ContinueWith(result =>
					{
						byte[] cacheBytes = result.Result;
						FileManager.saveFileToDirectory(cacheId, cacheBytes);
					});
			}
			*/
		}
	}
}
