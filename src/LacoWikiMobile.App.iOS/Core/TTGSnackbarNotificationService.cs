// <copyright file="TTGSnackbarNotificationService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.iOS.Core
{
	using LacoWikiMobile.App.Core;
	using TTGSnackBar;

	public class TTGSnackbarNotificationService : INotificationService
	{
		public void Notify(string message)
		{
			Helper.RunOnMainThreadIfRequired(() =>
			{
				TTGSnackbar snackbar = new TTGSnackbar(message)
				{
					// Note: This is the only working animation type in XF
					AnimationType = TTGSnackbarAnimationType.FadeInFadeOut,
				};

				snackbar.Show();
			});
		}
	}
}