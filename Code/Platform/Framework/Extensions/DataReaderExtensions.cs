using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Platform.Framework.Extensions
{
	public static class DataReaderExtensions
	{
		public static bool ReadBoolean(this IDataReader reader, int index)
		{
			var str = reader.GetString(index);

			bool val;
			if (Boolean.TryParse(str, out val))
				return val;

			return false;
		}

		public static Int64? ReadInt64OrNull(this IDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
				return null;

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return null;

			return Int64.Parse(str);
		}

		public static Int32? ReadInt32OrNull(this IDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
				return null;

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return null;

			return Int32.Parse(str);
		}

		public static Int16? ReadInt16OrNull(this IDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
				return null;

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return null;

			return Int16.Parse(str);
		}

		public static Guid? ReadGuidOrNull(this IDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
				return null;

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return null;

			return new Guid(str);
		}

		public static DateTime ReadDateTime(this IDataReader reader, int index)
		{
			var str = reader.GetString(index);

			return DateTime.Parse(str);
		}

		public static DateTime ReadDateTimeOrDefault(this IDataReader reader, int index)
		{
			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return DateTime.MinValue;

			DateTime dt;

			if (DateTime.TryParse(str, out dt))
				return dt;

			return DateTime.MaxValue;
		}

		public static DateTime? ReadDateTimeOrNull(this IDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
				return null;

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return null;

			DateTime dt;

			if (DateTime.TryParse(str, out dt))
				return dt;

			return null;
		}

		public static SourceAddress ReadSourceAddressOrNull(this IDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
				return null;

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return null;

			return new SourceAddress(str);
		}

		public static SourceAddressCollection ReadSourceAddressCollection(this IDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
				return new SourceAddressCollection();

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return new SourceAddressCollection();

			return new SourceAddressCollection(str);
		}

		public static T ReadEnumOrDefault<T>(this IDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
				return default(T);

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return default(T);

			return (T) Enum.Parse(typeof (T), str);
		}

		public static T? ReadEnumOrNull<T>(this IDataReader reader, int index) where T : struct 
		{
			if (reader.IsDBNull(index))
				return null;

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return null;

			return (T)Enum.Parse(typeof(T), str);
		}

		public static T ReadInstanceOrNull<T> (this IDataReader reader, int index) where T : IPersistXml
		{
			if (reader.IsDBNull(index))
				return default(T);

			var str = reader.GetString(index);

			if (String.IsNullOrEmpty(str))
				return default(T);

			return (T)str.ToInstance(typeof(T));
		}
	}
}
