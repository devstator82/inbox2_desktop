using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MappingIgnoreAttribute : Attribute
	{
	}
}