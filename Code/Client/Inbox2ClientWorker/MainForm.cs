using System;
using System.Windows.Forms;
using Inbox2.Platform.Channels.Connections;

namespace Inbox2ClientWorker
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			Opacity = 0;
		}

		void MainForm_Load(object sender, EventArgs e)
		{
			WorkerStartup.ClientCore();
			WorkerStartup.ClientTasks();
			WorkerStartup.HttpRestServer();
		}

		void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			ConnectionPoolScavenger.Shutdown();
		}
	}
}
