using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Network.Client
{
	public class ClientException : ApplicationException
	{
		public ClientException(string message, params object[] args) : base(string.Format(message, args))
		{
		}

		public ClientException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
