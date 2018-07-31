// <copyright file="CheckboxRenderer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using LacoWikiMobile.App.iOS.UserInterface;
using LacoWikiMobile.App.UserInterface;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Checkbox), typeof(CheckboxRenderer))]

namespace LacoWikiMobile.App.iOS.UserInterface
{
	using System;
	using System.ComponentModel;
	using LacoWikiMobile.App.UserInterface;
	using SaturdayMP.XPlugins.iOS;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	// Based on https://alexdunn.org/2018/04/10/xamarin-tip-build-your-own-checkbox-in-xamarin-forms/
	public class CheckboxRenderer : ViewRenderer<Checkbox, BEMCheckBox>
	{
		protected const int DefaultSize = 28;

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SizeRequest sizeConstraint = base.GetDesiredSize(widthConstraint, heightConstraint);

			if (sizeConstraint.Request.Width == 0)
			{
				double width = widthConstraint;

				if (widthConstraint <= 0)
				{
					width = CheckboxRenderer.DefaultSize;
				}

				sizeConstraint = new SizeRequest(new Size(width, sizeConstraint.Request.Height),
					new Size(width, sizeConstraint.Minimum.Height));
			}

			return sizeConstraint;
		}

		protected void OnCheckedChanged(object sender, EventArgs e)
		{
			Element.IsChecked = Control.On;
			Element.CheckedCommand?.Execute(Element.CheckedCommandParameter);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					BEMCheckBox checkBox = new BEMCheckBox();

					checkBox.BoxType = BEMBoxType.Square;
					checkBox.OnAnimationType = BEMAnimationType.Fill;
					checkBox.OffAnimationType = BEMAnimationType.Fill;

					UpdateColors(checkBox);

					checkBox.ValueChanged += OnCheckedChanged;
					SetNativeControl(checkBox);
				}

				Control.On = e.NewElement.IsChecked;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(Element.IsChecked))
			{
				Control.On = Element.IsChecked;
			}
			else
			{
				UpdateColors(Control);
			}
		}

		protected void UpdateColors(BEMCheckBox nativeCheckBox)
		{
			nativeCheckBox.TintColor = Element.OutlineColor.ToUIColor();
			nativeCheckBox.OffFillColor = Element.InnerColor.ToUIColor();
			nativeCheckBox.OnFillColor = Element.CheckedInnerColor.ToUIColor();
			nativeCheckBox.OnTintColor = Element.CheckedOutlineColor.ToUIColor();
			nativeCheckBox.OnCheckColor = Element.CheckColor.ToUIColor();
		}
	}
}