using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	public class DataEventArgs : EventArgs
	{
		public string Data { get; private set; }

		public DataEventArgs(string data)
		{
			Data = data;
		}
	}
}