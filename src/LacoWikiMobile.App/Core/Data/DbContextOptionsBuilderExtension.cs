﻿// <copyright file="DbContextOptionsBuilderExtension.cs" company="IIASA">
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
		public static void Configure(this DbContextOptionsBuilder<AppDataContext> dbContextOptionsBuilder)
		{
			string path = Path.Combine(GetPlatformFolder(), "localdata.db");
			dbContextOptionsBuilder.UseSqlite($"Filename={path}");
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