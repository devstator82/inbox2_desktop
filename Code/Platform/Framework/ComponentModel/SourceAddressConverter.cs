using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Framework.ComponentModel
{
	public class SourceAddressConverter : TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
				return null;

			if (String.IsNullOrEmpty(value.ToString()))
				return null;

			SourceAddress address = new SourceAddress(value.ToString());

			return address;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof (String);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			SourceAddress address = (SourceAddress) value;

			return address.ToString(true);
		}
	}
}
