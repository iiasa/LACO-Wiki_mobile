// <copyright file="CheckBox.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface
{
	using System;
	using System.Windows.Input;
	using Xamarin.Forms;

	public class Checkbox : View
	{
		public static readonly BindableProperty CheckColorProperty =
			BindableProperty.Create(nameof(CheckColor), typeof(Color), typeof(Checkbox), Color.Black);

		public static readonly BindableProperty CheckedCommandParameterProperty =
			BindableProperty.Create(nameof(CheckedCommandParameter), typeof(object), typeof(Checkbox));

		public static readonly BindableProperty CheckedCommandProperty =
			BindableProperty.Create(nameof(CheckedCommand), typeof(ICommand), typeof(Checkbox), default(ICommand));

		public static readonly BindableProperty CheckedInnerColorProperty =
			BindableProperty.Create(nameof(CheckedInnerColor), typeof(Color), typeof(Checkbox), Color.White);

		public static readonly BindableProperty CheckedOutlineColorProperty =
			BindableProperty.Create(nameof(CheckedOutlineColor), typeof(Color), typeof(Checkbox), Color.Black);

		public static readonly BindableProperty InnerColorProperty =
			BindableProperty.Create(nameof(InnerColor), typeof(Color), typeof(Checkbox), Color.White);

		public static readonly BindableProperty IsCheckedProperty =
			BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(Checkbox), false, BindingMode.TwoWay);

		public static readonly BindableProperty OutlineColorProperty =
			BindableProperty.Create(nameof(OutlineColor), typeof(Color), typeof(Checkbox), Color.Black);

		public event EventHandler OnCheckChanged;

		public Color CheckColor
		{
			get => (Color)GetValue(Checkbox.CheckColorProperty);
			set => SetValue(Checkbox.CheckColorProperty, value);
		}

		public ICommand CheckedCommand
		{
			get => (ICommand)GetValue(Checkbox.CheckedCommandProperty);
			set => SetValue(Checkbox.CheckedCommandProperty, value);
		}

		public object CheckedCommandParameter
		{
			get => GetValue(Checkbox.CheckedCommandParameterProperty);
			set => SetValue(Checkbox.CheckedCommandParameterProperty, value);
		}

		public Color CheckedInnerColor
		{
			get => (Color)GetValue(Checkbox.CheckedInnerColorProperty);
			set => SetValue(Checkbox.CheckedInnerColorProperty, value);
		}

		public Color CheckedOutlineColor
		{
			get => (Color)GetValue(Checkbox.CheckedOutlineColorProperty);
			set => SetValue(Checkbox.CheckedOutlineColorProperty, value);
		}

		public Color InnerColor
		{
			get => (Color)GetValue(Checkbox.InnerColorProperty);
			set => SetValue(Checkbox.InnerColorProperty, value);
		}

		public bool IsChecked
		{
			get => (bool)GetValue(Checkbox.IsCheckedProperty);
			set => SetValue(Checkbox.IsCheckedProperty, value);
		}

		public Color OutlineColor
		{
			get => (Color)GetValue(Checkbox.OutlineColorProperty);
			set => SetValue(Checkbox.OutlineColorProperty, value);
		}

		public void FireCheckChange()
		{
			OnCheckChanged?.Invoke(this, new CheckChangedArgs
			{
				IsChecked = IsChecked,
			});
		}

		public class CheckChangedArgs : EventArgs
		{
			public bool IsChecked { get; set; }
		}
	}
}