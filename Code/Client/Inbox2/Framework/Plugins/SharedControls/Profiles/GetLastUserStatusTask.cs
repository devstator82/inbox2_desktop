using System;
using System.Linq;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Framework.Plugins.SharedControls.Profiles
{
	public class GetLastUserStatusTask : ChannelTask
	{
		private readonly SourceAddress address;
		public ChannelStatusUpdate Status { get; private set; }

		public GetLastUserStatusTask(ChannelConfiguration configuration, IClientChannel channel, SourceAddress address) : base(configuration, channel)
		{
			this.address = address;
		}

		protected override void ExecuteChannelCore()
		{
			var channel = (IClientStatusUpdatesChannel) Channel;
			Status = channel.GetUserUpdates(address.Address, 1).FirstOrDefault();
		}
	}
}
