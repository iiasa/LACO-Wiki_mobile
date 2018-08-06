// <copyright file="CustomMap.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface.CustomMap
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using Xamarin.Forms;
	using Xamarin.Forms.Maps;

	public class CustomMap : Map
	{
		public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points), typeof(ICollection<IPoint>),
			typeof(CustomMap), new ObservableCollection<IPoint>());

		public ICollection<IPoint> Points
		{
			get => (ICollection<IPoint>)GetValue(CustomMap.PointsProperty);
			set => SetValue(CustomMap.PointsProperty, value);
		}
	}
}