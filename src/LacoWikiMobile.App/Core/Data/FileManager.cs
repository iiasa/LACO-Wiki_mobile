// <copyright file="FileManager.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using System;
	using System.IO;

	public class FileManager
	{
		private static string savingPath;

		public FileManager()
		{
		}

		public static string SavingPath
		{
			get
			{
				return savingPath;
			}

			set
			{
				savingPath = value;
			}
		}

		public static void SaveFileToDirectory(string layerName, byte[] bytesArray)
		{
			string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), layerName);
			System.Console.WriteLine("Filename " + fileName);
			try
			{
				File.WriteAllBytes(fileName, bytesArray);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		// Check if the cache for a specified url is saved on cache or not
		public static bool CacheFileExists(string url)
		{
			string fileName = url.GetHashCode().ToString();
			string path;
			path = Path.Combine(SavingPath, fileName);
			return File.Exists(path);
		}
		public static String GetCacheFilePath(string url)
		{
			string fileName = url.GetHashCode().ToString();
			string path;
			path = Path.Combine(SavingPath, fileName);
			return path;
		}

		public static string GetFullPath(string fileName)
		{
			string path;
			path = Path.Combine(SavingPath, fileName);
			return path;
		}

		public static void DeleteOfflineCache(string url)
		{
			string fileName = url.GetHashCode().ToString();
			string path;
			path = Path.Combine(SavingPath, fileName);
			File.Delete(path);
		}
	}
}