using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Plugins;

namespace Inbox2.Core.LauncherCommands
{
	[Export(typeof(LauncherCommand))]
	public class PutThisFileInMyDocuments : LauncherCommand
	{
		public PutThisFileInMyDocuments() : base("Put this file in my docs")
		{
		}

		public override void Execute(string query)
		{
			var filenames = LauncherState.Current.SelectedFiles.Select(f => f.FullName);

			LauncherState.Current.Channel.AddDocuments(filenames.ToArray());
		}

		public override bool CanExecute(string query)
		{
			return LauncherState.Current.HasSelectedFiles;
		}
	}
}