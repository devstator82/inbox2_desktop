using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Framework.Web;

namespace Inbox2.Core.Threading.Tasks.Application
{
	public class RemoveChannelTask : BackgroundTask
	{
		private readonly ChannelConfiguration configuration;
		private readonly VirtualMailBox mailbox;
		private readonly bool notify;

		public RemoveChannelTask(ChannelConfiguration configuration) : this(configuration, true)
		{
		}

		public RemoveChannelTask(ChannelConfiguration configuration, bool notify)
		{
			this.configuration = configuration;
			this.mailbox = VirtualMailBox.Current;
			this.notify = notify;
		}

		protected override void ExecuteCore()
		{
			var config = ClientState.Current.DataService.SelectByKey<ChannelConfig>(configuration.ChannelId);

			if (notify && config.ChannelConnection == ChannelConnection.Connected)
			{
				// Remove configuration from server
				HttpServiceRequest.Post(CloudApi.ApiBaseUrl + "account/removechannel", 
					String.Format("wrap_access_token={0}&key={1}", CloudApi.AccessToken, config.ChannelKey), true);
			}

			ClientState.Current.DataService.ExecuteNonQuery("DELETE FROM UserStatus WHERE SourceChannelId=" + config.ChannelConfigId);
			ClientState.Current.DataService.ExecuteNonQuery("DELETE FROM Profiles WHERE SourceChannelId=" + config.ChannelConfigId);
			ClientState.Current.DataService.ExecuteNonQuery("DELETE FROM Persons WHERE SourceChannelId=" + config.ChannelConfigId);
			ClientState.Current.DataService.ExecuteNonQuery("DELETE FROM Conversations WHERE ConversationIdentifier IN (SELECT ConversationIdentifier FROM Messages WHERE Messages.SourceChannelId=" + config.ChannelConfigId + " OR TargetChannelId=" + config.ChannelConfigId + ")");
			ClientState.Current.DataService.ExecuteNonQuery("DELETE FROM Messages WHERE SourceChannelId=" + config.ChannelConfigId + " OR TargetChannelId=" + config.ChannelConfigId);
			ClientState.Current.DataService.ExecuteNonQuery("DELETE FROM Documents WHERE SourceChannelId=" + config.ChannelConfigId);
			ClientState.Current.DataService.ExecuteNonQuery("DELETE FROM DocumentVersions WHERE SourceChannelId=" + config.ChannelConfigId);
			ClientState.Current.DataService.Delete(config);

			mailbox.StatusUpdates.RemoveAll(u => u.SourceChannelId == config.ChannelConfigId || u.TargetChannelId == config.ChannelConfigId.ToString());
			mailbox.Profiles.RemoveAll(p => p.SourceChannelId == config.ChannelConfigId);
			mailbox.Persons.RemoveAll(p => p.SourceChannelId == config.ChannelConfigId);
			// conversations are not deleted because well we don't care about then, will be gone on the next restart anyway
			mailbox.Messages.RemoveAll(m => m.SourceChannelId == config.ChannelConfigId || m.TargetChannelId == config.ChannelConfigId);
			mailbox.Documents.RemoveAll(d => d.SourceChannelId == config.ChannelConfigId || d.TargetChannelId == config.ChannelConfigId);
			mailbox.DocumentVersions.RemoveAll(d => d.SourceChannelId == config.ChannelConfigId || d.TargetChannelId == config.ChannelConfigId);

			// Delete channel from the ChannelsManager
			ChannelsManager.Remove(ChannelsManager.GetChannelObject(config.ChannelConfigId.Value));

			EventBroker.Publish(AppEvents.RebuildToolbar);
		}
	}
}
