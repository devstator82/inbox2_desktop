using System;
using System.Collections.Generic;
using Inbox2.Platform.Framework.CloudApi.Logging;

namespace Inbox2.Framework.Stats
{
	public static class ClientStats
	{
	    private static readonly List<TraceInfo> _traces;
		private static readonly object _synclock;

        public static TraceInfo CurrentTrace { get; private set; }

        static ClientStats()
        {
            _traces = new List<TraceInfo>();
            _synclock = new object();

            CreateNewTrace();

			LogEvent("Application Startup");
        }

        public static void CreateNewTrace()
        {
            lock (_synclock)
            {
                CurrentTrace = new TraceInfo();

                _traces.Add(CurrentTrace);
            }
        }

		public static void LogEvent(string eventname)
		{
			lock (_synclock)
                CurrentTrace.Logs.Add(new LogInfo(eventname, 0));
		}

		public static void LogEventWithTime(string eventname, double time)
		{
			lock (_synclock)
                CurrentTrace.Logs.Add(new LogInfo(eventname, time));
		}

		public static void LogEventWithSegment(string eventname, string segment)
		{
			lock (_synclock)
				CurrentTrace.Logs.Add(new LogInfo(eventname, segment));
		}

        public static List<TraceInfo> Flush()
		{
			lock (_synclock)
			{
				var temp = new List<TraceInfo>(_traces);

                _traces.Clear();
			    
                CreateNewTrace();

				return temp;
			}
		}

        public static void ReEnqueue(List<TraceInfo> list)
		{
			lock (_synclock)
			{
				_traces.InsertRange(0, list);
			}
		}               
	}
}
