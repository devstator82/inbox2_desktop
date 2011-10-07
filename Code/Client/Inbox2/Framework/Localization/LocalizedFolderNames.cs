using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.Localization
{
	public static class LocalizedFolderNames
	{
		public static string GetName(int folder)
		{
			switch (folder)
			{
				case Folders.Inbox:
					return Strings.Inbox;
				case Folders.SentItems:
					return Strings.SentItems;
				case Folders.Drafts:
					return Strings.Drafts;
				case Folders.Spam:
					return Strings.Spam;
				case Folders.Archive:
					return Strings.Archive;
				default:
					return Strings.Unknown;
			}
		}
	}
}
