// <copyright file="AppInitializerSettingsAndroid.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UITest.Settings
{
	public class AppInitializerSettingsAndroid
	{
		public string ApkFile { get; set; }

		public bool Debug { get; set; }

		public string DeviceSerial { get; set; }

		public bool EnableLocalScreenshots { get; set; }

		public WaitTimes WaitTimes { get; set; } = new WaitTimes();
	}
}