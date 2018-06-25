// <copyright file="ExtendedOAuth2Authenticator.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api
{
	using System;
	using System.Collections.Generic;
	using Xamarin.Auth;

	public class ExtendedOAuth2Authenticator : OAuth2Authenticator
	{
		public ExtendedOAuth2Authenticator(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl,
			GetUsernameAsyncFunc getUsernameAsync = null, bool isUsingNativeUI = false)
			: base(clientId, scope, authorizeUrl, redirectUrl, getUsernameAsync, isUsingNativeUI)
		{
		}

		public ExtendedOAuth2Authenticator(string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl,
			Uri accessTokenUrl, GetUsernameAsyncFunc getUsernameAsync = null, bool isUsingNativeUI = false)
			: base(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl, getUsernameAsync, isUsingNativeUI)
		{
		}

		public string AcrValues { get; set; }

		protected override void OnCreatingInitialUrl(IDictionary<string, string> query)
		{
			if (!string.IsNullOrEmpty(AcrValues))
			{
				query["acr_values"] = AcrValues;
			}

			base.OnCreatingInitialUrl(query);
		}
	}
}