// <copyright file="Bictionary.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System.Collections.Generic;

	public class Bictionary<T1, T2> : IBictionary<T1, T2>
	{
		public Bictionary()
		{
			Forward = new Dictionary<T1, T2>();
			Reverse = new Dictionary<T2, T1>();
		}

		public virtual ICollection<T1> Keys => Forward.Keys;

		public virtual ICollection<T2> Values => Forward.Values;

		protected IDictionary<T1, T2> Forward { get; set; }

		protected IDictionary<T2, T1> Reverse { get; set; }

		public virtual T1 this[T2 key]
		{
			get => Reverse[key];
			set
			{
				if (Reverse.ContainsKey(key))
				{
					Forward.Remove(Reverse[key]);
				}

				Reverse[key] = value;
				Forward[value] = key;
			}
		}

		public virtual T2 this[T1 key]
		{
			get => Forward[key];
			set
			{
				if (Forward.ContainsKey(key))
				{
					Reverse.Remove(Forward[key]);
				}

				Forward[key] = value;
				Reverse[value] = key;
			}
		}

		public virtual bool Contains(T1 key)
		{
			return Forward.ContainsKey(key);
		}

		public virtual bool Contains(T2 key)
		{
			return Reverse.ContainsKey(key);
		}

		public virtual void Remove(T1 key)
		{
			if (Forward.ContainsKey(key))
			{
				Reverse.Remove(Forward[key]);
			}

			Forward.Remove(key);
		}

		public virtual void Remove(T2 key)
		{
			if (Reverse.ContainsKey(key))
			{
				Forward.Remove(Reverse[key]);
			}

			Reverse.Remove(key);
		}
	}
}