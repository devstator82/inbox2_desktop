using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	public static class Folders
	{
		public const int None = 0;
		public const int Inbox = 10;
		public const int SentItems = 20;
		public const int Drafts = 30;
		public const int Spam = 40;
		public const int Trash = 50;
		public const int Archive = 100;

        public const int UnreadItems = 60; // Virtual folder
        public const int Starred = 70; // Virtual folder
	}
}