// <copyright file="PlatformCulture.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Localization
{
	using System;

	// See https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/localization
	public class PlatformCulture
	{
		public PlatformCulture(string platformCultureString)
		{
			if (string.IsNullOrEmpty(platformCultureString))
			{
				throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));
			}

			// .NET expects dash, not underscore
			PlatformString = platformCultureString.Replace("_", "-");

			int dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);

			if (dashIndex > 0)
			{
				string[] parts = PlatformString.Split('-');

				LanguageCode = parts[0];
				LocaleCode = parts[1];
			}
			else
			{
				LanguageCode = PlatformString;
				LocaleCode = string.Empty;
			}
		}

		public string LanguageCode { get; protected set; }

		public string LocaleCode { get; protected set; }

		public string PlatformString { get; protected set; }

		public override string ToString()
		{
			return PlatformString;
		}
	}
}