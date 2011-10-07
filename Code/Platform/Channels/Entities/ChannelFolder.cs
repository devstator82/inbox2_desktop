using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Platform.Channels.Entities
{
	public class ChannelFolder
	{
		public string FolderId { get; private set; }

		public string Name { get; private set; }

		public ChannelFolderType FolderType { get; private set; }

		public ChannelFolder(string folderId, string name, ChannelFolderType folderType)
		{
			FolderId = folderId;
			Name = name;
			FolderType = folderType;
		}

		public int ToStorageFolder()
		{
			switch (FolderType)
			{
				case ChannelFolderType.Inbox:
					return Folders.Inbox;
				case ChannelFolderType.SentItems:
					return Folders.SentItems;
				case ChannelFolderType.Drafts:
					return Folders.Drafts;
				case ChannelFolderType.Trash:
					return Folders.Trash;
				case ChannelFolderType.Spam:
					return Folders.Spam;
				case ChannelFolderType.Archive:
					return Folders.Archive;
				case ChannelFolderType.Label:
					return Folders.Inbox;
			}

			throw new NotSupportedException(String.Format("Unknown foldertype {0}", FolderType));
		}
	}
}
