// <copyright file="EnumLookup.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Tile
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	// TODO: Move to separate folder (Model?, Type?)
	internal class EnumLookup<T>
	{
		public EnumLookup()
		{
		}

		public EnumLookup(T value)
		{
			Id = Convert.ToInt32(value);
			Value = value;
			Name = value.ToString();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Name { get; set; }

		public T Value { get; set; }
	}
}