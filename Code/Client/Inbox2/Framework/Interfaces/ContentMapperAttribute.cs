using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces;

namespace Inbox2.Framework.Interfaces
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ContentMapperAttribute : Attribute
	{
		public Type MapperType { get; private set; }

		public ContentMapperAttribute(Type mapperTyoe)
		{
			MapperType = mapperTyoe;
		}
	}
}