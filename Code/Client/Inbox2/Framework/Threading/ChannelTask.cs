using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Framework.Threading
{
	public abstract class ChannelTask : BackgroundTask
	{
		private readonly IClientChannel channel;
		private readonly ChannelConfiguration configuration;

		public override bool RequiresNetworkConnection
		{
			get { return true; }
		}

		protected IClientChannel Channel
		{
			get { return channel; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ChannelTask"/> class.
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="channel">The channel.</param>
		protected ChannelTask(ChannelConfiguration configuration, IClientChannel channel)
		{
			this.channel = channel;
			this.configuration = configuration;
		}

		protected override bool CanExecute()
		{
			ChannelContext.Current.ClientContext = new ChannelClientContext(ClientState.Current.Context, configuration);

			return CanExecuteCore();
		}

		protected override void ExecuteCore()
		{
			try
			{
				if (channel is IPoolableChannel)
					((IPoolableChannel) channel).EnsureConnectionPool();

				ChannelContext.Current.ClientContext = new ChannelClientContext(ClientState.Current.Context, configuration);

				ExecuteChannelCore();
			}
			catch
			{
				// Exception has occured, recycle connection
				if (channel is IPoolableChannel)
					((IPoolableChannel)channel).FreeConnection();

			    throw;
			}
			finally
			{
				// Disconnects the channel
				if (channel is IClientInputChannel)
					((IClientInputChannel)channel).Disconnect();
			}			
		}

		protected virtual bool CanExecuteCore()
		{
			return true;
		}

		public override void Dispose()
		{
			if (Channel is IPoolableChannel)
				(Channel as IPoolableChannel).FreeAllConnections();
		}

		protected abstract void ExecuteChannelCore();
	}
}
