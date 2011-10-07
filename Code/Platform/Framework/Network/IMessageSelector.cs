using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Network
{
	public interface IMessageSelector
	{
		byte[] Select(byte[] data);
	}
}
