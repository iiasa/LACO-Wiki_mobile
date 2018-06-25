// <copyright file="NotificationService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid.Core
{
	using Android;
	using Android.App;
	using Android.Support.Design.Widget;
	using Android.Views;
	using LacoWikiMobile.App.Core;
	using Plugin.CurrentActivity;

	public class NotificationService : INotificationService
	{
		// See https://medium.com/@frankiefoo/how-to-display-androids-toast-snackbar-in-xamarin-forms-pcl-project-7ec31b1639b7
		public void Notify(string message)
		{
			Activity activity = CrossCurrentActivity.Current.Activity;
			View activityRootView = activity.FindViewById(Resource.Id.Content);

			Snackbar.Make(activityRootView, message, Snackbar.LengthLong).Show();
		}
	}
}