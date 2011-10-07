using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inbox2.Framework.Interop.Windows;

namespace Inbox2DefaultMailClient
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program().Run();
		}

		public void Run()
		{
			try
			{
				var handler = new DefaultMailClientHandler();

				handler.AttachMailToHandler();
				handler.AddSystemMailClientSettings();
				handler.AddUserMailClientSettings();
			}
			catch (Exception e)
			{
				MessageBox.Show("An error has occured while trying to set Inbox2 as default mail client, reason: " + e, "Error", MessageBoxButtons.OK);
			}
		}		
	}
}
