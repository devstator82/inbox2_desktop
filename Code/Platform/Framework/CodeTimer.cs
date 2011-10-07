using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;
using Logger=Inbox2.Platform.Logging.Logger;

namespace Inbox2.Platform.Framework
{
    public class CodeTimer : IDisposable
    {
		protected string methodName;
		protected bool isEnabled;
        protected bool executeWhen = true;
        private readonly Stopwatch stopwatch;

    	public string MethodName
    	{
			get { return methodName; }
    	}

        public long Elapsed
        {
            get { return stopwatch.ElapsedMilliseconds; }
        }

        public CodeTimer()
            : this(String.Empty)
        {
        }

        public CodeTimer(string methodName)
        {
            this.isEnabled = "/Settings/Diagnostics/CodeTimerEnabled".AsKey(false);
            this.methodName = methodName;

            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public CodeTimer(string methodName, bool executeWhen) : this(methodName)
        {
            this.executeWhen = executeWhen;
        }

        public void Dispose()
        {
            stopwatch.Stop();
            
           	WriteToLog();
        }

		public virtual void WriteToLog()
		{
			if (isEnabled && executeWhen)
			{
				// We use error logging because this is ususally enabled on production servers
				Logger.Debug("CodeTimer: [{0}] [{1}] ms", LogSource.Performance, methodName, Elapsed);
			}
		}		
    }
}
