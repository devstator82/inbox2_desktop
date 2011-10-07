using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace Inbox2ClientWorker
{
	public class AppManager : WindowsFormsApplicationBase
	{
		[STAThread]
		public static void Main()
		{			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var manager = new AppManager();
			manager.Run(Environment.GetCommandLineArgs());
		}

		public AppManager()
		{
			IsSingleInstance = true;
		}

		protected override void OnCreateMainForm()
		{
			MainForm = new MainForm();
		}
	}
}
