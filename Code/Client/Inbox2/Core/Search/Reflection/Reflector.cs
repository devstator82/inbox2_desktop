using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Search.Reflection
{
	static class Reflector
	{
		public static IEnumerable<IContentMapper> GetMappers<T>(T source)
		{
			Type type = typeof(T);

			var attributes = type.GetCustomAttributes(typeof(ContentMapperAttribute), false);

			if (attributes != null && attributes.Length > 0)
			{
				foreach (ContentMapperAttribute attribute in attributes)
				{
					yield return (IContentMapper)Activator.CreateInstance(attribute.MapperType, source);
				}
			}
		}

		public static string GetPrimaryKey<T>()
		{
			var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (var property in properties)
			{
				var attributes = property.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);

				if (attributes != null && attributes.Length > 0)
				{
					return property.Name;
				}
			}

			Logger.Error("Unable to determine primary key for entity {0}", LogSource.Search, typeof(T));

			return null;
		}

		public static PropertyInfo GetPrimaryKeyInstance<T>()
		{
			var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (var property in properties)
			{
				var attributes = property.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);

				if (attributes != null && attributes.Length > 0)
				{
					return property;
				}
			}

			Logger.Error("Unable to determine primary key for entity {0}", LogSource.Search, typeof(T));

			return null;
		}

		public static IEnumerable<PropertyToken> GetTokensFrom<T>(T instance, params IContentMapper[] mappers)
		{
			var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (var property in properties)
			{
				var attributes = property.GetCustomAttributes(typeof(IndexAttribute), true);

				if (attributes != null && attributes.Length > 0)
				{
					IndexAttribute attr = (IndexAttribute)attributes[0];

					object value = null;

					if (instance != null)
					{
						value = GetValue(instance, property, mappers);
					}

					// Value can be converted using IContentMapper, so try to retrieve the correct type converter
					var actualType = value == null ? property.PropertyType : value.GetType();

					TypeConverter conv = TypeDescriptor.GetConverter(actualType);

					yield return new PropertyToken
					{
						Name = property.Name,
						Store = attr.Store,
						Tokenize = attr.Tokenize,
						Value = conv.ConvertToString(value)
					};
				}
			}
		}

		public static object GetValue<T>(T instance, PropertyInfo property, IContentMapper[] mappers)
		{
			if (mappers.Length > 0)
			{
				foreach (var mapper in mappers)
				{
					if (mapper.PropertyName == property.Name)
						return mapper.GetContent();
				}
			}

			return property.GetValue(instance, null);
		}
	}
}
