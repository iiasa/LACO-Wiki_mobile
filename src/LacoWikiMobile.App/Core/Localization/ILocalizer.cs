// <copyright file="ILocalizer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Localization
{
	using System.Globalization;

	// See https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/localization
	public interface ILocalizer
	{
		CultureInfo GetCurrentCultureInfo();

		void SetLocale(CultureInfo cultureInfo);
	}
}