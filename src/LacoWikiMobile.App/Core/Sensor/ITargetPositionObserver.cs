// <copyright file="ITargetPositionObserver.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Sensor
{
	public interface ITargetPositionObserver
	{
		void OnDirectionChanged(double direction);

		void OnDistanceChanged(double distance);
	}
}