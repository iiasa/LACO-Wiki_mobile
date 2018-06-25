// <copyright file="JwtSecurityTokenExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;

	public static class JwtSecurityTokenExtension
	{
		public static string ProviderName(this JwtSecurityToken jwtSecurityToken)
		{
			return jwtSecurityToken.Claims.Single(x => x.Type == "idp").Value;
		}

		public static int UserId(this JwtSecurityToken jwtSecurityToken)
		{
			return int.Parse(jwtSecurityToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
		}

		public static string UserName(this JwtSecurityToken jwtSecurityToken)
		{
			return jwtSecurityToken.Claims.Single(x => x.Type == "name").Value;
		}
	}
}