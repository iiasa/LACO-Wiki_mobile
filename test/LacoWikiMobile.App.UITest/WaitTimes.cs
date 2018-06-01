// <copyright file="WaitTimes.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UITest
{
	using System;
	using Xamarin.UITest.Utils;

	public class WaitTimes : IWaitTimes
	{
		public TimeSpan GestureCompletionTimeout => TimeSpan.FromMinutes(1);

		public TimeSpan GestureWaitTimeout => TimeSpan.FromMinutes(1);

		public TimeSpan WaitForTimeout => TimeSpan.FromMinutes(1);
	}
}