// <copyright file="CheckboxRenderer.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using LacoWikiMobile.App.Droid.UserInterface;
using LacoWikiMobile.App.UserInterface;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Checkbox), typeof(CheckboxRenderer))]

namespace LacoWikiMobile.App.Droid.UserInterface
{
	using System.ComponentModel;
	using Android.Content;
	using Android.Content.Res;
	using Android.Support.V7.Widget;
	using Android.Widget;
	using LacoWikiMobile.App.UserInterface;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	// Based on https://alexdunn.org/2018/04/10/xamarin-tip-build-your-own-checkbox-in-xamarin-forms/
	public class CheckboxRenderer : ViewRenderer<Checkbox, AppCompatCheckBox>, CompoundButton.IOnCheckedChangeListener
	{
		protected const int DefaultSize = 28;

		public CheckboxRenderer(Context context)
			: base(context)
		{
		}

		public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			SizeRequest sizeRequest = base.GetDesiredSize(widthConstraint, heightConstraint);

			// See https://stackoverflow.com/questions/2151241/android-how-to-change-checkbox-size
			// Checkbox implementation is buggy, should not request specific size
			if (sizeRequest.Request.Width == 0)
			{
				int width = widthConstraint;

				if (widthConstraint <= 0)
				{
					width = CheckboxRenderer.DefaultSize;
				}

				sizeRequest = new SizeRequest(new Size(width, sizeRequest.Request.Height), new Size(width, sizeRequest.Minimum.Height));
			}

			return sizeRequest;
		}

		public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
		{
			((IViewController)Element).SetValueFromRenderer(Checkbox.IsCheckedProperty, isChecked);
			Element.CheckedCommand?.Execute(Element.CheckedCommandParameter);
		}

		protected ColorStateList GetBackgroundColorStateList(Color color)
		{
			return new ColorStateList(new[]
			{
				new[] { -Android.Resource.Attribute.StateEnabled }, // disabled
				new[] { -Android.Resource.Attribute.StateChecked }, // unchecked
				new[] { Android.Resource.Attribute.StateChecked }, // checked
			}, new int[] { color.WithSaturation(0.1).ToAndroid(), color.ToAndroid(), color.ToAndroid() });
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					AppCompatCheckBox checkBox = new AppCompatCheckBox(Context);

					if (Element.OutlineColor != default(Color))
					{
						UpdateColors(checkBox);
					}

					checkBox.SetOnCheckedChangeListener(this);
					SetNativeControl(checkBox);
				}

				Control.Checked = e.NewElement.IsChecked;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(Element.IsChecked))
			{
				Control.Checked = Element.IsChecked;
			}
			else
			{
				UpdateColors(Control);
			}
		}

		protected void UpdateColors(AppCompatCheckBox checkBox)
		{
			ColorStateList backgroundColor = GetBackgroundColorStateList(Element.CheckColor);

			checkBox.SupportButtonTintList = backgroundColor;
			checkBox.BackgroundTintList = GetBackgroundColorStateList(Element.InnerColor);
			checkBox.ForegroundTintList = GetBackgroundColorStateList(Element.OutlineColor);
		}
	}
}