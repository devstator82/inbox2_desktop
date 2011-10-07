using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Exceptions
{
	public class ChannelAuthenticationException : ChannelException
	{
		public ChannelAuthenticationException(string message) : base(message)
		{
		}

		public ChannelAuthenticationException(string message, Exception ex, params string[] format) : base(message, ex, format)
		{
		}

		public ChannelAuthenticationException(string message, params string[] format) : base(message, format)
		{
		}

		public ChannelAuthenticationException()
		{
		}

		public ChannelAuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public ChannelAuthenticationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}