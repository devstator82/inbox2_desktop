using System;
using System.ComponentModel.Composition;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Framework
{
	public enum MessageType
	{
		Success,
		Error,
	}

	public class ClientState
	{
		public static bool StartupSuccess { get; set; }

		#region Singleton pattern implementation

		private static ClientState _Current;

		public static ClientState Current
		{
			get
			{
				if (_Current == null)
					_Current = new ClientState();

				return _Current;
			}
		}

		private ClientState()
		{
			Messages = new AppMessages();
			SyncState = new SyncState();
		}

		#endregion

		public IFileStorage Storage { get; set; }

		public ISearch Search { get; set; }

		public ITaskQueue TaskQueue { get; set; }

		public IClientContext Context { get; set; }

		public IUndoManager UndoManager { get; set; }

		public IDataService DataService { get; set; }

		public IViewController ViewController { get; set; }

		public IClientAutomationService AutomationService { get; set; }

		public AppMessages Messages { get; private set; }

		public SyncState SyncState { get; private set; }

		public void ShowMessage(AppMessage message, MessageType messageType)
		{
			switch (messageType)
			{
				case MessageType.Success:
					Messages.Success = message;
					break;

				case MessageType.Error:
					Messages.Errors.Add(message);
					break;
			}
		}
	}
}