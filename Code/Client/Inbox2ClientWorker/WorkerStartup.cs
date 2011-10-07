using System;
using System.Net;
using System.Windows.Forms;
using HttpServer;
using Inbox2.Core;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading;
using Inbox2.Core.Threading.Handlers;
using Inbox2.Core.Threading.Repeat;
using Inbox2.Framework;
using Inbox2ClientWorker.Core.Threading.Tasks;
using Inbox2ClientWorker.HttpListener;
using Timer = System.Threading.Timer;

namespace Inbox2ClientWorker
{
	internal static class WorkerStartup
	{
		private static Timer _timer;

		public static void ClientCore()
		{
			// Increase maximum concurrent HttpWebRequest connections
			ServicePointManager.DefaultConnectionLimit = 100;

			Startup.Logging("log4net-cw.config");
			Startup.TypeConverters();
			Startup.DataSources();
			Startup.Search();
			Startup.CorePlugins();
			Startup.Plumbing();
			Startup.Commands();

			if (!CommandLine.Current.StandAlone)
				_timer = new Timer(CheckForHostProcess, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
		}

		public static void ClientTasks()
		{
			var queue = (TaskQueue)ClientState.Current.TaskQueue;

			new Repeat("ProcessQueue").Every(3).Seconds().Call(queue.ProcessingPool.Process);

			if (!CommandLine.Current.DisableStartupSendReceive)
			{
				if (CommandLine.Current.ReceiveAll)
					Tasks.ReceivePage(Int32.MaxValue);
				else
					Tasks.Receive();

				Tasks.Sync();
			}
		}

		public static void HttpRestServer()
		{
			var server = new Server();
			var module = new RestModule();

			module.Endpoints.Add(new ReceiveRestEndpoint());
			module.Endpoints.Add(new SyncRestEndpoint());
			module.Endpoints.Add(new StopRestEndpoint());

			server.Add(module);

			// use one http listener.
			server.Add(HttpServer.HttpListener.Create(IPAddress.Loopback, DebugKeys.HttpListenerPort));
			server.Start(5);
		}

		static void CheckForHostProcess(object state)
		{
			var processes = System.Diagnostics.Process.GetProcessesByName("inbox2");

			if (processes.Length == 0)
				Application.Exit();
		}
	}
}
