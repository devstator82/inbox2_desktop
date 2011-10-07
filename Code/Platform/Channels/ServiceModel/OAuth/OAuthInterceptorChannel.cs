using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Inbox2.Platform.Channels.ServiceModel.OAuth
{
	class OAuthInterceptorChannel : ChannelBase, IRequestChannel
	{
		IRequestChannel innerChannel;

		public OAuthInterceptorChannel(ChannelManagerBase channelManager, IRequestChannel innerChannel)
			: base(channelManager)
		{
			this.innerChannel = innerChannel;
		}

		#region Channel plumbing

		public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
		{
			return innerChannel.BeginRequest(message, timeout, callback, state);
		}

		public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
		{
			return BeginRequest(message, this.DefaultSendTimeout, callback, state);
		}

		public Message EndRequest(IAsyncResult result)
		{
			return innerChannel.EndRequest(result);
		}

		protected override void OnAbort()
		{
			innerChannel.Abort();
		}

		protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
		{
			return innerChannel.BeginClose(timeout, callback, state);
		}

		protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
		{
			return innerChannel.BeginOpen(timeout, callback, state);
		}

		protected override void OnClose(TimeSpan timeout)
		{
			innerChannel.Close(timeout);
		}

		protected override void OnEndClose(IAsyncResult result)
		{
			innerChannel.EndClose(result);
		}

		protected override void OnEndOpen(IAsyncResult result)
		{
			innerChannel.EndOpen(result);
		}

		protected override void OnOpen(TimeSpan timeout)
		{
			innerChannel.Open(timeout);
		}

		public EndpointAddress RemoteAddress
		{
			get { return innerChannel.RemoteAddress; }
		}

		public Message Request(Message message)
		{
			return Request(message, DefaultSendTimeout);
		}

		public Uri Via
		{
			get { return innerChannel.Via; }
		}

		#endregion

		public Message Request(Message request, TimeSpan timeout)
		{
			Message reply = null;

			if (request != null)
			{
				reply = innerChannel.Request(request, timeout);
			}

			return reply;
		}
	}
}