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
	public class SendThisFileToCommand : LauncherCommand
	{
		const string CommandText = "Send this file to {0}";

		public SendThisFileToCommand() : base(CommandText)
		{
		}

		public override void Execute(string query)
		{
			var filenames = LauncherState.Current.SelectedFiles.Select(f => f.FullName);

			LauncherState.Current.Channel.SendDocuments(
				filenames.ToArray(), 
				LauncherState.Current.SelectedAddresses);
		}

		public override void UpdateDescription(string query)
		{
			Description = String.Format(CommandText,
			                            LauncherState.Current.SelectedAddresses.ToHumanFriendlyString());
		}

		public override bool CanExecute(string query)
		{
			return LauncherState.Current.HasSelectedFiles 
			       && LauncherState.Current.HasSelectedAddresses;
		}
	}
}