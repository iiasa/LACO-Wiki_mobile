// <copyright file="TargetPositionObserverOptions.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Sensor
{
	public class TargetPositionObserverOptions
	{
		public TargetPositionObserverOptions(IPosition targetPosition)
		{
			TargetPosition = targetPosition;
		}

		public IPosition TargetPosition { get; protected set; }
	}
}