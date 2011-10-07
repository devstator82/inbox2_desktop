using System;
using System.Net;

namespace LumiSoft.Net
{
	/// <summary>
	/// Provides data for the SessionLog event.
	/// </summary>
	public class Log_EventArgs
	{
		private SocketLogger m_pLoggger     = null;
		private bool         m_FirstLogPart = true;
        private bool         m_LastLogPart  = false;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="logger">Socket logger.</param>
        /// <param name="firstLogPart">Specifies if first log part of multipart log.</param>
		/// <param name="lastLogPart">Specifies if last log part (logging ended).</param>
		public Log_EventArgs(SocketLogger logger,bool firstLogPart,bool lastLogPart)
		{	
			m_pLoggger     = logger;
            m_FirstLogPart = firstLogPart;
            m_LastLogPart  = lastLogPart;
		}


		#region Properties Implementation

		/// <summary>
		/// Gets log text.
		/// </summary>
		public string LogText
		{
			get{ return SocketLogger.LogEntriesToString(m_pLoggger,m_FirstLogPart,m_LastLogPart); }
		}

		/// <summary>
		/// Gets logger.
		/// </summary>
		public SocketLogger Logger
		{
			get{ return m_pLoggger; }
		}

		#endregion

	}
}
