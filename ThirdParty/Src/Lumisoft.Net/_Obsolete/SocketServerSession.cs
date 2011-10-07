using System;
using System.Net;

namespace LumiSoft.Net
{
	/// <summary>
	/// This is base class for SocketServer sessions.
	/// </summary>
	public abstract class SocketServerSession
	{
        private string       m_SessionID = "";
        private SocketEx     m_pSocket   = null;
        private SocketServer m_pServer   = null;
        private IPBindInfo   m_pBindInfo = null;        
        private DateTime     m_SessionStartTime;
        private string       m_UserName  = "";
        private object       m_Tag       = null;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="sessionID">Session ID.</param>
        /// <param name="socket">Server connected socket.</param>
        /// <param name="bindInfo">BindInfo what accepted socket.</param>
        /// <param name="server">Reference to server.</param>
        public SocketServerSession(string sessionID,SocketEx socket,IPBindInfo bindInfo,SocketServer server)
        {
            m_SessionID = sessionID;
            m_pSocket   = socket;
            m_pBindInfo = bindInfo;
            m_pServer   = server;
                        
            m_SessionStartTime = DateTime.Now;
        }


		#region method Kill

        /// <summary>
        /// Kills session.
        /// </summary>
        public virtual void Kill()
        {
        }

        #endregion


        #region method OnSessionTimeout

        /// <summary>
		/// Times session out.
		/// </summary>
		internal protected virtual void OnSessionTimeout()
        {           
        }

        #endregion

        #region method SetUserName

        /// <summary>
        /// Sets property UserName value.
        /// </summary>
        /// <param name="userName">User name.</param>
        protected void SetUserName(string userName)
        {
            m_UserName = userName;

            if(m_pSocket.Logger != null){
                m_pSocket.Logger.UserName = m_UserName;
            }
        }

        #endregion


        #region Properties Implemtation

        /// <summary>
		/// Gets session ID.
		/// </summary>
		public string SessionID
		{
			get{ return m_SessionID; }
		}

        /// <summary>
		/// Gets session start time.
		/// </summary>
		public DateTime SessionStartTime
		{
			get{ return m_SessionStartTime; }
		}

        /// <summary>
        /// Gets if session is authenticated.
        /// </summary>
        public bool Authenticated
        {
            get{
                if(m_UserName.Length > 0){
                    return true;
                }
                else{
                    return false;
                }
            }
        }

        /// <summary>
		/// Gets authenticated user name.
		/// </summary>
		public string UserName
		{
			get{ return m_UserName; }
		}

        /// <summary>
        /// Gets how many seconds has left before timout is triggered.
        /// </summary>
        public int ExpectedTimeout
        {
            get{
                return (int)((m_pServer.SessionIdleTimeOut - ((DateTime.Now.Ticks - SessionLastDataTime.Ticks) / 10000)) / 1000);
            }
        }

        /// <summary>
		/// Gets last data activity time.
		/// </summary>
		public DateTime SessionLastDataTime
		{
			get{
                if(m_pSocket == null){
                    return DateTime.MinValue;
                }
                else{
                    return m_pSocket.LastActivity;
                }
            }
        }
                
        /// <summary>
		/// Gets EndPoint which accepted conection.
		/// </summary>
		public IPEndPoint LocalEndPoint
		{
			get{ return (IPEndPoint)m_pSocket.LocalEndPoint; }
		}

		/// <summary>
		/// Gets connected Host(client) EndPoint.
		/// </summary>
		public IPEndPoint RemoteEndPoint
		{
			get{ 
                try{
                    return (IPEndPoint)m_pSocket.RemoteEndPoint; 
                }
                catch{ // Socket closed/disposed already
                    return null;
                }
            }
		}
		
		/// <summary>
		/// Gets or sets custom user data.
		/// </summary>
		public object Tag
		{
			get{ return m_Tag; }

			set{ m_Tag = value; }
		}

        /// <summary>
        /// Gets log entries that are currently in log buffer.
        /// </summary>
        public SocketLogger SessionActiveLog
        {
            get{ return m_pSocket.Logger; }
        }

        /// <summary>
        /// Gets how many bytes are readed through this session.
        /// </summary>
        public long ReadedCount
        {
            get{ return m_pSocket.ReadedCount; }
        }
        
        /// <summary>
        /// Gets how many bytes are written through this session.
        /// </summary>
        public long WrittenCount
        {
            get{ return m_pSocket.WrittenCount; }
        }
        
        /// <summary>
        /// Gets if the connection is an SSL connection.
        /// </summary>
        public bool IsSecureConnection
        {
            get{ return m_pSocket.SSL; }
        }



        /// <summary>
        /// Gets access to SocketEx.
        /// </summary>
        protected SocketEx Socket
        {
            get{ return m_pSocket; }
        }

        /// <summary>
        /// Gets access to BindInfo what accepted socket.
        /// </summary>
        protected IPBindInfo BindInfo
        {
            get{ return m_pBindInfo; }
        }

        #endregion
	}
}
