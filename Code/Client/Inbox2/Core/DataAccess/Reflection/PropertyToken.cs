using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Core.DataAccess.Reflection
{
	class PropertyToken
	{
		public string ColumName { get; private set; }
		
		public object Value { get; private set; }

		public Type SourceType { get; private set; }

		public PropertyToken(string columName, object value, Type sourceType)
		{
			ColumName = columName;
			Value = value;
			SourceType = sourceType;
		}
	}
}