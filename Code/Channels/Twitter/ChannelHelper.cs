using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using Inbox2.Platform.Channels;

namespace Inbox2.Channels.Twitter
{
	static class ChannelHelper
	{
		public const string ConsumerKey = "IoP4hAoBrVYsEBpL5R1g";
		public const string ConsumerSecret = "k44MoU6qb1UEOE1d2p7sROblE50sCwQfa9ug0lthC8g";

		public static string Token
		{
			get { return ChannelContext.Current.ClientContext.GetSetting("/Channels/Twitter/AuthToken") as string; }
		}

		public static string TokenSecret
		{
			get { return ChannelContext.Current.ClientContext.GetSetting("/Channels/Twitter/AuthSecret") as string; }
		}
	}
}