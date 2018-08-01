// <copyright file="TaskExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System;
	using System.Threading.Tasks;

	public static class TaskExtension
	{
		public static Task ContinueIfTrueWith(this Task<bool> task, Action nextTask)
		{
			return task.ContinueWith((result) =>
			{
				if (result.Result)
				{
					nextTask();
				}
			});
		}
	}
}