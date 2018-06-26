﻿// <copyright file="ValidationSession.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Data.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class ValidationSession
	{
		public int Id { get; set; }

		public string Name { get; set; }

		[Required]
		public User User { get; set; }
	}
}