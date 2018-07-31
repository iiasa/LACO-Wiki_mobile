// <copyright file="Extensions.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.VisualElementExtensions
{
	using Xamarin.Forms;

	// See https://www.jimbobbennett.io/animating-xamarin-forms-progress-bars/
	public class Extensions
	{
		public static readonly BindableProperty AnimatedIsVisibleProperty = BindableProperty.CreateAttached("AnimatedIsVisible",
			typeof(bool), typeof(VisualElement), true, BindingMode.OneWay,
			propertyChanged: (b, o, n) => { DoAnimation((VisualElement)b, (bool)n); });

		public static readonly BindableProperty AnimatedIsVisibleAnimationEasingProperty =
			BindableProperty.Create("AnimatedIsVisibleAnimationEasing", typeof(string), typeof(VisualElement), default(string));

		public static readonly BindableProperty AnimatedIsVisibleAnimationLengthProperty =
			BindableProperty.Create("AnimatedIsVisibleAnimationLength", typeof(uint), typeof(VisualElement), 250U);

		public static bool GetAnimatedIsVisible(BindableObject view)
		{
			return (bool)view.GetValue(Extensions.AnimatedIsVisibleProperty);
		}

		public static string GetAnimatedIsVisibleAnimationEasing(BindableObject view)
		{
			return (string)view.GetValue(Extensions.AnimatedIsVisibleAnimationEasingProperty);
		}

		public static uint GetAnimatedIsVisibleAnimationLength(BindableObject view)
		{
			return (uint)view.GetValue(Extensions.AnimatedIsVisibleAnimationLengthProperty);
		}

		public static void SetAnimatedIsVisible(BindableObject view, bool value)
		{
			view.SetValue(Extensions.AnimatedIsVisibleProperty, value);
		}

		public static void SetAnimatedIsVisibleAnimationEasing(BindableObject view, string value)
		{
			view.SetValue(Extensions.AnimatedIsVisibleAnimationEasingProperty, value);
		}

		public static void SetAnimatedIsVisibleAnimationLength(BindableObject view, uint value)
		{
			view.SetValue(Extensions.AnimatedIsVisibleAnimationLengthProperty, value);
		}

		private static void DoAnimation(VisualElement view, bool isVisible)
		{
			ViewExtensions.CancelAnimations(view);

			Easing easing = !string.IsNullOrEmpty(GetAnimatedIsVisibleAnimationEasing(view))
				? GetAnimatedIsVisibleAnimationEasing(view).ToEasing()
				: null;

			view.FadeTo(isVisible ? 1 : 0, GetAnimatedIsVisibleAnimationLength(view), easing);
		}
	}
}