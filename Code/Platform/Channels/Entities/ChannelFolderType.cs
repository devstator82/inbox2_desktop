using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	public enum ChannelFolderType
	{		
		Inbox = 0,
		Archive = 1,
		SentItems = 2,
		Drafts = 3,
		Trash = 4,
		Spam = 5,
		Label = 10,
	}
}
