// <copyright file="CircleBictionary.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Droid.UserInterface
{
	using System.Collections.Generic;
	using Android.Gms.Maps.Model;
	using LacoWikiMobile.App.Core;

	public class CircleBictionary<T> : IBictionary<T, Circle>
	{
		public CircleBictionary()
		{
			GenericMapping = new Bictionary<T, string>();
			CircleMapping = new Bictionary<string, Circle>();
		}

		public ICollection<T> Keys => GenericMapping.Keys;

		public ICollection<Circle> Values => CircleMapping.Values;

		protected Bictionary<string, Circle> CircleMapping { get; set; }

		protected Bictionary<T, string> GenericMapping { get; set; }

		public T this[Circle key]
		{
			get => GenericMapping[key.Id];
			set
			{
				CircleMapping.Remove(key.Id);

				CircleMapping[key.Id] = key;
				GenericMapping[key.Id] = value;
			}
		}

		public Circle this[T key]
		{
			get => CircleMapping[GenericMapping[key]];
			set
			{
				if (GenericMapping.Contains(key))
				{
					CircleMapping.Remove(GenericMapping[key]);
				}

				CircleMapping[value.Id] = value;
				GenericMapping[value.Id] = key;
			}
		}

		public bool Contains(T key)
		{
			return GenericMapping.Contains(key);
		}

		public bool Contains(Circle key)
		{
			return GenericMapping.Contains(key.Id);
		}

		public void Remove(Circle key)
		{
			GenericMapping.Remove(key.Id);
			CircleMapping.Remove(key.Id);
		}

		public void Remove(T key)
		{
			if (GenericMapping.Contains(key))
			{
				CircleMapping.Remove(GenericMapping[key]);
			}

			GenericMapping.Remove(key);
		}
	}
}