using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Plugins;

namespace Inbox2.Core.LauncherCommands
{
	[Export(typeof(LauncherCommand))]
	public class SendAMessageToCommand : LauncherCommand
	{
		private const string CommandText = "Send a message to {0}...";

		public SendAMessageToCommand() : base(CommandText)
		{
		}

		public override void Execute(string query)
		{		
			LauncherState.Current.Channel.SendMessage(String.Empty, String.Empty,
			                                          LauncherState.Current.SelectedAddresses);
		}

		public override void UpdateDescription(string query)
		{
			Description = String.Format(CommandText, LauncherState.Current.SelectedAddresses.ToHumanFriendlyString());
		}

		public override bool CanExecute(string query)
		{
			return LauncherState.Current.HasSelectedAddresses;
		}
	}
}