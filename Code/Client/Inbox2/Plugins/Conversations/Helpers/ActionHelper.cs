using System;
using System.Linq;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.ValueTypes;
using Inbox2.Framework.Plugins.SharedControls;
using Inbox2.Framework.Threading.AsyncUpdate;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.Conversations.Utils;

namespace Inbox2.Plugins.Conversations.Helpers
{
	public static class ActionHelper
	{
		public static void View(SourceAddress address)
		{
			Profile profile;

			using (VirtualMailBox.Current.Profiles.ReaderLock)
				profile = VirtualMailBox.Current.Profiles.FirstOrDefault(p => p.SourceAddress != null && p.SourceAddress.Equals(address));

			if (profile != null)
				EventBroker.Publish(AppEvents.View, profile.Person);
		}

		public static void New(SourceAddress to)
		{
			ClientState.Current.ViewController.MoveTo(
				PluginsManager.Current.GetPlugin<ConversationsPlugin>().NewItemView,
				new NewMessageDataHelper
					{
						To = to.ToList(),
						Body = MessageBodyGenerator.CreateBodyText()
					});
		}

		public static void Reply(Message source)
		{
			ClientState.Current.ViewController.MoveTo(
				PluginsManager.Current.GetPlugin<ConversationsPlugin>().NewItemView,
				new NewMessageDataHelper
					{
						SourceMessageId = source.MessageId,
						SelectedChannelId = source.SourceChannelId,
						Context = "Re: " + source.Context,
						To = source.From.ToList(),
						Body = MessageBodyGenerator.CreateBodyTextForReply(source, true)
					});
		}

		public static void ReplyAll(Message source)
		{
			ReplyAll(source, String.Empty);
		}

		public static void ReplyAll(Message source, string text)
		{
			var recipients = new SourceAddressCollection();
			recipients.AddRange(source.To);
			recipients.AddRange(source.CC);

			long channelid;

			// Remove receivers own address from list
			if (source.SourceChannelId != 0)
			{
				var channel = ChannelsManager.GetChannelObject(source.SourceChannelId);
				var sourceAddress = channel.InputChannel.GetSourceAddress();

				if (recipients.Contains(sourceAddress))
					recipients.Remove(sourceAddress);

				channelid = source.SourceChannelId;
			}
			else
			{
				var channel = ChannelsManager.GetChannelObject(source.TargetChannelId);
				var sourceAddress = channel.InputChannel.GetSourceAddress();

				if (recipients.Contains(sourceAddress))
					recipients.Remove(sourceAddress);

				channelid = source.TargetChannelId;
			}

			ClientState.Current.ViewController.MoveTo(
				PluginsManager.Current.GetPlugin<ConversationsPlugin>().NewItemView,
				new NewMessageDataHelper
					{
						SourceMessageId = source.MessageId,
						SelectedChannelId = channelid,
						Context = "Re: " + source.Context,
						To = source.From.ToList(),
						Cc = recipients,
						Body = MessageBodyGenerator.CreateBodyTextForReply(source, text),
						SuppressSignature = !String.IsNullOrEmpty(text)
					});
		}

		public static void Forward(Message source)
		{
			ClientState.Current.ViewController.MoveTo(
				PluginsManager.Current.GetPlugin<ConversationsPlugin>().NewItemView,
				new NewMessageDataHelper
					{
						SourceMessageId = source.MessageId,
						Context = "Fw: " + source.Context,
						Body = MessageBodyGenerator.CreateBodyTextForForward(source, true),
						AttachedFiles = source.Documents.Select(d => new AttachmentDataHelper(d)).ToList()
					});
		}

		public static void Delete(Message source)
		{
			var oldMessage = source.DuckCopy<Message>();

			#region Do action

			Action doAction = delegate
	          	{
	          		source.MessageFolder = Folders.Trash;
	          		source.TargetMessageState = EntityStates.Deleted;

					AsyncUpdateQueue.Enqueue(source);
	          	};

			#endregion

			#region Undo action

			Action undoAction = delegate
            	{
            		source.MessageFolder = oldMessage.MessageFolder;
					source.TargetMessageState = oldMessage.TargetMessageState;

					AsyncUpdateQueue.Enqueue(source);
            	};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}
	}
}