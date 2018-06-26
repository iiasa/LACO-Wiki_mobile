// <copyright file="Localizer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid.Core
{
	using System.Globalization;
	using System.Threading;
	using Java.Util;
	using LacoWikiMobile.App.Core.Localization;

	// See https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/localization
	public class Localizer : ILocalizer
	{
		public Localizer()
		{
			DefaultCultureInfo = new CultureInfo("en");
		}

		public CultureInfo DefaultCultureInfo { get; set; }

		public CultureInfo GetCurrentCultureInfo()
		{
			Locale androidLocale = Locale.Default;
			string netLanguage = AndroidToDotNetLanguage(androidLocale.ToString().Replace("_", "-"));

			// This gets called a lot - try/catch can be expensive so consider caching or something
			CultureInfo cultureInfo = null;

			try
			{
				cultureInfo = new CultureInfo(netLanguage);
			}
			catch (CultureNotFoundException exception1)
			{
				// iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
				// Fallback to first characters, in this case "en"
				try
				{
					string fallback = ToDotNetFallbackLanguage(new PlatformCulture(netLanguage));
					cultureInfo = new CultureInfo(fallback);
				}
				catch (CultureNotFoundException exception2)
				{
					// iOS language not valid .NET culture, falling back to English
					cultureInfo = DefaultCultureInfo;
				}
			}

			return cultureInfo;
		}

		public void SetLocale(CultureInfo cultureInfo)
		{
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
		}

		protected string AndroidToDotNetLanguage(string androidLanguage)
		{
			string netLanguage = androidLanguage;

			// Certain languages need to be converted to CultureInfo equivalent
			switch (androidLanguage)
			{
				case "ms-BN": // "Malaysian (Brunei)" not supported .NET culture
				case "ms-MY": // "Malaysian (Malaysia)" not supported .NET culture
				case "ms-SG": // "Malaysian (Singapore)" not supported .NET culture
					netLanguage = "ms"; // Closest supported
					break;

				case "in-ID": // "Indonesian (Indonesia)" has different code in  .NET
					netLanguage = "id-ID"; // Correct code for .NET
					break;

				case "gsw-CH": // "Schwiizertüütsch (Swiss German)" not supported .NET culture
					netLanguage = "de-CH"; // Closest supported
					break;

				// Add more application-specific cases here (if required)
				// ONLY use cultures that have been tested and known to work
			}

			return netLanguage;
		}

		protected string ToDotNetFallbackLanguage(PlatformCulture platformCulture)
		{
			// Use the first part of the identifier (two chars, usually);
			string netLanguage = platformCulture.LanguageCode;

			switch (platformCulture.LanguageCode)
			{
				case "gsw":
					netLanguage = "de-CH"; // Equivalent to German (Switzerland) for this app
					break;

				// Add more application-specific cases here (if required)
				// ONLY use cultures that have been tested and known to work
			}

			return netLanguage;
		}
	}
}