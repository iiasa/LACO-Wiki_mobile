// <copyright file="IApiAuthentication.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api
{
	using System;
	using System.Threading.Tasks;

	public interface IApiAuthentication
	{
		event EventHandler<EventArgs> Authenticated;

		ExtendedOAuth2Authenticator Authenticator { get; }

		void AuthenticateWithFacebook();

		void AuthenticateWithGeoWiki();

		void AuthenticateWithGoogle();

		Task EnsureAuthenticatedAsync();

		Task<string> GetAccessTokenAsync();

		Task<string> GetProviderNameAsync();

		Task<int> GetUserIdAsync();

		Task<string> GetUserNameAsync();

		Task<bool> IsAuthenticatedAsync();

		Task SignOutAsync();
	}
}