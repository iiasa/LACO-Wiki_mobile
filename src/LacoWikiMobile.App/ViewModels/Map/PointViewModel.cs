// <copyright file="PointViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.Map
{
	using System.ComponentModel;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using Xamarin.Forms;

	public class PointViewModel : IPoint, IStyleable, ISelectable, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public Color FillColor => Selected == false ? Color.FromHex("#009688") : Color.FromHex("#673AB7");

		public double Radius => Selected == false ? 1000 : 1500;

		public Color StrokeColor => Selected == false ? Color.FromHex("#004D40") : Color.FromHex("#311B92");

		public double StrokeWidth => Selected == false ? 10 : 15;

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public bool Selected { get; set; }
	}
}