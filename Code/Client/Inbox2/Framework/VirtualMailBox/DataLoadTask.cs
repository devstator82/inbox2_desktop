using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework.VirtualMailBox
{
	internal class DataLoadTask
	{
		private readonly Action action;
		private readonly ManualResetEvent signal;

		public DataLoadTask(Action action)
		{
			this.action = action;
			this.signal = new ManualResetEvent(false);
		}

		public ManualResetEvent ExecuteAsync()
		{
			ThreadPool.QueueUserWorkItem(Execute);

			return signal;
		}

		void Execute(object state)
		{
			try
			{
				using (new JoeCulture())
					action();
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while loading data. Exception = {0}", LogSource.Startup, ex);				
			}	
			finally
			{
				signal.Set();
			}
		}
	}
}
