using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Framework.Text.CommandLine;

namespace Inbox2.Framework
{
	public class CommandLine
	{
		#region Singleton access

		private static CommandLine _Current;

		public static CommandLine Current
		{
			get
			{
				if (_Current == null)
					_Current = Parse(System.Environment.CommandLine);

				return _Current;
			}
		}

		public static CommandLine Parse(string commandline)
		{
			var instance = new CommandLine();

			// Check for the environment switches
			var parser = new Parser(commandline, instance);
			parser.Parse();

			return instance;
		}

		#endregion

		[CommandLineSwitch("env", "Development environment to bind to")]
		public string Environment { get; set; }

		[CommandLineSwitch("data", "Location of the Inbox2 data directory")]
		public string DataDir { get; set; }

		[CommandLineSwitch("dsr", "Disables send/receive on startup")]
		public bool DisableStartupSendReceive { get; set; }

		[CommandLineSwitch("ra", "Receives everything on all channels")]
		public bool ReceiveAll { get; set; }

		[CommandLineSwitch("sa", "Overrides client worker standalone mode")]
		public bool StandAlone { get; set; }

		[CommandLineSwitch("mlt", "Mailto startup parameter")]
		public string Mailto { get; set; }
	}
}
