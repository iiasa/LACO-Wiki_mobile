// <copyright file="EnumOptions.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Tile
{
	using System;
	using LacoWikiMobile.App.Core.Tile.NamingConfiguration;

	public class EnumOptions
	{
		public static EnumOptions Default
		{
			get
			{
				EnumOptions enumOptions = new EnumOptions();
				enumOptions.SetNamingScheme(NamingScheme.SnakeCase);

				return enumOptions;
			}
		}

		public Func<string, string> NamingFuction { get; set; }

		public EnumOptions SetNamingScheme(Func<string, string> namingFunc)
		{
			NamingFuction = namingFunc;

			return this;
		}
	}
}