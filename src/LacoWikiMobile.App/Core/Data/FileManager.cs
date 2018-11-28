
using System;
using System.IO;

namespace LacoWikiMobile.App.Core.Data
{
	public class FileManager
	{
		public static string SavingPath;

		public FileManager()
		{
		}

		public static void saveFileToDirectory(string layerName, byte[] bytesArray) {
			string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), layerName);
			System.Console.WriteLine("Filename "+fileName);
			try {
				File.WriteAllBytes(fileName, bytesArray);
			}
			catch(Exception e) {
				Console.WriteLine(e.ToString());
			}

		}

		/**
		 * Check if the cache for a specified url is saved on cache or not
		 */
		public static bool CacheFileExists(string url)
		{
			string fileName = url.GetHashCode().ToString();
			string path;
    		path = Path.Combine (SavingPath, fileName);
			return File.Exists(path);
		}

		public static string getFullPath(string fileName) 
		{
			string path;

    		path = Path.Combine (SavingPath, fileName);

			return path;
		}

		public static void DeleteOfflineCache(string url) 
		{
			string fileName = url.GetHashCode().ToString();
			string path;
    		path = Path.Combine (SavingPath, fileName);
			File.Delete(path);
		}
	}
}
