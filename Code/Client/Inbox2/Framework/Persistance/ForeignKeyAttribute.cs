using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Persistance
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ForeignKeyAttribute : Attribute
	{
		public string FieldName { get; set; }
	}
}
