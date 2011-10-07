using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Handlers.Matchers
{
	public class MessageMatcher
	{
		/// <summary>
		/// Matches the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public static Conversation Match(Message message)
		{
			return new MessageMatcher().MatchToConversation(message);
		}
	
		/// <summary>
		/// Private constructor, only allow access through static accessors.
		/// </summary>
		internal MessageMatcher()
		{
			mailbox = VirtualMailBox.Current;
		}

		private readonly VirtualMailBox mailbox;

		/// <summary>
		/// Matches the message to a conversation.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		internal Conversation MatchToConversation(Message message)
		{
			Logger.Debug("Trying to match conversation for message {0}", LogSource.MessageMatcher, message);

			Conversation conversation;

			if (MatchOnConversationId(message, out conversation) == ExecutionResult.Break)
			{
				UpdateConversation(conversation);

				return conversation;
			}

			if (MatchOnInReplyTo(message, out conversation, 1) == ExecutionResult.Break)
			{
				UpdateConversation(conversation);

				return conversation;
			}

			if (MatchOnSubject(message, out conversation) == ExecutionResult.Break)
			{
				UpdateConversation(conversation);

				return conversation;
			}

			throw new ApplicationException("Unable to match message to conversation!");
		}

		/// <summary>
		/// Matches the conversation on the conversation id field.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="conversation">The conversation.</param>
		/// <returns></returns>
		ExecutionResult MatchOnConversationId(Message message, out Conversation conversation)
		{
			conversation = null;

			if (String.IsNullOrEmpty(message.ConversationIdentifier))
				return ExecutionResult.Continue;

			Logger.Debug("MatchOnConversationId: Message {0} had ConversationIdentifier {1}", LogSource.MessageMatcher, message, message.ConversationIdentifier);

			// Retreive conversation from database
			using (mailbox.Conversations.ReaderLock)
				conversation = mailbox.Conversations.FirstOrDefault(c => c.ConversationIdentifier == message.ConversationIdentifier);

			if (conversation == null)
			{
				Logger.Debug("MatchOnConversationId: Conversation did not exist, creating a new one with ConversationIdentifier {0}", LogSource.MessageMatcher, message.ConversationIdentifier);

				// Not found, create new one
				conversation = CreateNewConversation(message);
			}
			else
			{
				UpdateMessage(message, conversation);
			}

			return ExecutionResult.Break;
		}		

		/// <summary>
		/// Matches the conversation on the in reply to field.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="conversation">The conversation.</param>
		/// <returns></returns>
		ExecutionResult MatchOnInReplyTo(Message message, out Conversation conversation, int nrtry)
		{
			conversation = null;

			if (String.IsNullOrEmpty(message.InReplyTo))
				return ExecutionResult.Continue;

			Logger.Debug("MatchOnInReplyTo: Message {0} had In-Reply-To {1}", LogSource.MessageMatcher, message, message.InReplyTo);

			Message msg;

			using (mailbox.Messages.ReaderLock)
				msg = mailbox.Messages.FirstOrDefault(m => m.MessageIdentifier == message.InReplyTo);

			if (msg != null)
			{
				// Update source message stats
				msg.TrackAction(ActionType.ReplyForward, message.SortDate);

				// Found a conversationId
				using (mailbox.Conversations.ReaderLock)
					conversation = mailbox.Conversations.FirstOrDefault(c => c.ConversationIdentifier == msg.ConversationIdentifier);

				if (conversation == null)
				{
					if (nrtry == 3)
					{
						// We are unable to find the conversation after two tries, 
						// match the message being replied to, to fix this.
						Match(msg);

						return MatchOnInReplyTo(message, out conversation, ++nrtry);
					}

					// Seems we have a conversationId but the actual conversation has not been written to disk yet
					// Sleep for 500 ms and then try again
					Thread.Sleep(500);

					return MatchOnInReplyTo(message, out conversation, ++nrtry);
				}

				Logger.Debug("MatchOnInReplyTo: Found conversation with ConversationIdentifier [{0}] for Message {1}", LogSource.MessageMatcher, conversation, message);
				
				UpdateMessage(message, conversation);

				return ExecutionResult.Break;
			}

			// Continue as our other rules might generate a hit
			return ExecutionResult.Continue;
		}

		/// <summary>
		/// Matches the conversation on the the subject field.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="conversation">The conversation.</param>
		/// <returns></returns>
		ExecutionResult MatchOnSubject(Message message, out Conversation conversation)
		{
			if (message.MessageFolder == Folders.SentItems)
			{
				Logger.Debug("MatchOnSubject: Message {0} is a sent item. Creating new conversation because it is not a reply but a new message", LogSource.MessageMatcher, message);

				conversation = CreateNewConversation(message);

				return ExecutionResult.Break;
			}

			if (message.MessageFolder == Folders.Drafts)
			{
				Logger.Debug("MatchOnSubject: Message {0} is a concept message. Creating new conversation", LogSource.MessageMatcher, message);

				conversation = CreateNewConversation(message);

				return ExecutionResult.Break;
			}

			if (String.IsNullOrEmpty(message.Context) || String.IsNullOrEmpty(message.Context.ToClearSubject()) || message.Context.Trim().Length == 0)
			{
				Logger.Debug("MatchOnSubject: Message {0} has an empty context. Creating new conversation", LogSource.MessageMatcher, message);

				conversation = CreateNewConversation(message);

				return ExecutionResult.Break;
			}

			Logger.Debug("MatchOnSubject: trying to find conversation for Message {0} with Context {1}", LogSource.MessageMatcher, message, message.Context.ToClearSubject());

			string context = message.Context.ToClearSubject();

			// Determine if the recipients in the message match with the current recipients
			List<Message> messagesWithSameContext;

			using (mailbox.Messages.ReaderLock)
				messagesWithSameContext = mailbox.Messages.Where(
					m => m.MessageId != message.MessageId && (m.Context == context || m.Context.ToClearSubject() == context))
					.ToList();

			string conversationId = null;

			var recentRelatedMessage = messagesWithSameContext.Where(m => DateTime.Compare((m.SortDate).AddDays(3), DateTime.Now) >= 0).FirstOrDefault();

			if (recentRelatedMessage != null)
			{
				conversationId = recentRelatedMessage.ConversationIdentifier;
			}
			else
			{
				if (message.MessageFolder == Folders.Inbox)
				{
					if (messagesWithSameContext.Count > 0)
					{
						// This is an incoming message
						SourceAddress from = message.From;
						bool isEmailChannel = SourceAddress.IsValidEmail(from.Address);

						//Get all channels for user
						var addresses = ChannelsManager
							.Channels							
							.Where(c => c.InputChannel != null)
							.Select(c => c.InputChannel.SourceAdress)
							.ToList();

						foreach (var contextMessage in messagesWithSameContext)
						{
							var addressCollection = new SourceAddressCollection();
							addressCollection.AddRange(contextMessage.To);
							addressCollection.AddRange(contextMessage.CC);
							addressCollection.AddRange(contextMessage.BCC);
							addressCollection.Add(contextMessage.From);

							if (addressCollection.Contains(from, new SourceAddressComparer()))
							{
								bool hasMatchedConversation = false;

								if (message.To.Count(
										c => addresses.Contains(c.Address) || addressCollection.Contains(c)) > 0)
									hasMatchedConversation = true;

								if (!hasMatchedConversation && !isEmailChannel)
								{
									if (message.To.Count(addressCollection.Contains) > 0)
										hasMatchedConversation = true;
								}

								if (hasMatchedConversation)
								{
									conversationId = contextMessage.ConversationIdentifier;
									break;
								}
							}
						}
					}
				}
			}

			if (String.IsNullOrEmpty(conversationId))
			{
				Logger.Debug("MatchOnSubject: Message {0} has no matching conversation, creating a new one", LogSource.MessageMatcher, message);

				// Nothing found, create new conversation
				conversation = CreateNewConversation(message);
			}
			else
			{
				using (mailbox.Conversations.ReaderLock)
					conversation = mailbox.Conversations.FirstOrDefault(c => c.ConversationIdentifier == conversationId);

				Logger.Debug("MatchOnSubject: Message {0} has matching conversation with ConversationId {1}", LogSource.MessageMatcher, message, conversation.ConversationId);

				UpdateMessage(message, conversation);

				Logger.Debug("MatchOnSubject: Message {0} now has ConversationId {1}", LogSource.MessageMatcher, message, message.ConversationIdentifier);
			}

			// End of line
			return ExecutionResult.Break;
		}

		/// <summary>
		/// Updates the conversation.
		/// </summary>
		/// <param name="conversation">The conversation.</param>
		void UpdateConversation(Conversation conversation)
		{
			conversation.UpdateProperty("Last");
			conversation.UpdateProperty("ProcessingHints");
			conversation.UpdateProperty("TotalMessageCount");
		}

		/// <summary>
		/// Creates a new conversation.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		Conversation CreateNewConversation(Message message)
		{
			Conversation conversation = DispatcherActivator<Conversation>.Create();

			string conversationId = message.ConversationIdentifier;

			if (String.IsNullOrEmpty(message.ConversationIdentifier))
			{
				// Create new conversation id
				conversationId = Guid.NewGuid().ToConversationId();
			}

			conversation.ConversationIdentifier = conversationId;
			conversation.Context = message.Context.ToClearSubject();

			ClientState.Current.DataService.Save(conversation);

			Logger.Debug("Conversation {0} was created successfully", LogSource.MessageMatcher, conversation);
			Logger.Debug("Message {0} now has ConversationIdentifier {1}", LogSource.MessageMatcher, message, conversationId);

			UpdateMessage(message, conversation);

			Debug.Assert(!String.IsNullOrEmpty(message.ConversationIdentifier), "Message can not have an empty ConversationIdentifier!");

			mailbox.Conversations.Add(conversation);

			return conversation;
		}

		void UpdateMessage(Message message, Conversation conversation)
		{
			message.ConversationIdentifier = conversation.ConversationIdentifier;
			ClientState.Current.DataService.Update(message);

			Thread.CurrentThread.ExecuteOnUIThread(() => conversation.Add(message));
		}
	}
}
