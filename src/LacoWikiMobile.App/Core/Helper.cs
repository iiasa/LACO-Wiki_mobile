// <copyright file="Helper.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System;
	using Xamarin.Forms;

	public class Helper
	{
		public static void RunOnMainThreadIfRequired(Action action)
		{
			if (!Device.IsInvokeRequired)
			{
				action();
			}
			else
			{
				Device.BeginInvokeOnMainThread(action);
			}
		}

		public static void EnsureOnMainThread()
		{
			if (Device.IsInvokeRequired)
			{
				throw new InvalidOperationException("Not on main thread.");
			}
		}
	}
}