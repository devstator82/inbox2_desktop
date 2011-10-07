using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Framework.ComponentModel
{
	public class SourceAddressCollectionConverter : TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			SourceAddressCollection collection = new SourceAddressCollection();

			if (value == null)
				return collection;

			if (String.IsNullOrEmpty(value.ToString()))
				return collection;

			string[] parts = value.ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length > 0)
			{
				foreach (var part in parts)
					collection.Add(new SourceAddress(part.Trim()));
			}

			return collection;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			SourceAddressCollection collection = (SourceAddressCollection)value;

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < collection.Count; i++)
			{
				var address = collection[i];

				if (address == null)
					continue;

				sb.Append(address.ToString());

				if (i < collection.Count - 1)
					sb.Append("; ");
			}

			return sb.ToString();
		}
	}
}