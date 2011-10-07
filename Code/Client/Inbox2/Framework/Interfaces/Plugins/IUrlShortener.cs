using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IUrlShortener
	{
		string Name { get; }

		string Shorten(string url);
	}
}
