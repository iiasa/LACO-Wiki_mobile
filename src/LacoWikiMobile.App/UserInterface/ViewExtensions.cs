// <copyright file="ViewExtensions.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface
{
	using System;
	using System.Threading.Tasks;
	using Xamarin.Forms;

	public static class ViewExtensions
	{
		public static void CancelAnimation(this VisualElement self)
		{
			self.AbortAnimation("ProgressTo");
		}

		public static Task<bool> ProgressTo(this CircularProgressBar view, double progress, uint length = 250, Easing easing = null)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}

			if (easing == null)
			{
				easing = Easing.Linear;
			}

			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			WeakReference<CircularProgressBar> weakView = new WeakReference<CircularProgressBar>(view);

			Action<double> callback = f =>
			{
				CircularProgressBar v;

				if (weakView.TryGetTarget(out v))
				{
					v.Progress = f;
				}
			};

			new Animation(callback, view.Progress, progress, easing).Commit(view, "ProgressTo", 16, length,
				finished: (f, a) => tcs.SetResult(a));

			return tcs.Task;
		}
	}
}