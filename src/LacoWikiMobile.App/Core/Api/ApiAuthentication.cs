// <copyright file="ApiAuthentication.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Api
{
	using System;
	using System.IdentityModel.Tokens.Jwt;
	using System.Threading.Tasks;
	using Xamarin.Auth.Presenters;
	using Xamarin.Essentials;

	public class ApiAuthentication : IApiAuthentication
	{
		protected const string AccessTokenKey = "access_token";

		protected const string UserIdKey = "user_id";

		protected const string UserNameKey = "user_name";

		protected const string ProviderNameKey = "provider";

		public ApiAuthentication(INotificationService notificationService)
		{
			NotificationService = notificationService;

			Authenticator = new ExtendedOAuth2Authenticator("webapi", "webapi", new Uri("https://laco-wiki.net/identity/connect/authorize"),
				new Uri("laco-wiki-app://"), null, true);

			Authenticator.Completed += async (sender, args) =>
			{
				if (args.IsAuthenticated)
				{
					string accessToken = args.Account.Properties["access_token"];
					JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(accessToken);

					await SecureStorage.SetAsync(ApiAuthentication.AccessTokenKey, accessToken);
					await SecureStorage.SetAsync(ApiAuthentication.UserIdKey, jwtSecurityToken.UserId().ToString());
					await SecureStorage.SetAsync(ApiAuthentication.UserNameKey, jwtSecurityToken.UserName());
					await SecureStorage.SetAsync(ApiAuthentication.ProviderNameKey, jwtSecurityToken.ProviderName());

					Authenticated?.Invoke(this, EventArgs.Empty);
				}
			};

			Authenticator.Error += (sender, args) => { };
		}

		public event EventHandler<EventArgs> Authenticated;

		public ExtendedOAuth2Authenticator Authenticator { get; set; }

		protected INotificationService NotificationService { get; set; }

		public void AuthenticateWithFacebook()
		{
			Authenticate("Facebook");
		}

		public void AuthenticateWithGeoWiki()
		{
			Authenticate("GeoWiki");
		}

		public void AuthenticateWithGoogle()
		{
			Authenticate("Google");
		}

		public async Task EnsureAuthenticatedAsync()
		{
			if (!await IsAuthenticatedAsync())
			{
				throw new NotAuthenticatedException();
			}
		}

		public async Task<string> GetAccessTokenAsync()
		{
			string accessToken = await SecureStorage.GetAsync(ApiAuthentication.AccessTokenKey);

			if (string.IsNullOrEmpty(accessToken))
			{
				throw new InvalidOperationException();
			}

			return accessToken;
		}

		public async Task<int> GetUserIdAsync()
		{
			return int.Parse(await GetValueAsync(ApiAuthentication.UserIdKey));
		}

		public async Task<string> GetUserNameAsync()
		{
			return await GetValueAsync(ApiAuthentication.UserNameKey);
		}

		public async Task<string> GetProviderNameAsync()
		{
			return await GetValueAsync(ApiAuthentication.ProviderNameKey);
		}

		public async Task<string> GetValueAsync(string key)
		{
			string value = await SecureStorage.GetAsync(key);

			if (string.IsNullOrEmpty(value))
			{
				throw new InvalidOperationException();
			}

			return value;
		}

		public async Task<bool> IsAuthenticatedAsync()
		{
			string accessToken = await SecureStorage.GetAsync(ApiAuthentication.AccessTokenKey);

			return !string.IsNullOrEmpty(accessToken);
		}

		public async Task SignOutAsync()
		{
			await SecureStorage.SetAsync(ApiAuthentication.AccessTokenKey, string.Empty);
		}

		protected void Authenticate(string provider)
		{
			Authenticator.AcrValues = $"idp:{provider}";

			OAuthLoginPresenter presenter = new OAuthLoginPresenter();
			presenter.Login(Authenticator);
		}
	}
}