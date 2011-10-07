using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IClientStatusUpdatesChannel : IClientChannel
	{
		ChannelSocialProfile GetProfile();		

		IEnumerable<ChannelStatusUpdate> GetMentions(int pageSize);

		IEnumerable<ChannelStatusUpdate> GetUpdates(int pageSize);

		IEnumerable<ChannelStatusUpdate> GetUserUpdates(string username, int pageSize);

		IEnumerable<ChannelStatusUpdate> GetUpdates(string keyword, int pageSize);

		void UpdateMyStatus(ChannelStatusUpdate update);

		IClientStatusUpdatesChannel Clone();
	}
}
