using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces
{
	public interface IContextTask
	{
		Dictionary<string, object> Values { get; }
	}
}
