using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Exceptions
{
	public class ChannelException : ApplicationException
	{
		public ChannelException(string message) : base(message)
		{
		}

		public ChannelException(string message, Exception ex, params string[] format)
			: base(String.Format(message, format), ex)
		{
		}

		public ChannelException(string message, params string[] format)
			: base(String.Format(message, format))
		{
		}

		public ChannelException()
		{
		}

		public ChannelException(SerializationInfo info, StreamingContext context) 
			: base(info, context)
		{
		}

		public ChannelException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}