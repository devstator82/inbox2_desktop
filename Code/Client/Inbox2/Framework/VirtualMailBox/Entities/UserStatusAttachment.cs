using System;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows.Media;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.UI.AsyncImage;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	public class UserStatusAttachment : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private AsyncHttpImage loader;

		[PrimaryKey] public long AttachmentId { get; set; }

		[Persist] public long UserId { get; set; }

		[Persist] public string StatusKey { get; set; }

		[Persist] public string PreviewImageUrl { get; set; }

		[Persist] public string PreviewAltText { get; set; }

		[Persist] public string TargetUrl { get; set; }

		[Persist] public short MediaType { get; set; }

		[Persist] public DateTime DateCreated { get; set; }

		public ImageSource Preview
		{
			get
			{
				if (loader == null)
				{
					loader = new AsyncHttpImage(PreviewImageUrl, () => OnPropertyChanged("Preview"), () => loader = null);
				}

				return loader.AsyncSource;
			}
		}

		public bool HasPreview
		{
			get
			{
				return MediaType == StatusAttachmentTypes.Video
					|| MediaType == StatusAttachmentTypes.Photo;
			}
		}

		public UserStatusAttachment()
		{
		}

		public UserStatusAttachment(IDataReader reader) : this()
		{
			AttachmentId = reader.GetInt64(0);
			UserId = reader.GetInt64(1);
			StatusKey = reader.GetString(2);
			PreviewImageUrl = reader.GetString(3);
			PreviewAltText = reader.GetString(4);
			TargetUrl = reader.GetString(5);
			MediaType = reader.GetInt16(6);
			DateCreated = reader.ReadDateTime(7);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}
	}
}
