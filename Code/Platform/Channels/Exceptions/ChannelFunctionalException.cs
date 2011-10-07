using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Exceptions
{
	public class ChannelFunctionalException : ChannelException
	{
		public bool DoNotRetry { get; set; }

		public bool DoNotEnqueue { get; set; }

		public ChannelFunctionalException(string message) : base(message)
		{
		}

		public ChannelFunctionalException(string message, Exception ex, params string[] format) : base(message, ex, format)
		{
		}

		public ChannelFunctionalException(string message, params string[] format) : base(message, format)
		{
		}

		public ChannelFunctionalException()
		{
		}

		public ChannelFunctionalException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public ChannelFunctionalException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
