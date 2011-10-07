using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Framework.Localization;

namespace Inbox2.Core.Threading.Tasks
{
    internal static class ExceptionHelper
    {
        internal static string BuildChannelError(IClientInputChannel channel, ConnectResult result, bool isException)
		{
			var messageBuilder = new StringBuilder();

			messageBuilder.Append(Strings.UnableToConnect);
			messageBuilder.Append(", ");

			if (isException)
			{
				messageBuilder.Append(Strings.DueToAnApplicationError);
			}
			else
			{
				switch (result)
				{
					case ConnectResult.AuthFailure:						
						messageBuilder.AppendFormat(Strings.ServerSaid, channel.AuthMessage);
						break;
					default:
						messageBuilder.Append(Strings.DueToAChannelError);						
						break;
				}
			}

            return messageBuilder.ToString();
		}
    }
}
