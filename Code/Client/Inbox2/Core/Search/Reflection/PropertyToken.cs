using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Core.Search.Reflection
{
	class PropertyToken
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public bool Tokenize { get; set; }

		public bool Store { get; set; }
	}
}
