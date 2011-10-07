using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class SortOnPropertyAttribute : Attribute
	{
		private string propertyName;

		public string PropertyName
		{
			get { return propertyName; }
		}

		public SortOnPropertyAttribute(string propertyName)
		{
			this.propertyName = propertyName;
		}
	}
}
