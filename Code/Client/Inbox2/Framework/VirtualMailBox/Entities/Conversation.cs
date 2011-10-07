using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.VirtualMailBox.Comparers;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	public class Conversation : IEntityBase
	{
		public event PropertyChangedEventHandler PropertyChanged;		

		[PrimaryKey] public long ConversationId { get; set; }

		[Persist] public string ConversationIdentifier { get; set; }
		[Persist] public string Context { get; set; }	

		public AdvancedObservableCollection<Message> Messages { get; private set; }
		public AdvancedObservableCollection<Message> Expanded { get; private set; }

		public bool IsExpanded
		{
			get { return Expanded.Count > 0; }
		}

		public Message First
		{
			get
			{
				return Messages.FirstOrDefault(m => m.IsVisible);
			}
		}

		public Message Last
		{
			get
			{
				return Messages.LastOrDefault();
			}
		}

		public string UniqueId
		{
			get { return EntityKeyPrefixes.Conversation + ConversationId; }
		}

		public DateTime SortDate
		{
			get
			{
				// If we have messages in inbox make sure that sent items don't push the message up unnessesarily
				if (Messages.Any(m => m.MessageFolder == Folders.Inbox))
					return Messages.Where(m => m.MessageFolder == Folders.Inbox).Max(m => m.SortDate);

				return Messages.Max(m => m.SortDate);
			}
		}

		public Conversation()
		{
			Messages = new AdvancedObservableCollection<Message>();
			Expanded = new AdvancedObservableCollection<Message>();
		}

		public Conversation(IDataReader reader) : this()
		{
			ConversationId = reader.GetInt64(0);

			if (!reader.IsDBNull(1))
				ConversationIdentifier = reader.GetString(1);

			if (!reader.IsDBNull(2))
				Context = reader.GetString(2);
		}
	
		public void UpdateProperty(string propertyName)
		{
			OnPropertyChanged(propertyName);
		}

		public SourceAddressCollection GetConversationMembers()
		{
			var recipients = new SourceAddressCollection();
			var comparer = new MailAddressEqualityComparer();

			foreach (var message in Messages)
			{
				if (!recipients.Contains(message.From, comparer))
					recipients.Add(message.From);

				foreach (var address in message.To)
				{
					if (!recipients.Contains(address, comparer))
						recipients.Add(address);
				}

				foreach (var address in message.CC)
				{
					if (!recipients.Contains(address, comparer))
						recipients.Add(address);
				}

				foreach (var address in message.BCC)
				{
					if (!recipients.Contains(address, comparer))
						recipients.Add(address);
				}
			}

			return recipients;
		}

		public void Remove(Message message)
		{
			Messages.Remove(message);
		}

		public void Add(Message message)
		{
			var newList = new List<Message>(Messages) { message };

			message.Conversation = this;

			Messages.Replace(newList.OrderBy(m => m.SortDate));

			if (IsExpanded)
				Expanded.Replace(Messages.Reverse().Skip(1));

			OnPropertyChanged("First");
			OnPropertyChanged("Last");
		}

		public void Replace(IEnumerable<Message> messages)
		{
			foreach (var message in messages)
				message.Conversation = this;

			Messages.Replace(messages);

			OnPropertyChanged("First");
			OnPropertyChanged("Last");
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}

		public override string ToString()
		{
			return String.Format("[{0} {1}]", ConversationIdentifier, Context);
		}
	}
}