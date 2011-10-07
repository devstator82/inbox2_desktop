using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Core.DataAccess.Reflection
{
	class TableMap
	{
		public string TableName { get; set; }

		public PropertyToken PrimaryKey { get; set; }

		public List<PropertyToken> Columns { get; set; }
	}
}