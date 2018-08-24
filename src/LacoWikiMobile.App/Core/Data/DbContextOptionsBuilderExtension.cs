// <copyright file="DbContextOptionsBuilderExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data
{
	using System;
	using System.IO;
	using Microsoft.EntityFrameworkCore;
	using Xamarin.Forms;

	public static class DbContextOptionsBuilderExtension
	{
		public static void Configure<TContext>(this DbContextOptionsBuilder<TContext> dbContextOptionsBuilder,
			string filename = "localdata.db") where TContext : DbContext
		{
			string path = Path.Combine(GetPlatformFolder(), filename);
			dbContextOptionsBuilder.UseSqlite($"Filename={path}");
		}

		public static void CopyFromStream<TContext>(this DbContextOptionsBuilder<TContext> dbContextOptionsBuilder, Stream stream,
			string filename = "localdata.db") where TContext : DbContext
		{
			using (FileStream fileStream = File.Create(Path.Combine(GetPlatformFolder(), filename)))
			{
				stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(fileStream);
			}
		}

		public static bool FileExists<TContext>(this DbContextOptionsBuilder<TContext> dbContextOptionsBuilder,
			string filename = "localdata.db") where TContext : DbContext
		{
			return File.Exists(Path.Combine(GetPlatformFolder(), filename));
		}

		private static string GetPlatformFolder()
		{
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library");

				case Device.Android:
					return Environment.GetFolderPath(Environment.SpecialFolder.Personal);

				default:
					throw new NotSupportedException();
			}
		}
	}
}