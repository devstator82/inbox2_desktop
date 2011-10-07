using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	public static class SendStates
	{
		public const int None = 0;
		public const int Initial = 10;
		public const int Sending = 20;
		public const int Sent = 30;
		public const int DoNotSend = 100;
	}
}