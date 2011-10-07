using System;
using System.ComponentModel;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	public class Note : IEntityBase, IEditableObject
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[IndexAndStore]
		[PrimaryKey] public long? InternalNoteId { get; set; }
		
		[Persist] public long? InternalMessageId { get; set; }

		[Persist] public string FriendlyRowKey { get; set; }

		[Persist] public string ConversationId { get; set; }
		[Persist] public long SourceChannelId { get; set; }
		[Persist] public long TargetChannelId { get; set; }
		[Persist] public string Locations { get; set; }
		
		[Persist]
		[IndexTokenizeAndStore]
		public string Context { get; set; }

		[Persist]
		[IndexTokenizeAndStore]
		public string Content { get; set; }
		
		[Persist] public string SearchVector { get; set; }

		[Persist] public EntityStates NoteState { get; set; }
		[Persist] public ProcessingHints ProcessingHints { get; set; }
		
		[Persist] public int Version { get; set; }
		[Persist] public int NoteFolder { get; set; }		
		[Persist] public int ContentType { get; set; }
		[Persist] public int DownloadState { get; set; }
		[Persist] public int SendState { get; set; }		

		[Persist] public DateTime DateExpires { get; set; }
		[Persist] public DateTime DateCreated { get; set; }
		[Persist] public DateTime DateModified { get; set; }

		public string UniqueId
		{
			get { return EntityKeyPrefixes.Note + InternalNoteId; }
		}

		public Note()
		{
			ProcessingHints = ProcessingHints.None;
			NoteState = EntityStates.None;
			DateCreated = DateTime.Now;
		}

		public void Set(EntityStates options)
		{
			NoteState |= options;
		}

		public void Unset(EntityStates options)
		{
			NoteState &= ~options;
		}

		public bool IsSet(EntityStates option)
		{
			return (NoteState & option) == option;
		}

		public void UpdateProperty(string propertyName)
		{
			OnPropertyChanged(propertyName);
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
			return String.Format("[{0}]", Context);
		}

		public void BeginEdit()
		{
			
		}

		public void EndEdit()
		{
			
		}

		public void CancelEdit()
		{
			
		}
	}
}


