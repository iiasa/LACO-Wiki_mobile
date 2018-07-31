// <copyright file="ObservableCollectionExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq;

	public static class ObservableCollectionExtension
	{
		public static ObservableCollection<T> OnChildrenPropertyChanged<T>(this ObservableCollection<T> collection,
			PropertyChangedEventHandler callback)
		{
			foreach (INotifyPropertyChanged item in collection.OfType<INotifyPropertyChanged>())
			{
				item.PropertyChanged += callback;
			}

			collection.CollectionChanged += (sender, args) =>
			{
				switch (args.Action)
				{
					case NotifyCollectionChangedAction.Add:

						// If Action is NotifyCollectionChangedAction.Add, then NewItems contains the items that were added.
						// In addition, if NewStartingIndex is not -1, then it contains the index where the new items were added.
						foreach (INotifyPropertyChanged item in args.NewItems.OfType<INotifyPropertyChanged>())
						{
							item.PropertyChanged += callback;
						}

						break;

					case NotifyCollectionChangedAction.Remove:

						// If Action is NotifyCollectionChangedAction.Remove, then OldItems contains the items that were removed.
						// In addition, if OldStartingIndex is not -1, then it contains the index where the old items were removed.
						foreach (INotifyPropertyChanged item in args.OldItems.OfType<INotifyPropertyChanged>())
						{
							item.PropertyChanged -= callback;
						}

						break;

					case NotifyCollectionChangedAction.Replace:

						// If Action is NotifyCollectionChangedAction.Replace, then OldItems contains the replaced items and NewItems contains
						// the replacement items. In addition, NewStartingIndex and OldStartingIndex are equal, and if they are not -1, then
						// they contain the index where the items were replaced.
						foreach (INotifyPropertyChanged item in args.OldItems.OfType<INotifyPropertyChanged>())
						{
							item.PropertyChanged -= callback;
						}

						foreach (INotifyPropertyChanged item in args.NewItems.OfType<INotifyPropertyChanged>())
						{
							item.PropertyChanged += callback;
						}

						break;

					case NotifyCollectionChangedAction.Reset:

						// If Action is NotifyCollectionChangedAction.Reset, then no other properties are valid.
						// Cannot remove event handlers
						throw new InvalidOperationException();
				}
			};

			return collection;
		}

		public static T OnPropertyChanged<T>(this T element, PropertyChangedEventHandler callback) where T : INotifyPropertyChanged
		{
			element.PropertyChanged += callback;

			return element;
		}
	}
}