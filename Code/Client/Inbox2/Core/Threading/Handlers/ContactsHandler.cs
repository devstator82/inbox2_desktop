using System;
using Inbox2.Core.Threading.Handlers.Matchers;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Core.Threading.Handlers
{
	public static class ContactsHandler
	{
		public static void Init()
		{
			EventBroker.Subscribe<ChannelContact>(AppEvents.ContactReceived, ContactReceived);
			EventBroker.Subscribe<Message>(AppEvents.MessageStored, MessageStored);
		}

		public static void ContactReceived(ChannelContact contact)
		{
			new ContactMatcher(contact).Execute();
		}

		public static void MessageStored(Message message)
		{
			new ProfileMatcher(message).Execute();
		}
	}
}
