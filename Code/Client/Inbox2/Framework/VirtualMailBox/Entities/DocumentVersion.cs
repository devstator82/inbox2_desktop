using System;
using System.Data;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	public class DocumentVersion
	{
		[PrimaryKey] public long? VersionId { get; set; }

		[Persist] public long DocumentId { get; set; }
		[Persist] public long? MessageId { get; set; }

		[Persist] public string Filename { get; set; }

		[Persist] public long SourceChannelId { get; set; }
		[Persist] public long TargetChannelId { get; set; }

		[Persist] public SourceAddress From { get; set; }
		[Persist] public SourceAddressCollection To { get; set; }

		[Persist] public string StreamName { get; set; }		
		[Persist] public string Crc32 { get; set; }
		[Persist] public long Size { get; set; }

		[Persist] public DateTime? DateReceived { get; set; }
		[Persist] public DateTime? DateSent { get; set; }

		[Persist] public DateTime? DateCreated { get; set; }
		[Persist] public DateTime? DateModified { get; set; }

		public DocumentVersion()
		{
		}

		public DocumentVersion(IDataReader reader) : this()
		{
			VersionId = reader.GetInt64(0);
			DocumentId = reader.GetInt64(1);
			MessageId = reader.ReadInt64OrNull(2);
			
			Filename = reader.GetString(3);
			
			SourceChannelId = reader.GetInt64(4);
			TargetChannelId = reader.GetInt64(5);

			From = new SourceAddress(reader.GetString(6));
			To = new SourceAddressCollection(reader.GetString(7));

			StreamName = reader.GetString(8);
			Crc32 = reader.GetString(9);
			Size = reader.GetInt32(10);

			DateReceived = reader.ReadDateTimeOrNull(11);
			DateSent = reader.ReadDateTimeOrNull(12);

			DateCreated = reader.ReadDateTimeOrNull(13);
			DateModified = reader.ReadDateTimeOrNull(14);
		}
	}
}


