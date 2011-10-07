using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.ComponentModel;
using Inbox2.Platform.Interfaces;
using Inbox2.Platform.Logging;

namespace Inbox2.Platform.Framework.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Serializes the current source object to xml and returns the resulting xml as a string.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static string ToXml(this object source)
		{
			XmlSerializer serializer = new XmlSerializer(source.GetType());

			return ToXml(source, serializer);
		}

		/// <summary>
		/// Serializes the current source object to xml and returns the resulting xml as a string.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="serializer">The serializer.</param>
		/// <returns></returns>
		public static string ToXml(this object source, XmlSerializer serializer)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				serializer.Serialize(ms, source);

				ms.Seek(0, SeekOrigin.Begin);

				using (StreamReader sr = new StreamReader(ms))
					return sr.ReadToEnd();
			}
		}
		
		/// <summary>
		/// Creates an instance from the serialized xml.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static T ToInstance<T>(this string source) where T : new()
		{
			return ToInstance<T>(source, false);
		}

		/// <summary>
		/// Creates an instance from the serialized xml.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="newOnNull">Boolean indicating wether to create a new instance if result is null.</param>
		/// <returns></returns>
		public static T ToInstance<T>(this string source, bool newOnNull) where T : new()
		{
			if (String.IsNullOrEmpty(source))
				return newOnNull ? new T() : default(T);

			return (T)ToInstance(source, typeof(T));
		}

		public static object ToInstance(this string source, Type t)
		{
			using (StringReader sr = new StringReader(source))
			{
				// Reader settings are used to ignore any unsupported ascii characters
				var xmlReaderSettings = new XmlReaderSettings
				{
					ConformanceLevel = ConformanceLevel.Fragment,
					ValidationType = ValidationType.None,
					CheckCharacters = false,
				};

				XmlReader xmlReader = XmlTextReader.Create(sr, xmlReaderSettings);
				XmlSerializer serializer = new XmlSerializer(t);
				return serializer.Deserialize(xmlReader);
			}
		}

		/// <summary>
		/// Creates an instance from the serialized xml.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="serializer">The serializer.</param>
		/// <returns></returns>
		public static T ToInstance<T>(this string source, XmlSerializer serializer)
		{
			using (StringReader sr = new StringReader(source))
			{
				return (T)serializer.Deserialize(sr);
			}
		}

		/// <summary>
		/// Deeps copies the given source object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static T DeepCopy<T>(this T source) where T : new()
		{
			var xml = source.ToXml();

			var newObj = xml.ToInstance<T>();

			Debug.Assert(ReferenceEquals(xml, newObj) == false, "Objects are not allowed to be the same");

			return newObj;
		}


		/// <summary>
		/// Deeps copies the given source object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="serializer">The serializer.</param>
		/// <returns></returns>
		public static T DeepCopy<T>(this T source, XmlSerializer serializer)
		{
			var xml = source.ToXml(serializer);

			var newObj = xml.ToInstance<T>(serializer);

			Debug.Assert(ReferenceEquals(xml, newObj) == false, "Objects are not allowed to be the same");

			return newObj;
		}

		/// <summary>
		/// Deep copies the given entity and returns a new one by matching property names.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static T DuckCopy<T>(this object source)
		{
			if (source == null)
				return default(T);;

			T instance = Activator.CreateInstance<T>();

			return source.DuckCopy<T>(instance);
		}

		/// <summary>
		/// Deep copies the given entity and returns a new one by matching property names.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="target">The target.</param>
		/// <returns></returns>
		public static T DuckCopy<T>(this object source, T target)
		{
			if (source == null)
				return default(T);

			PropertyInfo[] properties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo property in properties)
			{
				PropertyInfo pi = target.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);

				if (pi != null && pi.CanWrite)
				{
					if (pi.PropertyType == property.PropertyType)
					{
						// Copy property over as it has the same type
						pi.SetValue(target, property.GetValue(source, null), null);
					}
					else if (pi.PropertyType == typeof(String))
					{
						// Target property has type string, source does not, try to convert to string
						object val = property.GetValue(source, null);

						if (val != null)
						{
							// Edge case for stream
							if (val is MemoryStream)
								val = StreamExtensions.ReadString((val as MemoryStream));

							pi.SetValue(target, val.ToString(), null);
						}
					}
					else if (pi.PropertyType == typeof(SourceAddress))
					{
						// Target property has type SourceAddress, source does not
						object val = property.GetValue(source, null);

						if (val != null)
						{
							pi.SetValue(target, new SourceAddress(val.ToString()), null);
						}
					}
					else if (pi.PropertyType == typeof(SourceAddressCollection))
					{
						// Target property has type SourceAddress, source does not
						object val = property.GetValue(source, null);

						if (val != null)
						{
							pi.SetValue(target, new SourceAddressCollectionConverter().ConvertFrom(val.ToString()), null);
						}
					}
					else if (pi.PropertyType == typeof(MemoryStream))
					{
						// Target property has type SourceAddress, source does not
						object val = property.GetValue(source, null);

						if (val != null)
						{
							pi.SetValue(target, StreamExtensions.ToStream(val.ToString()), null);
						}
					}
					else if (pi.PropertyType == typeof(DateTime) || pi.PropertyType == typeof(DateTime?))
					{
						object val = property.GetValue(source, null);

						if (val != null && !String.IsNullOrEmpty(val.ToString()))
						{
							DateTime dt;

							if (DateTime.TryParse(val.ToString(), out dt))
								pi.SetValue(target, dt, null);
						}
					}
					else if (pi.PropertyType.IsEnum)
					{
						object val = property.GetValue(source, null);

						if (val != null)
						{
							pi.SetValue(target, Enum.Parse(pi.PropertyType, val.ToString(), true), null);
						}
					}
					else
					{
						if (pi.CanWrite)
						{
							TypeConverter conv = TypeDescriptor.GetConverter(pi.PropertyType);
							object value = property.GetValue(source, null);

							if (value != null && conv.CanConvertTo(pi.PropertyType))
								pi.SetValue(target, conv.ConvertFromString(value.ToString()), null);
						}
					}
				}
			}

			// Copy a reference to source if the element is IDuckClonable
			if (target is IDuckClonable)
				(target as IDuckClonable).Source = source;
			
			return target;
		}

		/// <summary>
		/// Fills the properties of the source object from the given SQLiteDataReader object.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="reader">The command.</param>
		public static void CreateFrom(this object source, IDataReader reader)
		{
			PropertyInfo[] properties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo property in properties)
			{
				if (property.GetCustomAttributes(typeof (MappingIgnoreAttribute), true).Length > 0)
					continue;

				object value;

				try
				{
					int ord = reader.GetOrdinal(property.Name);

					// Column not found in result set
					if (ord < 0)
						continue;

					value = reader.GetValue(ord);
				}
				catch (Exception)
				{
					continue;
				}

				try
				{
					if (value != null && value != DBNull.Value)
					{
						if (property.PropertyType.IsAssignableFrom(typeof(SourceAddress)))
						{
							property.SetValue(source, new SourceAddress(value.ToString()), null);
						}
						else if (property.PropertyType.IsAssignableFrom(typeof(SourceAddressCollection)))
						{
							property.SetValue(source, new SourceAddressCollection(value.ToString()), null);
						}
						if (property.PropertyType.IsAssignableFrom(typeof(MemoryStream)))
						{
							property.SetValue(source, value.ToString().ToStream(), null);
						}
						else if (property.PropertyType.IsAssignableFrom(typeof(DateTime)))
						{
							DateTime dt;

							if (DateTime.TryParse(value.ToString(), out dt))
								property.SetValue(source, dt, null);
						}
						else if (property.PropertyType.IsAssignableFrom(typeof(Boolean)))
						{
							// Will assign true if the value from database is 1
							property.SetValue(source, value.ToString() == "1" 
								|| value.ToString().ToLower() == "true", null);
						}
						else if (property.PropertyType.IsAssignableFrom(typeof(Enum)))
						{
							property.SetValue(source, Enum.Parse(property.PropertyType, value.ToString(), true), null);
						}
						else if (typeof(IPersistXml).IsAssignableFrom(property.PropertyType))
						{
							// Create object
							var obj = value.ToString().ToInstance(property.PropertyType);

							property.SetValue(source, obj, null);
						}
						else
						{
							TypeConverter conv = TypeDescriptor.GetConverter(property.PropertyType);

							property.SetValue(source, conv.ConvertFromString(value.ToString()), null);
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error("Unable to deserialize property of type {0} with value {1}, Exception = {2}", LogSource.Storage, property.PropertyType, value, ex);
				}
			}
		}

		/// <summary>
		/// Copies the properties of the source objects and adds them as parameters in
		/// the given SQLiteCommand object.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="command">The command.</param>
		public static void CopyTo(this object source, IDbCommand command)
		{
			PropertyInfo[] properties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo property in properties)
			{
				object value = property.GetValue(source, null);

				TypeConverter conv = TypeDescriptor.GetConverter(property.PropertyType);

				if (command.CommandText.IndexOf("@" + property.Name) > -1)
				{
					// The default DateTime converter does not save the seonds, this causes
					// problems with the threading feature when two messages are sent/received
					// in the same minute (which is quite likely to happen).
					string valueStr = value is DateTime
										? ((DateTime)value).ToString()
										: value == null ? String.Empty : conv.ConvertToString(value);

					var param = command.CreateParameter();
					param.ParameterName = "@" + property.Name;
					param.Value = valueStr;
				}
			}
		}

		public static IEnumerable<PropertyValue> GetProperties(this object o)
		{
			if (o != null)
			{
				PropertyDescriptorCollection props = TypeDescriptor.GetProperties(o);
				foreach (PropertyDescriptor prop in props)
				{
					object val = prop.GetValue(o);

					if (val != null)
					{
						yield return new PropertyValue { Name = prop.Name, Value = val };
					}
				}
			}
		}

		public static Dictionary<string, object> GetDictionary(this object o)
		{
			Dictionary<string, object> result = new Dictionary<string, object>();

			foreach (var value in o.GetProperties())
			{
				result.Add(value.Name, value.Value);
			}

			return result;
		}
	}
}