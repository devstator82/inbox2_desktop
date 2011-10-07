using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IChannelCredentialsProvider
	{
		bool CanValidateCredentials { get; }

		ChannelCredentials GetCredentials();

		ConnectResult ValidateCredentials();
	}
}