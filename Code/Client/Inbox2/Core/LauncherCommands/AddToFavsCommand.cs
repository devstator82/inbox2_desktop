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
	public class AddToFavsCommand : LauncherCommand
	{
		public AddToFavsCommand() : base("Add to favs")
		{
		}

		public override void Execute(string query)
		{
			LauncherState.Current.Channel.AddForLater(
				LauncherState.Current.SelectedUris[0].ToString(), NoteTypes.Url);
		}

		public override bool CanExecute(string query)
		{
			return LauncherState.Current.HasSelectedUris;
		}
	}
}