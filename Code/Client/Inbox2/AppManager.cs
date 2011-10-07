using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Microsoft.VisualBasic.ApplicationServices;

namespace Inbox2
{
	public class ApplicationManager : WindowsFormsApplicationBase
	{		
		[STAThread]
		public static void Main(string[] args)
		{
			ApplicationManager manager = new ApplicationManager();
			manager.Run(args);
		}

		private App _instance;

		public ApplicationManager()
		{
			IsSingleInstance = true;
		}

		protected override bool OnStartup(StartupEventArgs args)
		{
			//this is the first time
			_instance = new App();
			_instance.Run();

			return false;
		}

		protected override void OnStartupNextInstance(StartupNextInstanceEventArgs args)
		{
			//each other initialization
			base.OnStartupNextInstance(args);

			_instance.Activate();

			var cmdline = String.Concat("Inbox2.exe ", String.Join(" ", args.CommandLine.ToArray()));
			var commandline = CommandLine.Parse(cmdline);

			// Publish event if we have been activated by a system mailto call
			if (!String.IsNullOrEmpty(commandline.Mailto))
				EventBroker.Publish(AppEvents.New, commandline.Mailto);
		}
	}
}
