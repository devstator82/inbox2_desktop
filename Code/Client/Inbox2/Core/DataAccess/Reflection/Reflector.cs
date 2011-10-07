using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;

namespace Inbox2.Core.DataAccess.Reflection
{
	static class Reflector
	{
		public static TableMap BuildMapFrom<T>()
		{
			return BuildMapFromType(typeof(T));
		}

		public static TableMap BuildMapFromType(Type type)
		{
			return BuildMapFromInstance(type, null);
		}

		public static TableMap BuildMapFromInstance<T>(T instance)
		{
			return BuildMapFromInstance(instance.GetType(), instance);
		}

		public static TableMap BuildMapFromInstance(Type type, object instance)
		{
			PersistableClassAttribute persistableClassAttribute = GetAttributeFrom<PersistableClassAttribute>(type);

			if (persistableClassAttribute == null)
				throw new ApplicationException("Instance was not marked with the Persistable attribute!");

			// Make sure a valid name is specified for the table name
			if (String.IsNullOrEmpty(persistableClassAttribute.TableName))
				persistableClassAttribute.TableName = type.Name + "s";

			// Find PK property
			var primaryKeyFields = GetPropertiesForAttribute<PrimaryKeyAttribute>(type).ToList();

			if (primaryKeyFields.Count == 0)
				throw new ApplicationException("Instance did not have a property marked with the PrimaryKey attribute!");

			if (primaryKeyFields.Count > 1)
				throw new ApplicationException("Instance has more then one property marked with the PrimaryKey attribute!");

			// Todo: make sure the PK is nullable

			PrimaryKeyAttribute primaryKeyAttribute = GetAttributeFrom<PrimaryKeyAttribute>(primaryKeyFields[0]);

			// Make sure a valid name is specified for the primary key
			if (String.IsNullOrEmpty(primaryKeyAttribute.FieldName))
				primaryKeyAttribute.FieldName = primaryKeyFields[0].Name;

			object value = instance == null ? null : primaryKeyFields[0].GetValue(instance, null);
			
			PropertyToken primaryKeyToken = new PropertyToken(primaryKeyAttribute.FieldName, value, primaryKeyFields[0].PropertyType);

			// Get columns to persist
			var columns = new List<PropertyToken>();
			var columnProperties = GetPropertiesForAttribute<PersistAttribute>(type).ToList();

			foreach (var columnProperty in columnProperties)
			{
				object columnValue = instance == null ? null : columnProperty.GetValue(instance, null);

				columns.Add(new PropertyToken(columnProperty.Name, columnValue, columnProperty.PropertyType));
			}

			return new TableMap { TableName = persistableClassAttribute.TableName, PrimaryKey = primaryKeyToken, Columns = columns };
		}

		public static void SetPropertyValue(object instance, PropertyToken token, object newValue)
		{
			PropertyInfo property = instance.GetType().GetProperty(token.ColumName, BindingFlags.Instance | BindingFlags.Public);

			if (property == null)
				throw new ApplicationException(String.Format("Property {0} was not found on object {1}", token.ColumName, instance));

			property.SetValue(instance, newValue, null);
		}

		static A GetAttributeFrom<A>(Type type) where A : Attribute
		{
			var attributes = type.GetCustomAttributes(typeof(A), true);

			if (attributes != null && attributes.Length > 0)
			{
				return (A)attributes.First();
			}

			return default(A);			
		}

		static A GetAttributeFrom<A>(PropertyInfo instance) where A : Attribute
		{
			var attributes = instance.GetCustomAttributes(typeof(A), true);

			if (attributes != null && attributes.Length > 0)
			{
				return (A)attributes.First();
			}

			return default(A);
		}

		static IEnumerable<PropertyInfo> GetPropertiesForAttribute<A>(Type type) where A : Attribute
		{
			var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			if (properties != null && properties.Length > 0)
			{
				foreach (var property in properties)
				{
					var attributes = property.GetCustomAttributes(typeof (A), true);

					if (attributes != null && attributes.Length > 0)
					{
						yield return property;
					}
				}
			}
		}
	}
}