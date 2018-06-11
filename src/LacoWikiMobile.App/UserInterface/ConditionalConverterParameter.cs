// <copyright file="ConditionalConverterParameter.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface
{
	using Xamarin.Forms;

	public class ConditionalConverterParameter
	{
		public string IfFalse { get; set; }

		public string IfTrue { get; set; }

		public TypeConverter TypeConverter { get; set; }
	}
}