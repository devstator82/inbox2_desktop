using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	public class FeedItem
	{
		[PrimaryKey] public long? FeedItemId { get; set; }		
		[Persist] public string ChannelKey { get; set; }
		[Persist] public string ChannelId { get; set; }
		[Persist] public string Title { get; set; }
		[Persist] public string Url { get; set; }
		[Persist] public bool IsRead { get; set; }
		[Persist] public DateTime Published { get; set; }		
	}
}
