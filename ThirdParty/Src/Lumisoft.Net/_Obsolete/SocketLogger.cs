using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using LumiSoft.Net;

namespace LumiSoft.Net
{
	/// <summary>
	/// Socket logger.
	/// </summary>
	public class SocketLogger
	{
		private Socket               m_pSocket         = null;
		private string               m_SessionID       = "";
        private string               m_UserName        = "";
        private IPEndPoint           m_pLoaclEndPoint  = null;
        private IPEndPoint           m_pRemoteEndPoint = null;
		private LogEventHandler      m_pLogHandler     = null;
		private List<SocketLogEntry> m_pEntries        = null;
        private bool                 m_FirstLogPart    = true;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="logHandler"></param>
		public SocketLogger(Socket socket,LogEventHandler logHandler)
		{	
			m_pSocket         = socket;
			m_pLogHandler     = logHandler;
            		
			m_pEntries = new List<SocketLogEntry>();
        }


        #region static method LogEntriesToString

        /// <summary>
        /// Converts log entries to string.
        /// </summary>
        /// <param name="logger">Socket logger.</param>
        /// <param name="firstLogPart">Specifies if first log part of multipart log.</param>
        /// <param name="lastLogPart">Specifies if last log part (logging ended).</param>
        /// <returns></returns>
        public static string LogEntriesToString(SocketLogger logger,bool firstLogPart,bool lastLogPart)
        {
            string logText = "//----- Sys: 'Session:'" + logger.SessionID + " added " + DateTime.Now + "\r\n";
            if(!firstLogPart){
                logText = "//----- Sys: 'Session:'" + logger.SessionID + " partial log continues " + DateTime.Now + "\r\n";
            }

            foreach(SocketLogEntry entry in logger.LogEntries){
                if(entry.Type == SocketLogEntryType.ReadFromRemoteEP){
                    logText += CreateEntry(logger,entry.Text,">>>");
                }
                else if(entry.Type == SocketLogEntryType.SendToRemoteEP){
                    logText += CreateEntry(logger,entry.Text,"<<<");
                }
                else{
                    logText += CreateEntry(logger,entry.Text,"---");
                }
            }

            if(lastLogPart){
                logText += "//----- Sys: 'Session:'" + logger.SessionID + " removed " + DateTime.Now + "\r\n";
            }
            else{
                logText += "//----- Sys: 'Session:'" + logger.SessionID + " partial log " + DateTime.Now + "\r\n";
            }

            return logText;
        }

        #endregion


        #region static method CreateEntry

        private static string CreateEntry(SocketLogger logger,string text,string prefix)
		{
			string retVal = "";

			if(text.EndsWith("\r\n")){
				text = text.Substring(0,text.Length - 2);
			}

			string remIP = "xxx.xxx.xxx.xxx";
			try{
				if(logger.RemoteEndPoint != null){
					remIP = ((IPEndPoint)logger.RemoteEndPoint).Address.ToString();
				}
			}
			catch{
			}

			string[] lines = text.Replace("\r\n","\n").Split('\n');
			foreach(string line in lines){
				retVal += "SessionID: " + logger.SessionID + "  RemIP: " + remIP + "  " + prefix + "  '" + line + "'\r\n";
			}

			return retVal;
		}

		#endregion


		#region method AddReadEntry

		/// <summary>
		/// Adds data read(from remoteEndpoint) entry.
		/// </summary>
		/// <param name="text">Log text.</param>
		/// <param name="size">Readed text size.</param>
		public void AddReadEntry(string text,long size)
		{
            if(m_pLoaclEndPoint == null || m_pRemoteEndPoint == null){
                m_pLoaclEndPoint  = (IPEndPoint)m_pSocket.LocalEndPoint;
                m_pRemoteEndPoint = (IPEndPoint)m_pSocket.RemoteEndPoint;
            }

			m_pEntries.Add(new SocketLogEntry(text,size,SocketLogEntryType.ReadFromRemoteEP));

			OnEntryAdded();
		}

		#endregion

		#region method AddSendEntry

		/// <summary>
		/// Adds data send(to remoteEndpoint) entry.
		/// </summary>
		/// <param name="text">Log text.</param>
		/// <param name="size">Sent text size.</param>
		public void AddSendEntry(string text,long size)
		{
            if(m_pLoaclEndPoint == null || m_pRemoteEndPoint == null){
                m_pLoaclEndPoint  = (IPEndPoint)m_pSocket.LocalEndPoint;
                m_pRemoteEndPoint = (IPEndPoint)m_pSocket.RemoteEndPoint;
            }

			m_pEntries.Add(new SocketLogEntry(text,size,SocketLogEntryType.SendToRemoteEP));

			OnEntryAdded();
		}

		#endregion

		#region method AddTextEntry

		/// <summary>
		/// Adds free text entry.
		/// </summary>
		/// <param name="text">Log text.</param>
		public void AddTextEntry(string text)
		{            
			m_pEntries.Add(new SocketLogEntry(text,0,SocketLogEntryType.FreeText));

			OnEntryAdded();
		}

		#endregion


        #region method Flush

        /// <summary>
		/// Requests to write all in memory log entries to log log file.
		/// </summary>
		public void Flush()
		{
			if(m_pLogHandler != null){
				m_pLogHandler(this,new Log_EventArgs(this,m_FirstLogPart,true));
			}
        }

        #endregion



        #region method OnEntryAdded

        /// <summary>
        /// This method is called when new loge entry has added.
        /// </summary>
		private void OnEntryAdded()
		{
			// Ask to server to write partial log
			if(m_pEntries.Count > 100){
				if(m_pLogHandler != null){
					m_pLogHandler(this,new Log_EventArgs(this,m_FirstLogPart,false));	
				}
				
				m_pEntries.Clear();
                m_FirstLogPart = false;
			}
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets or sets session ID.
		/// </summary>
		public string SessionID
		{
			get{ return m_SessionID; }

			set{ 
				m_SessionID = value; 
			}
		}

		/// <summary>
		/// Gets or sets authenticated user name.
		/// </summary>
		public string UserName
		{
			get{ return m_UserName; }

			set{ m_UserName = value; }
		}

		/// <summary>
		/// Gets current cached log entries.
		/// </summary>
		public SocketLogEntry[] LogEntries
		{
			get{ return m_pEntries.ToArray(); }
		}

		/// <summary>
		/// Gets local endpoint.
		/// </summary>
		public IPEndPoint LocalEndPoint
		{
			get{ return m_pLoaclEndPoint; }
		}

		/// <summary>
		/// Gets remote endpoint.
		/// </summary>
		public IPEndPoint RemoteEndPoint
		{
			get{ return m_pRemoteEndPoint; }
		}

		#endregion

	}
}
