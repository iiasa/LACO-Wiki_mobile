// <copyright file="IPermissionService.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Plugin.Permissions.Abstractions;

	public interface IPermissionService
	{
		Task<bool> CheckAndRequestPermissionIfRequiredAsync(Permission permission);

		Task<bool> CheckAndRequestPermissionsIfRequiredAsync(IEnumerable<Permission> permissions);
	}
}