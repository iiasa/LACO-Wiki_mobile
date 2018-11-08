using System;
using System.IO;

namespace LacoWikiMobile.App.Core.Data
{
	public class FileManager
	{

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

		public static bool CacheFileExists(string layerName)
		{
			string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), layerName);
			return File.Exists(fileName);
		}

		public static void DeleteCache(string layerName) 
		{
			string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), layerName);
			File.Delete(fileName);
		}
	}
}
