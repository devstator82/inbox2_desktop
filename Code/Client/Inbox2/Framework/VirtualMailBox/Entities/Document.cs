using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.UI.AsyncImage;
using Inbox2.Framework.VirtualMailBox.Mappers;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	[ContentMapper(typeof(DocumentContentMapper))]
	public class Document : IEntityBase
	{
		public static readonly List<string> _ImageExtensions = new List<string> { ".bmp", ".png", ".gif", ".jpg", ".jpeg", ".avi", ".mpg", ".wmv" };

		public event PropertyChangedEventHandler PropertyChanged;

		private AsyncThumbnailImage loader;

		[IndexAndStore]
		[PrimaryKey] public long? DocumentId { get; set; }

		[IndexTokenizeAndStore]
		[Persist] public string Filename { get; set; }		

		[Persist] public long SourceChannelId { get; set; }
		[Persist] public long TargetChannelId { get; set; }

		[Persist] public ContentType ContentType { get; set; }

		[Persist] public string ContentId { get; set; }

		[IndexTokenizeAndStore]
		public Stream ContentStream { get; set; }	

		[Persist] public string DocumentKey { get; set; }
		[Persist] public string StreamName { get; set; }		
		[Persist] public string Crc32 { get; set; }
		[Persist] public long Size { get; set; }

		[Persist] public bool IsRead { get; set; }

		[Persist] public int DocumentFolder { get; set; }

		[Persist] public string Labels { get; set; }
		[Persist] public bool ContentSynced { get; set; }

		[Persist] public DateTime? DateReceived { get; set; }
		[Persist] public DateTime? DateSent { get; set; }

		[Persist] public DateTime? DateCreated { get; set; }
		[Persist] public DateTime? DateModified { get; set; }		

		public AdvancedObservableCollection<DocumentVersion> Versions { get; private set; }

		public AdvancedObservableCollection<Label> LabelsList { get; private set; }

	    public string FilenameExtension
	    {
	        get
	        {
	            return Path.GetExtension(Filename).ToLower();
	        }
	    }

        public string FilenameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Filename);
            }
        }

        public ImageSource PreviewImageSource
		{
			get
			{
				if (loader == null)
				{
					string filename = ClientState.Current.Storage.ResolvePhysicalFilename(".", StreamName);
					loader = new AsyncThumbnailImage(filename, () => OnPropertyChanged("PreviewImageSource"), () => loader = null);
				}

				return loader.AsyncSource;
			}
		}

		public Message Message { get; set; }

		public string UniqueId
		{
			get { return EntityKeyPrefixes.Document + DocumentId; }
		}

		public DateTime SortDate
		{
			get { return DateReceived ?? DateSent.Value; }
		}

		public bool IsImage
		{
			get
			{
				return IsFilenameImage(Filename);
			}
		}
	
		public static bool IsFilenameImage(string filename)
		{
			return _ImageExtensions.Contains(Path.GetExtension(filename).ToLower());
		}

		public Document()
		{
			DocumentKey = Guid.NewGuid().GetHash();
			DateCreated = DateTime.Now;
			Versions = new AdvancedObservableCollection<DocumentVersion>();
			LabelsList = new AdvancedObservableCollection<Label>();
			ContentSynced = true;
		}

		public Document(IDataReader reader) : this()
		{
			DocumentId = reader.GetInt64(0);

			if (!reader.IsDBNull(1))
				Filename = reader.GetString(1);

			if (!reader.IsDBNull(2))
				SourceChannelId = reader.GetInt64(2);

			if (!reader.IsDBNull(3))
				TargetChannelId = reader.GetInt64(3);

			if (!reader.IsDBNull(4))
				ContentType = (ContentType) Enum.Parse(typeof (ContentType), reader.GetString(4));

			if (!reader.IsDBNull(5))
				ContentId = reader.GetString(5);

			if (!reader.IsDBNull(6))
				DocumentKey = reader.GetString(6);

			if (!reader.IsDBNull(7))
				StreamName = reader.GetString(7);

			if (!reader.IsDBNull(8))
				Crc32 = reader.GetString(8);

			if (!reader.IsDBNull(9))
				Size = reader.GetInt32(9);

			if (!reader.IsDBNull(10))
				IsRead = reader.ReadBoolean(10);

			if (!reader.IsDBNull(11))
				DocumentFolder = reader.GetInt32(11);

			if (!reader.IsDBNull(12))
				Labels = reader.GetString(12);

			if (!reader.IsDBNull(13))
				DateReceived = reader.ReadDateTimeOrNull(13);

			if (!reader.IsDBNull(14))
				DateSent = reader.ReadDateTimeOrNull(14);
			
			if (!reader.IsDBNull(15))
				DateCreated = reader.ReadDateTimeOrNull(15);

			if (!reader.IsDBNull(16))
				DateModified = reader.ReadDateTimeOrNull(16);
		}

		public void MarkRead()
		{
			IsRead = true;

			UpdateProperty("IsRead");
		}

		public void MarkUnread()
		{
			IsRead = false;

			UpdateProperty("IsRead");
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

		public MediaType GetMediaType()
		{
			string ext = Path.GetExtension(Filename);

			switch (ext.ToLower())
			{
				case ".gif":
				case ".jpg":
				case ".png":
				case ".bmp":
					return MediaType.Image;
				case ".mp3":
				case ".wav":
				case ".mid":
					return MediaType.Audio;
				case ".mpg":
				case ".avi":
				case ".wmv":
					return MediaType.Video;
				default:
					return MediaType.Other;
			}
		}

		public override string ToString()
		{
			return String.Format("[{0}]", Filename);
		}
		
		public void DeleteFrom(Message message)
		{
			var mailbox = VirtualMailBox.Current;

			// Get all versions associated with the message we want to detach
			// this document from.
			var versionsForMessage = Versions
				.Where(v => v.MessageId == message.MessageId.Value)
				.ToList();

			// Remove all versions for this message
			foreach (var version in versionsForMessage)
			{
				Versions.Remove(version);

				ClientState.Current.DataService.Delete(version);
			}			
			
			if (Versions.Count == 0)
			{
				// No versions left, delete document and stream from disk
				ClientState.Current.Storage.Delete(".", StreamName);

				// todo remove stream from search-index

				using (mailbox.Documents.WriterLock)
					mailbox.Documents.Remove(this);

				ClientState.Current.DataService.Delete(this);
			}

			message.Documents.Remove(this);
		}
	}
}


