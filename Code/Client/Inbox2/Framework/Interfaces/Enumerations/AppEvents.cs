using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces.Enumerations
{
	public static class AppEvents
	{
		public const string New = "New";
		public const string View = "View";
		public const string Save = "Save";

		public const string RequestReceive = "RequestSend";
		public const string RequestSend = "RequestReceive";
		public const string RequestSync = "RequestSync";
		public const string RequestCommands = "RequestCommands";	

		public const string RequestNewSearch = "RequestNewSearch";
		public const string RequestStatusUpdate = "RequestStatusUpdate";
		public const string RequestAddLabels = "RequestAddLabels";
        public const string RequestFirstImportant = "RequestFirstImportant";
        public const string RequestFocus = "RequestFocus";
        public const string RequestOpenToolbarItem = "RebuildOpenToolbarItem";
		public const string RequestDockChannel = "RequestDockChannel";

		public const string MessageStored = "MessageStored";
		public const string DocumentReceived = "DocumentReceived";
		public const string ContactReceived = "ContactReceived";
		public const string StatusUpdateReceived = "StatusUpdateReceived";
		public const string LabelCreated = "LabelCreated";

		public const string ReceiveFinished = "ReceiveFinished";
		public const string SyncFinished = "SyncFinished";
		public const string SyncStatusUpdatesFinished = "SyncStatusUpdatesFinished";
		public const string SyncContactsFinished = "SyncContactsFinished";
		public const string ReceiveMessagesFinished = "ReceiveMessagesFinished";		
		public const string SendMessageFinished = "SendMessageFinished";
		public const string UpdateCheckFinished = "UpdateCheckFinished";

		// Message has been stored in receive flow
		public const string ConversationUpdated = "ConversationUpdated";		
		public const string MessageUpdated = "MessageUpdated";
		public const string MessageLabelsUpdated = "MessageLabelsUpdated";

		public const string RebuildToolbar = "RebuildToolbar";
		public const string RebuildOverview = "RebuildOverview";

		public const string TabChanged = "TabChanged";
		public const string ShuttingDown = "ShuttingDown";		
	}
}
