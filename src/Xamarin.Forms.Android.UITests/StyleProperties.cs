// <copyright file="StyleProperties.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using Xamarin.Forms;
using Xamarin.Forms.StyleSheets;

[assembly: StyleProperty("spacing", typeof(StackLayout), nameof(StackLayout.SpacingProperty))]
[assembly: StyleProperty("row-spacing", typeof(Grid), nameof(Grid.RowSpacingProperty))]
[assembly: StyleProperty("column-spacing", typeof(Grid), nameof(Grid.ColumnSpacingProperty))]
[assembly: StyleProperty("width-request", typeof(VisualElement), nameof(VisualElement.WidthRequestProperty))]
[assembly: StyleProperty("height-request", typeof(VisualElement), nameof(VisualElement.HeightRequestProperty))]
[assembly: StyleProperty("corner-radius", typeof(Frame), nameof(Frame.CornerRadiusProperty))]
[assembly: StyleProperty("has-shadow", typeof(Frame), nameof(Frame.HasShadowProperty))]
[assembly: StyleProperty("vertical-text-alignment", typeof(Label), nameof(Label.VerticalTextAlignmentProperty))]
[assembly: StyleProperty("horizontal-options", typeof(View), nameof(View.HorizontalOptions))]
[assembly: StyleProperty("vertical-options", typeof(View), nameof(View.VerticalOptionsProperty))]

namespace Xamarin.Forms.Android.UITests
{
	public class StyleProperties
	{
	}
}