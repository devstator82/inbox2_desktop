using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Core.LauncherCommands
{
	[Export(typeof(LauncherCommand))]
	public class MakeANoteCommand : LauncherCommand
	{
		public MakeANoteCommand() : base("Make a note")
		{
		}

		public override void Execute(string query)
		{
			LauncherState.Current.Channel.AddForLater(query, NoteTypes.Note);
		}

		public override bool CanExecute(string query)
		{
			return !String.IsNullOrEmpty(query);
		}
	}
}