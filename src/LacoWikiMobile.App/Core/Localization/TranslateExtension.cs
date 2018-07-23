// <copyright file="TranslateExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Localization
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Microsoft.Extensions.Localization;
	using Prism.DryIoc;
	using Xamarin.Forms;
	using Xamarin.Forms.Xaml;
	using Xamarin.Forms.Xaml.Internals;

	// See https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/localization
	[ContentProperty("Text")]
	public class TranslateExtension : IMarkupExtension
	{
		public string Text { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException(nameof(serviceProvider));
			}

			TypeInfo rootObjectType = GetRootObjectType(serviceProvider);
			return GetStringLocalizer(rootObjectType.AsType())[Text];
		}

		protected static IStringLocalizer GetStringLocalizer(Type type)
		{
			Type stringLocalizerType = typeof(StringLocalizer<>);
			Type stringLocalizerTypeOfT = stringLocalizerType.MakeGenericType(type);

			return ((PrismApplication)Application.Current).Container.Resolve(stringLocalizerTypeOfT) as IStringLocalizer;
		}

		protected TypeInfo GetRootObjectType(IServiceProvider serviceProvider)
		{
			// Little workaround to get page object https://forums.xamarin.com/discussion/33678/x-reference-not-working
			IRootObjectProvider rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			SimpleValueTargetProvider valueTargetProvider =
				serviceProvider.GetService(typeof(IProvideValueTarget)) as SimpleValueTargetProvider;

			// If XamlCompilation is active, IRootObjectProvider is not available, but SimpleValueTargetProvider is available
			// If XamlCompilation is inactive, IRootObjectProvider is available, but SimpleValueTargetProvider is not available
			if (rootObjectProvider == null && valueTargetProvider == null)
			{
				throw new ArgumentException(
					$"{nameof(serviceProvider)} does not provide an {nameof(IRootObjectProvider)} or {nameof(SimpleValueTargetProvider)}");
			}

			if (rootObjectProvider != null)
			{
				return (rootObjectProvider.RootObject as Element).GetType().GetTypeInfo();
			}

			PropertyInfo propertyInfo = valueTargetProvider.GetType()
				.GetTypeInfo()
				.DeclaredProperties.FirstOrDefault(dp => dp.Name.Contains("ParentObjects"));

			if (propertyInfo == null)
			{
				throw new ArgumentNullException("ParentObjects");
			}

			IEnumerable<object> parentObjects = propertyInfo.GetValue(valueTargetProvider) as IEnumerable<object>;
			object parentObject = parentObjects.LastOrDefault(x => x.GetType().GetTypeInfo().IsSubclassOf(typeof(Element)));

			if (parentObject == null)
			{
				throw new ArgumentNullException("parentObject");
			}

			return (parentObject as Element).GetType().GetTypeInfo();
		}
	}
}