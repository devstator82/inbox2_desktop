using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Core.Threading
{
    public static class ExceptionHelper
    {
		public static string BuildChannelError(IClientInputChannel channel, ConnectResult result, bool isException)
		{
			var messageBuilder = new StringBuilder();

			messageBuilder.AppendFormat("Unable to connect ");

			if (isException)
			{
				messageBuilder.AppendFormat(" due to an application error");
			}
			else
			{
				switch (result)
				{
					case ConnectResult.AuthFailure:
						messageBuilder.AppendFormat(", server said: {0}", channel.AuthMessage);
						break;
					default:
						messageBuilder.AppendFormat(" due to a channel error");
						break;
				}
			}

            return messageBuilder.ToString();
		}
    }
}
