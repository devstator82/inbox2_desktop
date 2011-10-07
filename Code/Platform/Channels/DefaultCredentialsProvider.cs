using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels
{
	public class DefaultCredentialsProvider : IChannelCredentialsProvider
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public DefaultCredentialsProvider(string username, string password)
		{
			Username = username;
			Password = password;
		}

		public virtual bool CanValidateCredentials
		{
			get { return false; }
		}

		public ChannelCredentials GetCredentials()
		{
			return new ChannelCredentials { Claim = Username, Evidence = Password };
		}

		public virtual ConnectResult ValidateCredentials()
		{
			return ConnectResult.AuthFailure;
		}
	}
}