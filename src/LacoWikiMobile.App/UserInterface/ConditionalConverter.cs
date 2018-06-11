// <copyright file="ConditionalConverter.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface
{
	using System;
	using System.Globalization;
	using Xamarin.Forms;

	public class ConditionalConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ConditionalConverterParameter conditionalConverterParameter = (ConditionalConverterParameter)parameter;

			if ((bool)value)
			{
				return conditionalConverterParameter.TypeConverter == null
					? conditionalConverterParameter.IfTrue
					: conditionalConverterParameter.TypeConverter.ConvertFromInvariantString(conditionalConverterParameter.IfTrue);
			}

			return conditionalConverterParameter.TypeConverter == null
				? conditionalConverterParameter.IfFalse
				: conditionalConverterParameter.TypeConverter.ConvertFromInvariantString(conditionalConverterParameter.IfFalse);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}