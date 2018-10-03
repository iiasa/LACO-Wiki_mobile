using System;
using System.IO;

namespace LacoWikiMobile.App.Core.Data
{
	public class FileManager
	{

		public FileManager()
		{
		}

		public static void saveFileToDirectory(string cacheId, byte[] bytesArray) {
			string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), cacheId);
			System.Console.WriteLine("Filename "+fileName);
			try {
				File.WriteAllBytes(fileName, bytesArray);
			}
			catch(Exception e) {
				Console.WriteLine(e.ToString());
			}

		}

	}
}
