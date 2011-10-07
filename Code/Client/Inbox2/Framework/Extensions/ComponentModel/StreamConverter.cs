using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Extensions;

namespace Inbox2.Framework.Extensions.ComponentModel
{
	public class StreamConverter : TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value == null)
				return new MemoryStream();

			if (String.IsNullOrEmpty(value.ToString()))
				return new MemoryStream();

			return value.ToString().ToStream();
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			Stream s = (Stream)value;

			return s.ReadString();
		}
	}
}
