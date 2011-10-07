using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Persistance
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PersistableClassAttribute : Attribute
	{
		public string TableName { get; set; }

		public PersistableClassAttribute()
		{
		}
	}
}
