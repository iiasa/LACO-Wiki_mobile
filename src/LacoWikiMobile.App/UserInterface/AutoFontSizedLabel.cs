// <copyright file="AutoFontSizedLabel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Xamarin.Forms;

	public class AutoFontSizedLabel : Label
	{
		public ICollection<AutoFontSizedLabel> LinkedLabels { get; set; } = new List<AutoFontSizedLabel>();

		public double PossibleFontSize { get; set; }

		public void AutoFit()
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}

			double oldFontSize = FontSize;

			FontSize = 10;
			AutoFontSizeResult lowerFontSizeResult = new AutoFontSizeResult()
			{
				FontSize = 10,
				TextSize = Measure(Width, double.PositiveInfinity).Request.Height,
			};

			FontSize = 100;
			AutoFontSizeResult upperFontSizeResult = new AutoFontSizeResult()
			{
				FontSize = 100,
				TextSize = Measure(Width, double.PositiveInfinity).Request.Height,
			};

			if ((lowerFontSizeResult.TextSize == -1) || (upperFontSizeResult.TextSize == -1))
			{
				FontSize = oldFontSize;
				return;
			}

			while (upperFontSizeResult.FontSize - lowerFontSizeResult.FontSize > 1)
			{
				// Get the average font size of the upper and lower bounds.
				double fontSize = (upperFontSizeResult.FontSize + lowerFontSizeResult.FontSize) / 2;

				// Check the new text height against the container height.
				FontSize = fontSize;
				AutoFontSizeResult newFontSizeResult = new AutoFontSizeResult()
				{
					FontSize = fontSize,
					TextSize = Measure(Width, double.PositiveInfinity).Request.Height,
				};

				if (newFontSizeResult.TextSize > Height)
				{
					upperFontSizeResult = newFontSizeResult;
				}
				else
				{
					lowerFontSizeResult = newFontSizeResult;
				}
			}

			PossibleFontSize = lowerFontSizeResult.FontSize;

			if (!LinkedLabels.Any(x => x.PossibleFontSize != 0))
			{
				FontSize = PossibleFontSize;
			}
			else
			{
				double fontSize = Math.Min(PossibleFontSize, LinkedLabels.Where(x => x.PossibleFontSize != 0).Min(x => x.PossibleFontSize));

				FontSize = fontSize;

				foreach (AutoFontSizedLabel label in LinkedLabels)
				{
					label.FontSize = fontSize;
				}
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == "Text")
			{
				AutoFit();
			}
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			AutoFit();
		}
	}
}