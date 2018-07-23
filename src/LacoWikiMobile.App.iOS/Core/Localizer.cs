// <copyright file="Localizer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS.Core
{
	using System.Globalization;
	using System.Threading;
	using Foundation;
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
			string netLanguage;

			if (NSLocale.PreferredLanguages.Length > 0)
			{
				string preferredLanguage = NSLocale.PreferredLanguages[0];
				netLanguage = iOSToDotnetLanguage(preferredLanguage);
			}
			else
			{
				return DefaultCultureInfo;
			}

			// This gets called a lot - try/catch can be expensive so consider caching or something
			CultureInfo cultureInfo = null;

			try
			{
				cultureInfo = new CultureInfo(netLanguage);
			}
			catch (CultureNotFoundException)
			{
				// iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
				// Fallback to first characters, in this case "en"
				try
				{
					string fallback = ToDotNetFallbackLanguage(new PlatformCulture(netLanguage));
					cultureInfo = new CultureInfo(fallback);
				}
				catch (CultureNotFoundException)
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

#pragma warning disable SA1300 // Element should begin with upper-case letter
		protected string iOSToDotnetLanguage(string iOSLanguage)
#pragma warning restore SA1300 // Element should begin with upper-case letter
		{
			string netLanguage = iOSLanguage;

			// Certain languages need to be converted to CultureInfo equivalent
			switch (iOSLanguage)
			{
				case "ms-MY": // "Malaysian (Malaysia)" not supported .NET culture
				case "ms-SG": // "Malaysian (Singapore)" not supported .NET culture
					netLanguage = "ms"; // Closest supported
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
				case "pt":
					netLanguage = "pt-PT"; // Fallback to Portuguese (Portugal)
					break;

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