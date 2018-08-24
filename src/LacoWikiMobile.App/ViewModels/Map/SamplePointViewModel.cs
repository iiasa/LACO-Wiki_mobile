// <copyright file="SamplePointViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels.Map
{
	using System.ComponentModel;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using Xamarin.Forms;

	public class SamplePointViewModel : ISamplePoint, IStyleable, ISelectable, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// TODO: Pass from CSS to Element to ViewModel when custom CSS properties are supported
		// See https://github.com/xamarin/Xamarin.Forms/issues/2891
		public Color FillColor
		{
			get
			{
				if (IsValidated)
				{
					return Color.FromHex("#B2DFDB");
				}

				return Selected == false ? Color.FromHex("#009688") : Color.FromHex("#673AB7");
			}
		}

		public double Radius
		{
			get
			{
				if (IsValidated)
				{
					return 100;
				}

				return Selected == false ? 150 : 200;
			}
		}

		// TODO: Pass from CSS to Element to ViewModel when custom CSS properties are supported
		// See https://github.com/xamarin/Xamarin.Forms/issues/2891
		public Color StrokeColor
		{
			get
			{
				if (IsValidated)
				{
					return Color.FromHex("#4DB6AC");
				}

				return Selected == false ? Color.FromHex("#004D40") : Color.FromHex("#311B92");
			}
		}

		public double StrokeWidth => Selected == false ? 15 : 20;

		public int Id { get; set; }

		public bool IsValidated { get; set; }

		public double Latitude { get; set; }

		public int LegendItemId { get; set; }

		public double Longitude { get; set; }

		public bool IsSelectable => !IsValidated;

		public bool Selected { get; set; }

		public int ValidationSessionId { get; set; }
	}
}