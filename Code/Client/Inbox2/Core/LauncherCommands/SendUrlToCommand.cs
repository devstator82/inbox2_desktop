using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Core.LauncherCommands
{
	[Export(typeof(LauncherCommand))]
	public class SendUrlToCommand : LauncherCommand
	{
		private const string CommandText = "Send url to {0}";

		public SendUrlToCommand() : base(CommandText)
		{
		}

		public override void UpdateDescription(string query)
		{
			Description = String.Format(CommandText,
			                            LauncherState.Current.SelectedAddresses.ToHumanFriendlyString());
		}

		public override void Execute(string query)
		{
			LauncherState.Current.Channel.SendForLater(
				LauncherState.Current.SelectedUris[0].ToString(), 
				NoteTypes.Url, 
				LauncherState.Current.SelectedAddresses);
		}

		public override bool CanExecute(string query)
		{
			return LauncherState.Current.HasSelectedUris 
			       && LauncherState.Current.HasSelectedAddresses;
		}
	}
}