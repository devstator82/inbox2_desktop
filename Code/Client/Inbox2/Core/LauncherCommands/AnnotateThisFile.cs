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
	public class AnnotateThisFile : LauncherCommand
	{
		public AnnotateThisFile() : base("Annotate this file")
		{
		}

		public override void Execute(string query)
		{
			
		}

		public override bool CanExecute(string query)
		{
			return LauncherState.Current.HasSelectedFiles;
		}
	}
}