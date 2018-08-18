// <copyright file="Extensions.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.VisualElementExtensions
{
	using System;
	using LacoWikiMobile.App.Core.Sensor;
	using Xamarin.Forms;

	// See https://www.jimbobbennett.io/animating-xamarin-forms-progress-bars/
	public class Extensions
	{
		public static readonly BindableProperty AnimatedIsVisibleProperty = BindableProperty.CreateAttached("AnimatedIsVisible",
			typeof(bool), typeof(VisualElement), true, BindingMode.OneWay,
			propertyChanged: (b, o, n) => { DoIsVisibleAnimation((VisualElement)b, (bool)n); });

		public static readonly BindableProperty AnimatedIsVisibleAnimationEasingProperty =
			BindableProperty.Create("AnimatedIsVisibleAnimationEasing", typeof(string), typeof(VisualElement), default(string));

		public static readonly BindableProperty AnimatedIsVisibleAnimationLengthProperty =
			BindableProperty.Create("AnimatedIsVisibleAnimationLength", typeof(uint), typeof(VisualElement), 250U);

		public static readonly BindableProperty AnimatedRotationProperty = BindableProperty.CreateAttached("AnimatedRotation",
			typeof(double), typeof(VisualElement), default(double), BindingMode.OneWay,
			propertyChanged: (b, o, n) => { DoRotationAnimation((VisualElement)b, (double)o, (double)n); });

		public static readonly BindableProperty AnimatedRotationAnimationEasingProperty =
			BindableProperty.Create("AnimatedRotationAnimationEasing", typeof(string), typeof(VisualElement), default(string));

		public static readonly BindableProperty AnimatedRotationAnimationLengthProperty =
			BindableProperty.Create("AnimatedRotationAnimationLength", typeof(uint), typeof(VisualElement), 250U);

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

		public static double GetAnimatedRotation(BindableObject view)
		{
			return (double)view.GetValue(Extensions.AnimatedRotationProperty);
		}

		public static string GetAnimatedRotationAnimationEasing(BindableObject view)
		{
			return (string)view.GetValue(Extensions.AnimatedRotationAnimationEasingProperty);
		}

		public static uint GetAnimatedRotationAnimationLength(BindableObject view)
		{
			return (uint)view.GetValue(Extensions.AnimatedRotationAnimationLengthProperty);
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

		public static void SetAnimatedRotation(BindableObject view, double value)
		{
			view.SetValue(Extensions.AnimatedRotationProperty, value);
		}

		public static void SetAnimatedRotationAnimationEasing(BindableObject view, string value)
		{
			view.SetValue(Extensions.AnimatedRotationAnimationEasingProperty, value);
		}

		public static void SetAnimatedRotationAnimationLength(BindableObject view, uint value)
		{
			view.SetValue(Extensions.AnimatedRotationAnimationLengthProperty, value);
		}

		private static void DoIsVisibleAnimation(VisualElement view, bool isVisible)
		{
			ViewExtensions.CancelAnimations(view);

			Easing easing = !string.IsNullOrEmpty(GetAnimatedIsVisibleAnimationEasing(view))
				? GetAnimatedIsVisibleAnimationEasing(view).ToEasing()
				: null;

			view.FadeTo(isVisible ? 1 : 0, GetAnimatedIsVisibleAnimationLength(view), easing);
		}

		private static void DoRotationAnimation(VisualElement view, double oldRotation, double newRotation)
		{
			ViewExtensions.CancelAnimations(view);

			Easing easing = !string.IsNullOrEmpty(GetAnimatedRotationAnimationEasing(view))
				? GetAnimatedRotationAnimationEasing(view).ToEasing()
				: null;

			double differenceViewToNew = view.Rotation.Normalize().DifferenceTo(newRotation);

			uint length = (uint)Math.Min(GetAnimatedRotationAnimationLength(view),
				Math.Max(50, GetAnimatedRotationAnimationLength(view) * (180 / differenceViewToNew)));

			view.RotateTo(view.Rotation - differenceViewToNew, length, easing);
		}
	}
}