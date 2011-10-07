using System;
using System.Threading;
using System.Web;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Threading;
using Inbox2.Framework.Threading.Progress;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.Web;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Commands
{
	class SendStatusUpdateCommand : ConnectedCommand
	{
		private readonly UserStatus status;

		public SendStatusUpdateCommand(UserStatus status)
		{
			this.status = status;
		}

		protected override void ExecuteCore()
		{
			var group = new ProgressGroup { Status = Strings.UpdatingStatus };

			ProgressManager.Current.Register(group);

			try
			{
				foreach (var config in status.TargetChannels)
				{
					var channel = ChannelsManager.GetChannelObject(config.ChannelId);

					try
					{
						group.SourceChannelId = config.ChannelId;						

						ChannelContext.Current.ClientContext = new ChannelClientContext(ClientState.Current.Context, config);

						Logger.Debug("Updating status. Channel = {0}", LogSource.BackgroundTask, config.DisplayName);

						if (config.IsConnected)
						{
							var data = String.Format("wrap_access_token={0}&channels={1}&inreplyto={2}&body={3}",
								CloudApi.AccessToken, config.ChannelKey, status.InReplyTo, HttpUtility.UrlEncode(status.Status));

							HttpServiceRequest.Post(String.Concat(CloudApi.ApiBaseUrl, "send/statusupdate"), data, true);

						}							
						else
						{
							channel.StatusUpdatesChannel.UpdateMyStatus(status.DuckCopy<ChannelStatusUpdate>());
						}
					}
					catch (Exception ex)
					{
						ClientState.Current.ShowMessage(
							new AppMessage(
								String.Concat(
									String.Format(Strings.ErrorOccuredDuringStatusUpdate, channel.Configuration.DisplayName),
									String.Format(Strings.ServerSaid, ex.Message)))
							{
								SourceChannelId = config.ChannelId
							}, MessageType.Error);

						throw;
					}
				}

				Thread.CurrentThread.ExecuteOnUIThread(() =>
					ClientState.Current.ShowMessage(
						new AppMessage(Strings.StatusUpdatedSuccessfully)
							{
								EntityId = status.StatusId,
								EntityType = EntityType.UserStatus
							}, MessageType.Success));
			}
			finally
			{
				group.IsCompleted = true;
			}
		}
	}
}
