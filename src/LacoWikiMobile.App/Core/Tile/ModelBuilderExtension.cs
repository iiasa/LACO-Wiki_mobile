// <copyright file="ModelBuilderExtension.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Core.Tile
{
	using System;
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public static class ModelBuilderExtension
	{
		// See https://github.com/aspnet/EntityFrameworkCore/issues/12248#issuecomment-395450990
		public static void ConfigureEnums(this ModelBuilder modelBuilder, EnumOptions enumOptions)
		{
			foreach (IMutableProperty property in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetProperties()).ToList())
			{
				Type propertyType = property.ClrType;

				if (!propertyType.IsEnumOrNullableEnumType())
				{
					continue;
				}

				IMutableEntityType entityType = property.DeclaringEntityType;

				Type concreteType = typeof(EnumLookup<>).MakeGenericType(propertyType.GetEnumOrNullableEnumType());
				EntityTypeBuilder enumLookupBuilder = modelBuilder.Entity(concreteType);

				string typeName = propertyType.GetEnumOrNullableEnumType().Name;
				string tableName = enumOptions.NamingFuction(typeName);
				enumLookupBuilder.ToTable(tableName);

				// TODO: Check status of https://github.com/aspnet/EntityFrameworkCore/issues/12194 before using migrations
				object[] data = Enum.GetValues(propertyType.GetEnumOrNullableEnumType())
					.OfType<object>()
					.Select(x =>
					{
						object instance = Activator.CreateInstance(concreteType);

						concreteType.GetProperty(nameof(EnumLookup<object>.Id)).SetValue(instance, (int)x);
						concreteType.GetProperty(nameof(EnumLookup<object>.Value)).SetValue(instance, (int)x);
						concreteType.GetProperty(nameof(EnumLookup<object>.Name)).SetValue(instance, x.ToString());

						return instance;
					})
					.ToArray();

				enumLookupBuilder.HasData(data);

				modelBuilder.Entity(entityType.Name)
					.HasOne(concreteType)
					.WithMany()
					.HasPrincipalKey(nameof(EnumLookup<Enum>.Value))
					.HasForeignKey(property.Name);
			}
		}

		public static void ConfigureNames(this ModelBuilder modelBuilder, NamingOptions namingOptions)
		{
			foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
			{
				if (namingOptions.EntitiesToSkipEntirely(entity))
				{
					continue;
				}

				// Entity / Table
				if (!namingOptions.EntitiesToSkipTableNaming(entity))
				{
					string tableName = namingOptions.TableNameSource(entity);

					entity.Relational().TableName = namingOptions.TableNamingFunction(tableName);
				}

				// Properties
				entity.GetProperties()
					.ToList()
					.ForEach(x => x.Relational().ColumnName = namingOptions.PropertyNamingFunction(x.Relational().ColumnName));

				// Primary and Alternative keys
				entity.GetKeys().ToList().ForEach(x => x.Relational().Name = namingOptions.ConstraintNamingFunction(x.Relational().Name));

				// Foreign keys
				entity.GetForeignKeys()
					.ToList()
					.ForEach(x => x.Relational().Name = namingOptions.ConstraintNamingFunction(x.Relational().Name));

				// Indices
				entity.GetIndexes()
					.ToList()
					.ForEach(x => x.Relational().Name = namingOptions.ConstraintNamingFunction(x.Relational().Name));
			}
		}

		public static Type GetEnumOrNullableEnumType(this Type propertyType)
		{
			if (!propertyType.IsEnumOrNullableEnumType())
			{
				return null;
			}

			return propertyType.IsEnum ? propertyType : propertyType.GetGenericArguments()[0];
		}

		public static bool IsEnumOrNullableEnumType(this Type propertyType)
		{
			if (propertyType.IsEnum)
			{
				return true;
			}

			if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				if (propertyType.GetGenericArguments()[0].IsEnum)
				{
					return true;
				}
			}

			return false;
		}
	}
}