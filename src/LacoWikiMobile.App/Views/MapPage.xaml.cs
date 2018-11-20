// <copyright file="MapPage.xaml.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Views
{
	using System;
	using LacoWikiMobile.App.ViewModels.Map;
	using Xamarin.Forms;
	using Xamarin.Forms.Xaml;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
		public MapPage()
		{
			InitializeComponent();
		}
	}
}