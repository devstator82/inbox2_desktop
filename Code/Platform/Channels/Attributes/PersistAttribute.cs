using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class PersistAttribute : Attribute
	{
		public string FieldName { get; set; }

		public int MinimumVersion { get; set; }
	}
}