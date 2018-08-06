// <copyright file="IBictionary.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System.Collections.Generic;

	public interface IBictionary<T1, T2>
	{
		ICollection<T1> Keys { get; }

		ICollection<T2> Values { get; }

		T1 this[T2 key] { get; set; }

		T2 this[T1 key] { get; set; }

		bool Contains(T1 key);

		bool Contains(T2 key);

		void Remove(T2 key);

		void Remove(T1 key);
	}
}