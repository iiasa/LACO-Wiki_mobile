// <copyright file="StringExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.VisualElementExtensions
{
	using System.Collections.Generic;
	using Xamarin.Forms;

	public static class StringExtension
	{
		private static readonly IDictionary<string, Easing> EasingMapping = new Dictionary<string, Easing>()
		{
			{ nameof(Easing.Linear), Easing.Linear },
			{ nameof(Easing.SinOut), Easing.SinOut },
			{ nameof(Easing.SinIn), Easing.SinIn },
			{ nameof(Easing.SinInOut), Easing.SinInOut },
			{ nameof(Easing.CubicIn), Easing.CubicIn },
			{ nameof(Easing.CubicOut), Easing.CubicOut },
			{ nameof(Easing.CubicInOut), Easing.CubicInOut },
			{ nameof(Easing.BounceOut), Easing.BounceOut },
			{ nameof(Easing.BounceIn), Easing.BounceIn },
			{ nameof(Easing.SpringIn), Easing.SpringIn },
			{ nameof(Easing.SpringOut), Easing.SpringOut },
		};

		public static Easing ToEasing(this string easing)
		{
			if (string.IsNullOrEmpty(easing))
			{
				return null;
			}

			if (StringExtension.EasingMapping.ContainsKey(easing))
			{
				return StringExtension.EasingMapping[easing];
			}

			return null;
		}
	}
}