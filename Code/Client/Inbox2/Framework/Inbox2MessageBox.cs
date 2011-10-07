using System;
using System.Threading;

namespace Inbox2.Framework
{
	[Serializable]
	public enum Inbox2MessageBoxButton
	{
		None,
		OK,
		OKCancel,
		YesNoCancel,
		YesNo,
		Conversation
	}

	[Serializable]
	public enum Inbox2MessageBoxResult
	{
		None,
		OK,
		Cancel,
		Yes,
		No,
		Conversation,
		Message
	}

	[Serializable]
	public class Inbox2MessageBoxResultWrapper
	{
		public Inbox2MessageBoxResult Result { get; private set; }

		public bool DoNotAskAgainResult { get; private set; }		

		public Inbox2MessageBoxResultWrapper(Inbox2MessageBoxResult result, bool doNotAskAgainResult)
		{
			Result = result;
			DoNotAskAgainResult = doNotAskAgainResult;
		}
	}

	[Serializable]
	public class MessageBoxEventArgs : EventArgs
	{
		public string Message { get; private set; }

		public Inbox2MessageBoxButton Buttons { get; private set; }

		public Inbox2MessageBoxResult Result { get; set; }

		public string DoNotAskAgainMessage { get; private set; }

		public bool DoNotAskAgain { get; private set; }

		public bool DoNotAskAgainChecked { get; private set; }

		public bool DoNotAskAgainResult { get; set; }

		public bool DisableActions { get; set; }

		public MessageBoxEventArgs(string message, Inbox2MessageBoxButton buttons, string doNotAskAgainMessage, bool doNotAskAgainChecked)
		{
			Message = message;
			Buttons = buttons;

			if (!String.IsNullOrEmpty(doNotAskAgainMessage))
			{
				DoNotAskAgain = true;
				DoNotAskAgainChecked = doNotAskAgainChecked;
				DoNotAskAgainMessage = doNotAskAgainMessage;
			}
		}
	}

	public static class Inbox2MessageBox
	{
		public static event EventHandler<MessageBoxEventArgs> ShowMessageBox;

		public static Inbox2MessageBoxResultWrapper Show(string messageBoxText, Inbox2MessageBoxButton button)
		{
			return Show(messageBoxText, button, null);
		}

		public static Inbox2MessageBoxResultWrapper Show(string messageBoxText, Inbox2MessageBoxButton button, string doNotAskAgainMessage)
		{
			return Show(messageBoxText, button, null, false);
		}
		
		public static Inbox2MessageBoxResultWrapper Show(string messageBoxText, Inbox2MessageBoxButton button, string doNotAskAgainMessage, bool doNotAskAgainChecked)
		{
			Inbox2MessageBoxResultWrapper wrapper = null;

			Thread.CurrentThread.ExecuteOnUIThread(delegate
			{
				var args = new MessageBoxEventArgs(messageBoxText, button, doNotAskAgainMessage, doNotAskAgainChecked);

				ShowMessageBox(args, args);

				wrapper = new Inbox2MessageBoxResultWrapper(args.Result, args.DoNotAskAgainResult);
			});

			return wrapper;
		}
	}
}
