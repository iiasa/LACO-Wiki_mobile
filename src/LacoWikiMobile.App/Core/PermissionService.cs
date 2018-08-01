// <copyright file="PermissionService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Plugin.Permissions;
	using Plugin.Permissions.Abstractions;

	public class PermissionService : IPermissionService
	{
		public async Task<bool> CheckAndRequestPermissionIfRequiredAsync(Permission permission)
		{
			PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

			if (status == PermissionStatus.Granted)
			{
				return true;
			}

			Dictionary<Permission, PermissionStatus> results = await CrossPermissions.Current.RequestPermissionsAsync(permission);

			if (results.ContainsKey(permission))
			{
				status = results[permission];
			}

			if (status == PermissionStatus.Granted)
			{
				return true;
			}

			return false;
		}

		public async Task<bool> CheckAndRequestPermissionsIfRequiredAsync(IEnumerable<Permission> permissions)
		{
			foreach (Permission permission in permissions)
			{
				if (!await CheckAndRequestPermissionIfRequiredAsync(permission))
				{
					return false;
				}
			}

			return true;
		}
	}
}