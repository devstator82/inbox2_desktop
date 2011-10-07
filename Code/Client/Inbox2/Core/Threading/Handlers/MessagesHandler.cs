using System;
using Inbox2.Core.Threading.Handlers.Matchers;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.VirtualMailBox.View;

namespace Inbox2.Core.Threading.Handlers
{
	public static class MessagesHandler
	{
		public static void Init()
		{
			EventBroker.Subscribe<Message>(AppEvents.MessageStored, MessageStored);
		}

		public static void MessageStored(Message message)
		{
			var mailbox = VirtualMailBox.Current;

			// Match thread
			MessageMatcher.Match(message);

			mailbox.Messages.Add(message);

			ViewFilter.Current.UpdateCurrentViewAsync();

			// Add to search index
			ClientState.Current.Search.Store(message);
		}
	}
}