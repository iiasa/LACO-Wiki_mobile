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
		public static Task ContinueIfTrueWith(this Task<bool> task, Action continuationAction)
		{
			return task.ContinueWith((result) =>
			{
				if (result.Result)
				{
					continuationAction();
				}

				return Task.CompletedTask;
			});
		}

		public static Task ContinueIfTrueWith(this Task<bool> task, Func<Task> continuationAction)
		{
			return task.ContinueWith((result) =>
			{
				if (result.Result)
				{
					return continuationAction();
				}

				return Task.CompletedTask;
			});
		}

		public static Task<bool> ContinueIfTrueWith(this Task<bool> task, Func<Task<bool>> continuationFunction)
		{
			return task.ContinueWith((result) =>
				{
					if (result.Result)
					{
						return continuationFunction();
					}

					return Task.FromResult(false);
				})
				.Unwrap();
		}

		public static Task<bool> ContinueIfTrueWith(this Task<bool> task, Func<bool> continuationFunction)
		{
			return task.ContinueWith((result) =>
			{
				if (result.Result)
				{
					return continuationFunction();
				}

				return false;
			});
		}
	}
}