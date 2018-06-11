// <copyright file="RepeaterStackLayout.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.UserInterface
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Linq;
	using System.Reflection;
	using Xamarin.Forms;

	public class RepeaterStackLayout : StackLayout
	{
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable),
			typeof(RepeaterStackLayout), default(IEnumerable<object>), BindingMode.TwoWay, null, ItemsSourceChanged);

		public static readonly BindableProperty ItemTemplateProperty =
			BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RepeaterStackLayout), default(DataTemplate));

		private ObservableCollection<object> observableSource;

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(RepeaterStackLayout.ItemsSourceProperty);
			set => SetValue(RepeaterStackLayout.ItemsSourceProperty, value);
		}

		public DataTemplate ItemTemplate
		{
			get => (DataTemplate)GetValue(RepeaterStackLayout.ItemTemplateProperty);
			set => SetValue(RepeaterStackLayout.ItemTemplateProperty, value);
		}

		protected ObservableCollection<object> ObservableSource
		{
			get => this.observableSource;

			set
			{
				if (this.observableSource != null)
				{
					this.observableSource.CollectionChanged -= CollectionChanged;
				}

				this.observableSource = value;

				if (this.observableSource != null)
				{
					this.observableSource.CollectionChanged += CollectionChanged;
				}
			}
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
			Children.Clear();

			if (ItemsSource == null)
			{
				ObservableSource = null;
				return;
			}

			foreach (object item in ItemsSource)
			{
				Children.Add(GetItemView(item));
			}

			bool isObservable = ItemsSource.GetType().GetTypeInfo().IsGenericType &&
				(ItemsSource.GetType().GetGenericTypeDefinition() == typeof(ObservableCollection<>));

			if (isObservable)
			{
				ObservableSource = new ObservableCollection<object>(ItemsSource.Cast<object>());
			}
		}

		private static void ItemsSourceChanged(BindableObject bindable, object value, object newValue)
		{
			RepeaterStackLayout itemsLayout = (RepeaterStackLayout)bindable;
			itemsLayout.SetItems();
		}

		private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
				{
					int index = e.NewStartingIndex;

					foreach (object item in e.NewItems)
					{
						Children.Insert(index++, GetItemView(item));
					}

					break;
				}

				case NotifyCollectionChangedAction.Move:
				{
					object item = ObservableSource[e.OldStartingIndex];

					Children.RemoveAt(e.OldStartingIndex);
					Children.Insert(e.NewStartingIndex, GetItemView(item));

					break;
				}

				case NotifyCollectionChangedAction.Remove:
				{
					Children.RemoveAt(e.OldStartingIndex);

					break;
				}

				case NotifyCollectionChangedAction.Replace:
				{
					Children.RemoveAt(e.OldStartingIndex);
					Children.Insert(e.NewStartingIndex, GetItemView(ObservableSource[e.NewStartingIndex]));

					break;
				}

				case NotifyCollectionChangedAction.Reset:
				{
					Children.Clear();

					foreach (object item in ItemsSource)
					{
						Children.Add(GetItemView(item));
					}

					break;
				}
			}
		}
	}
}