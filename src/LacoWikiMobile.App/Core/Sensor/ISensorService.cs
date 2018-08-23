// <copyright file="ISensorService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Sensor
{
	using System.Threading.Tasks;

	public interface ISensorService
	{
		Task<bool> SubscribeToTargetPositionEventsAsync(ITargetPositionObserver observer, TargetPositionObserverOptions options);

		Task<bool> UnsubscribeToTargetPositionEventsAsync(ITargetPositionObserver observer);
	}
}