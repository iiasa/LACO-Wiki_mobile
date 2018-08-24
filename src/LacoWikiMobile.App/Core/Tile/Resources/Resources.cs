// <copyright file="Resources.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Tile.Resources
{
	using System.IO;
	using System.Reflection;

	public static class Resources
	{
		public static Stream GetIIASATiles() => Assembly.GetCallingAssembly().GetManifestResourceStream($"{GetPath()}.iiasa.mbtiles");

		public static string GetPath() => typeof(Resources).Namespace;
	}
}