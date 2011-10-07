using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Exceptions;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Core.Threading.Tasks.Users
{
	public class ValidateLoginTask : ChannelTask, IContextTask
	{
		private readonly ChannelConfiguration configuration;
		private readonly IClientInputChannel channel;
		private readonly SetupChannelClientContext context;

		public string RedirectResult { get; set; }

		public Dictionary<string, object> Values
		{
			get { return context.Values; }
		}

		public ValidateLoginTask(ChannelConfiguration configuration, IClientInputChannel channel)
			: base(configuration, channel)
		{
			this.configuration = configuration;
			this.channel = channel;
			this.context = new SetupChannelClientContext();
		}

		protected override void ExecuteChannelCore()
		{
			ChannelContext.Current.ClientContext = context;

			switch (configuration.DisplayStyle)
			{
				case DisplayStyle.Simple:
				case DisplayStyle.Advanced:
				case DisplayStyle.Other:
					ValidateCredentials();
					break;
				case DisplayStyle.Redirect:
				case DisplayStyle.RedirectWithPin:
				case DisplayStyle.FbConnect:
					ValidatePin();
					break;
			}
		}

		void ValidatePin()
		{
			var builder = configuration.RedirectBuilder;
			var verifier = builder.ParseVerifier(RedirectResult);

			if (!builder.ValidateReturnValue(verifier))
				throw new ChannelAuthenticationException();

			var username = builder.GetUsername();

			// Save the username into our configuration
			ChannelBuilder.SetChannelAuthentication(username, String.Empty, configuration.InputChannel,
				configuration.OutputChannel, configuration.ContactsChannel,
				configuration.CalendarChannel, configuration.StatusUpdatesChannel);
		}

		void ValidateCredentials()
		{
			if (channel.CredentialsProvider.CanValidateCredentials)
			{
				// Use credentialsprovider if it is capable
				channel.CredentialsProvider.ValidateCredentials();
			}
			else
			{
				var result = channel.Connect();

				channel.Disconnect();

				if (result != ConnectResult.Success)
				{
					Inbox2MessageBox.Show(ExceptionHelper.BuildChannelError(channel, result, false), Inbox2MessageBoxButton.OK);

					throw new ChannelAuthenticationException();
				}
			}
		}
	}
}