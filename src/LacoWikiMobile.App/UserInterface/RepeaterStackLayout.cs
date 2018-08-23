// <copyright file="RepeaterStackLayout.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Linq;
	using LacoWikiMobile.App.Core;
	using Xamarin.Forms;

	public class RepeaterStackLayout : StackLayout
	{
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList),
			typeof(RepeaterStackLayout), default(IList), BindingMode.TwoWay, null, ItemsSourceChanged);

		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate),
			typeof(RepeaterStackLayout), default(DataTemplate));

		public IList ItemsSource
		{
			get => (IList)GetValue(RepeaterStackLayout.ItemsSourceProperty);
			set => SetValue(RepeaterStackLayout.ItemsSourceProperty, value);
		}

		public DataTemplate ItemTemplate
		{
			get => (DataTemplate)GetValue(RepeaterStackLayout.ItemTemplateProperty);
			set => SetValue(RepeaterStackLayout.ItemTemplateProperty, value);
		}

		protected virtual View GetItemView(object item)
		{
			object content = ItemTemplate.CreateContent();

			View view = content as View;

			if (view == null)
			{
				return null;
			}

			view.BindingContext = item;

			return view;
		}

		protected virtual void SetItems()
		{
			Helper.RunOnMainThreadIfRequired(() =>
			{
				Children.Clear();

				if (ItemsSource == null)
				{
					return;
				}

				// Clone to avoid exceptions when ItemSource gets modified while iterating
				foreach (object item in ItemsSource.Cast<object>().ToList())
				{
					Children.Add(GetItemView(item));
				}
			});
		}

		private static void ItemsSourceChanged(BindableObject bindable, object value, object newValue)
		{
			if (Device.IsInvokeRequired)
			{
				throw new NotSupportedException($"Modification of {nameof(ItemsSource)} only allowed in GUI thread.");
			}

			RepeaterStackLayout repeaterStackLayout = (RepeaterStackLayout)bindable;

			// Detach
			INotifyCollectionChanged oldItemsSourceNotifyCollectionChanged = value as INotifyCollectionChanged;

			if (oldItemsSourceNotifyCollectionChanged != null)
			{
				oldItemsSourceNotifyCollectionChanged.CollectionChanged -= repeaterStackLayout.ItemsSourceOnCollectionChanged;
			}

			// Attach
			INotifyCollectionChanged newItemsSourceNotifyCollectionChanged = newValue as INotifyCollectionChanged;

			if (newItemsSourceNotifyCollectionChanged != null)
			{
				newItemsSourceNotifyCollectionChanged.CollectionChanged += repeaterStackLayout.ItemsSourceOnCollectionChanged;
			}

			repeaterStackLayout.SetItems();
		}

		private void ItemsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
				{
					// If Action is NotifyCollectionChangedAction.Add, then NewItems contains the items that were added.
					// In addition, if NewStartingIndex is not -1, then it contains the index where the new items were added.
					if (e.NewStartingIndex == -1)
					{
						throw new NotSupportedException();
					}

					Helper.RunOnMainThreadIfRequired(() =>
					{
						int index = e.NewStartingIndex;

						foreach (object item in e.NewItems)
						{
							Children.Insert(index++, GetItemView(item));
						}
					});

					break;
				}

				case NotifyCollectionChangedAction.Move:
				{
					// If Action is NotifyCollectionChangedAction.Move, then NewItems and OldItems are logically equivalent
					// (i.e., they are SequenceEqual, even if they are different instances), and they contain the items that
					// moved. In addition, OldStartingIndex contains the index where the items were moved from, and NewStartingIndex
					// contains the index where the items were moved to. A Move operation is logically treated as a Remove followed
					// by an Add, so NewStartingIndex is interpreted as though the items had already been removed.
					Helper.RunOnMainThreadIfRequired(() =>
					{
						int index = e.NewStartingIndex;

						foreach (object item in e.NewItems)
						{
							Children.RemoveAt(e.OldStartingIndex);
							Children.Insert(index++, GetItemView(item));
						}
					});

					break;
				}

				case NotifyCollectionChangedAction.Remove:
				{
					// If Action is NotifyCollectionChangedAction.Remove, then OldItems contains the items that were removed.
					// In addition, if OldStartingIndex is not -1, then it contains the index where the old items were removed.
					if (e.OldStartingIndex == -1)
					{
						throw new NotSupportedException();
					}

					Helper.RunOnMainThreadIfRequired(() =>
					{
						foreach (object item in e.OldItems)
						{
							Children.RemoveAt(e.OldStartingIndex);
						}
					});

					break;
				}

				case NotifyCollectionChangedAction.Replace:
				{
					// If Action is NotifyCollectionChangedAction.Replace, then OldItems contains the replaced items and NewItems contains
					// the replacement items. In addition, NewStartingIndex and OldStartingIndex are equal, and if they are not -1, then
					// they contain the index where the items were replaced.
					if (e.NewStartingIndex == -1 || e.OldStartingIndex == -1)
					{
						throw new NotSupportedException();
					}

					Helper.RunOnMainThreadIfRequired(() =>
					{
						foreach (object item in e.OldItems)
						{
							Children.RemoveAt(e.OldStartingIndex);
						}

						int index = e.NewStartingIndex;

						foreach (object item in e.NewItems)
						{
							Children.Insert(index++, GetItemView(item));
						}
					});

					break;
				}

				case NotifyCollectionChangedAction.Reset:
				{
					// If Action is NotifyCollectionChangedAction.Reset, then no other properties are valid.
					Helper.RunOnMainThreadIfRequired(() =>
					{
						Children.Clear();

						foreach (object item in ItemsSource)
						{
							Children.Add(GetItemView(item));
						}
					});

					break;
				}
			}
		}
	}
}