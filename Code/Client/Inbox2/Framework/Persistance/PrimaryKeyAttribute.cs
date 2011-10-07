using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Attributes;

namespace Inbox2.Framework.Persistance
{
	[AttributeUsage(AttributeTargets.Property)]
	public class PrimaryKeyAttribute : Attribute
	{
		public string FieldName { get; set; }
	}
}
