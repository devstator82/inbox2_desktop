using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace Inbox2.Platform.Channels.ServiceModel.OAuth
{
	public class OAuthInterceptorBindingElement : BindingElement
	{
		public override BindingElement Clone()
		{
			return new OAuthInterceptorBindingElement();
		}

		public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
		{
			if (!CanBuildChannelFactory<TChannel>(context))
			{
				throw new ArgumentException();
			}

			IChannelFactory<IRequestChannel> innerChannelFactory = (IChannelFactory<IRequestChannel>)base.BuildChannelFactory<TChannel>(context);

			return (IChannelFactory<TChannel>)new OAuthChannelFactory(innerChannelFactory);
		}

		public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
		{
			//we cant build this
			throw new NotSupportedException();
		}

		public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
		{
			if (typeof(TChannel) != typeof(IRequestChannel))
			{
				return false;
			}

			return base.CanBuildChannelFactory<TChannel>(context);
		}

		public override bool CanBuildChannelListener<TChannel>(BindingContext context)
		{
			return false;
		}

		public override T GetProperty<T>(BindingContext context)
		{
			return context.GetInnerProperty<T>();
		}
	}
}