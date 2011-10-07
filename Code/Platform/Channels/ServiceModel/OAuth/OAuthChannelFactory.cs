using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Inbox2.Platform.Channels.ServiceModel.OAuth
{
	class OAuthChannelFactory : ChannelFactoryBase<IRequestChannel>
	{
		IChannelFactory<IRequestChannel> innerChannelFactory;

		public OAuthChannelFactory(IChannelFactory<IRequestChannel> innerChannelFactory)
		{
			this.innerChannelFactory = innerChannelFactory;
		}

		protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
		{
			return innerChannelFactory.BeginOpen(timeout, callback, state);
		}

		protected override IRequestChannel OnCreateChannel(EndpointAddress address, Uri via)
		{
			IRequestChannel innerChannel = innerChannelFactory.CreateChannel(address, via);

			return new OAuthInterceptorChannel(this, innerChannel);
		}

		protected override void OnEndOpen(IAsyncResult result)
		{
			innerChannelFactory.EndOpen(result);
		}

		protected override void OnOpen(TimeSpan timeout)
		{
			innerChannelFactory.Open(timeout);
		}

		public override T GetProperty<T>()
		{
			T baseProperty = base.GetProperty<T>();

			if (baseProperty != null)
			{
				return baseProperty;
			}

			return this.innerChannelFactory.GetProperty<T>();
		}
	}
}