// <copyright file="MapPageViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.ViewModels
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using LacoWikiMobile.App.Core;
	using LacoWikiMobile.App.UserInterface.CustomMap;
	using LacoWikiMobile.App.ViewModels.Map;
	using Microsoft.Extensions.Localization;
	using Prism.Navigation;

	public class MapPageViewModel : ViewModelBase
	{
		public MapPageViewModel(INavigationService navigationService, IPermissionService permissionService,
			IStringLocalizer<MapPageViewModel> stringLocalizer)
			: base(navigationService, permissionService, stringLocalizer)
		{
			Points.Add(new PointViewModel()
			{
				Longitude = 12.4964,
				Latitude = 41.9028,
			});
		}

		public ICollection<IPoint> Points { get; set; } = new ObservableCollection<IPoint>();
	}
}