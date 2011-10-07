using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Inbox2.Framework.UI
{
	public static class Commands
	{
		public static RoutedCommand Refresh = new RoutedCommand();
		
		public static RoutedCommand ViewDocuments = new RoutedCommand();
		public static RoutedCommand ViewContacts = new RoutedCommand();
				
		public static RoutedCommand OpenDocument = new RoutedCommand();
		public static RoutedCommand SaveDocument = new RoutedCommand();
		public static RoutedCommand DeleteDocument = new RoutedCommand();

		public static RoutedCommand View = new RoutedCommand();
		public static RoutedCommand New = new RoutedCommand();
		
		public static RoutedCommand Reply = new RoutedCommand();
		public static RoutedCommand ReplyAll = new RoutedCommand();
		public static RoutedCommand Forward = new RoutedCommand();
		
		public static RoutedCommand Delete = new RoutedCommand();
		public static RoutedCommand Star = new RoutedCommand();
		
		public static RoutedCommand MarkRead = new RoutedCommand();
		public static RoutedCommand MarkUnread = new RoutedCommand();

		public static RoutedCommand Archive = new RoutedCommand();
		public static RoutedCommand Unarchive = new RoutedCommand();

		public static RoutedCommand Send = new RoutedCommand();
		public static RoutedCommand Search = new RoutedCommand();
		public static RoutedCommand SaveDraft = new RoutedCommand();
		public static RoutedCommand Cancel = new RoutedCommand();
		public static RoutedCommand Navigate = new RoutedCommand();
		public static RoutedCommand Maximize = new RoutedCommand();

		public static RoutedCommand EmptyTrash = new RoutedCommand();

		public static RoutedCommand ShortenUrls = new RoutedCommand();	

		public static RoutedCommand Login = new RoutedCommand();
		public static RoutedCommand Logout = new RoutedCommand();

        public static RoutedCommand Next = new RoutedCommand();
        public static RoutedCommand Previous = new RoutedCommand();
	}
}
