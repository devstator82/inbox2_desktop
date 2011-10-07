using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Platform.Channels.Extensions
{
	public static class ObjectExtensions
	{
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
	}

	public sealed class PropertyValue
	{
		private object value;

		public string Name { get; set; }			
		
		public object Value
		{
			get
			{
				if (value == null)
					return null;

				if (value is DateTime)
					return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");

				if (value is Boolean)
					return ((bool)value) ? 1 : 0;

				if (value is SourceAddress)
					return ((SourceAddress) value).ToString(true);

				if (value is IPersistXml)
				{
					using (MemoryStream ms = new MemoryStream())
					{
						XmlSerializer ser = new XmlSerializer(value.GetType());
						ser.Serialize(ms, value);

						ms.Seek(0, SeekOrigin.Begin);

						using (StreamReader sr = new StreamReader(ms))
							return sr.ReadToEnd();
					}
				}

				return value.ToString();
			}
			set
			{
				this.value = value;
			}
		}
	}
}
