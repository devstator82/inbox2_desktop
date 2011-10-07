using System;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

using LumiSoft.Net.AUTH;

namespace LumiSoft.Net.POP3.Server
{
	#region Event delegates

	/// <summary>
	/// Represents the method that will handle the AuthUser event for POP3_Server.
	/// </summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A AuthUser_EventArgs that contains the event data.</param>
	public delegate void AuthUserEventHandler(object sender,AuthUser_EventArgs e);

	/// <summary>
	/// Represents the method that will handle the GetMessgesList event for POP3_Server.
	/// </summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A GetMessagesInfo_EventArgs that contains the event data.</param>
	public delegate void GetMessagesInfoHandler(object sender,GetMessagesInfo_EventArgs e);

	/// <summary>
	/// Represents the method that will handle the GetMessage,DeleteMessage,GetTopLines event for POP3_Server.
	/// </summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A GetMessage_EventArgs that contains the event data.</param>
	public delegate void MessageHandler(object sender,POP3_Message_EventArgs e);

    /// <summary>
	/// Represents the method that will handle the GetMessageStream event for POP3_Server.
	/// </summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">Event data.</param>
	public delegate void GetMessageStreamHandler(object sender,POP3_eArgs_GetMessageStream e);

	#endregion

	/// <summary>
	/// POP3 server component.
	/// </summary>
	public class POP3_Server : SocketServer
	{       				
		#region Event declarations

		/// <summary>
		/// Occurs when new computer connected to POP3 server.
		/// </summary>
		public event ValidateIPHandler ValidateIPAddress = null;

		/// <summary>
		/// Occurs when connected user tryes to authenticate.
		/// </summary>
		public event AuthUserEventHandler AuthUser = null;

		/// <summary>
		/// Occurs user session ends. This is place for clean up.
		/// </summary>
		public event EventHandler SessionEnd = null;

		/// <summary>
		/// Occurs user session resetted. Messages marked for deletion are unmarked.
		/// </summary>
		public event EventHandler SessionResetted = null;

		/// <summary>
		/// Occurs when server needs to know logged in user's maibox messages.
		/// </summary>
		public event GetMessagesInfoHandler GetMessgesList = null;

        /// <summary>
		/// Occurs when user requests to get specified message.
		/// </summary>
		public event GetMessageStreamHandler GetMessageStream = null;

		/// <summary>
		/// Occurs when user requests delete message.
		/// </summary>		
		public event MessageHandler DeleteMessage = null;

		/// <summary>
		/// Occurs when user requests specified message TOP lines.
		/// </summary>
		public event MessageHandler GetTopLines = null;

		/// <summary>
		/// Occurs when POP3 session has finished and session log is available.
		/// </summary>
		public event LogEventHandler SessionLog = null;

		#endregion
                
        private int           m_MaxConnectionsPerIP = 0;
		private SaslAuthTypes m_SupportedAuth       = SaslAuthTypes.All;
        private string        m_GreetingText        = "";

		/// <summary>
		/// Defalut constructor.
		/// </summary>
		public POP3_Server() : base()
		{
			this.BindInfo = new IPBindInfo[]{new IPBindInfo("",IPAddress.Any,110,SslMode.None,null)};
		}


		#region override InitNewSession

		/// <summary>
		/// Initialize and start new session here. Session isn't added to session list automatically, 
		/// session must add itself to server session list by calling AddSession().
		/// </summary>
		/// <param name="socket">Connected client socket.</param>
        /// <param name="bindInfo">BindInfo what accepted socket.</param>
		protected override void InitNewSession(Socket socket,IPBindInfo bindInfo)
		{
            // Check maximum conncurent connections from 1 IP.
            if(m_MaxConnectionsPerIP > 0){
                lock(this.Sessions){
                    int nSessions = 0;
                    foreach(SocketServerSession s in this.Sessions){
                        IPEndPoint ipEndpoint = s.RemoteEndPoint;
                        if(ipEndpoint != null){
                            if(ipEndpoint.Address.Equals(((IPEndPoint)socket.RemoteEndPoint).Address)){
                                nSessions++;
                            }
                        }

                        // Maximum allowed exceeded
                        if(nSessions >= m_MaxConnectionsPerIP){
                            socket.Send(System.Text.Encoding.ASCII.GetBytes("-ERR Maximum connections from your IP address is exceeded, try again later !\r\n"));
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();
                            return;
                        }
                    }
                }
            }

            string   sessionID = Guid.NewGuid().ToString();
            SocketEx socketEx  = new SocketEx(socket);
            if(LogCommands){
                socketEx.Logger = new SocketLogger(socket,this.SessionLog);
				socketEx.Logger.SessionID = sessionID;
            }
			POP3_Session session = new POP3_Session(sessionID,socketEx,bindInfo,this);
		}

		#endregion


		#region method IsUserLoggedIn

		/// <summary>
		/// Checks if user is logged in.
		/// </summary>
		/// <param name="userName">User name.</param>
		/// <returns></returns>
		internal bool IsUserLoggedIn(string userName)
		{			
			lock(this.Sessions){
				foreach(SocketServerSession sess in this.Sessions){
					if(sess.UserName.ToLower() == userName.ToLower()){
						return true;
					}
				}
			}
			
            return false;
		}

		#endregion


		#region Properties implementation
		
        /// <summary>
		/// Gets or sets server supported authentication types.
		/// </summary>
		public SaslAuthTypes SupportedAuthentications
		{
			get{ return m_SupportedAuth; }

			set{ m_SupportedAuth = value; }
		}

        /// <summary>
		/// Gets or sets server greeting text.
		/// </summary>
		public string GreetingText
		{
			get{ return m_GreetingText; }

			set{ m_GreetingText = value; }
		}

        /// <summary>
		/// Gets or sets maximum allowed conncurent connections from 1 IP address. Value 0 means unlimited connections.
		/// </summary>
		public int MaxConnectionsPerIP
		{
			get{ return m_MaxConnectionsPerIP; }

			set{ m_MaxConnectionsPerIP = value; }
		}

        /// <summary>
		/// Gets active sessions.
		/// </summary>
		public new POP3_Session[] Sessions
		{
			get{
                SocketServerSession[] sessions     = base.Sessions;
                POP3_Session[]        pop3Sessions = new POP3_Session[sessions.Length];
                sessions.CopyTo(pop3Sessions,0);

                return pop3Sessions; 
            }
		}

		#endregion

		#region Events Implementation

		#region function OnValidate_IpAddress
        
		/// <summary>
		/// Raises event ValidateIP event.
		/// </summary>
		/// <param name="localEndPoint">Server IP.</param>
		/// <param name="remoteEndPoint">Connected client IP.</param>
		/// <returns>Returns true if connection allowed.</returns>
		internal virtual bool OnValidate_IpAddress(IPEndPoint localEndPoint,IPEndPoint remoteEndPoint) 
		{			
			ValidateIP_EventArgs oArg = new ValidateIP_EventArgs(localEndPoint,remoteEndPoint);
			if(this.ValidateIPAddress != null){
				this.ValidateIPAddress(this, oArg);
			}

			return oArg.Validated;						
		}

		#endregion

		#region function OnAuthUser

		/// <summary>
		/// Authenticates user.
		/// </summary>
		/// <param name="session">Reference to current pop3 session.</param>
		/// <param name="userName">User name.</param>
		/// <param name="passwData"></param>
		/// <param name="data"></param>
		/// <param name="authType"></param>
		/// <returns></returns>
		internal virtual AuthUser_EventArgs OnAuthUser(POP3_Session session,string userName,string passwData,string data,AuthType authType) 
		{				
			AuthUser_EventArgs oArg = new AuthUser_EventArgs(session,userName,passwData,data,authType);
			if(this.AuthUser != null){
				this.AuthUser(this,oArg);
			}
			
			return oArg;
		}

		#endregion


		#region method OnGetMessagesInfo

		/// <summary>
		/// Gest pop3 messages info.
		/// </summary>
		/// <param name="session"></param>
		/// <param name="messages"></param>
		internal virtual void OnGetMessagesInfo(POP3_Session session,POP3_MessageCollection messages) 
		{				
			GetMessagesInfo_EventArgs oArg = new GetMessagesInfo_EventArgs(session,messages,session.UserName);
			if(this.GetMessgesList != null){
				this.GetMessgesList(this, oArg);
			}
		}

		#endregion

        #region method OnGetMessageStream

        /// <summary>
        /// Raises event 'GetMessageStream'.
        /// </summary>
        /// <param name="session">Reference to POP3 session.</param>
        /// <param name="messageInfo">Message info what message stream to get.</param>
        /// <returns></returns>
        internal POP3_eArgs_GetMessageStream OnGetMessageStream(POP3_Session session,POP3_Message messageInfo)
        {
            POP3_eArgs_GetMessageStream eArgs = new POP3_eArgs_GetMessageStream(session,messageInfo);
            if(this.GetMessageStream != null){
                this.GetMessageStream(this,eArgs);
            }
            return eArgs;
        }

        #endregion
                
		#region function OnDeleteMessage

		/// <summary>
		/// Raises delete message event.
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message">Message which to delete.</param>
		/// <returns></returns>
		internal virtual bool OnDeleteMessage(POP3_Session session,POP3_Message message) 
		{				
			POP3_Message_EventArgs oArg = new POP3_Message_EventArgs(session,message,null);
			if(this.DeleteMessage != null){
				this.DeleteMessage(this,oArg);
			}
			
			return true;
		}

		#endregion

		#region function OnGetTopLines

		/// <summary>
		/// Raises event GetTopLines.
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message">Message wich top lines to get.</param>
		/// <param name="nLines">Header + number of body lines to get.</param>
		/// <returns></returns>
		internal byte[] OnGetTopLines(POP3_Session session,POP3_Message message,int nLines)
		{
			POP3_Message_EventArgs oArgs = new POP3_Message_EventArgs(session,message,null,nLines);
			if(this.GetTopLines != null){
				this.GetTopLines(this,oArgs);
			}
			return oArgs.MessageData;
		}

		#endregion

		
		#region function OnSessionEnd

		/// <summary>
		/// Raises SessionEnd event.
		/// </summary>
		/// <param name="session">Session which is ended.</param>
		internal void OnSessionEnd(object session)
		{
			if(this.SessionEnd != null){
				this.SessionEnd(session,new EventArgs());
			}
		}

		#endregion

		#region function OnSessionResetted

		/// <summary>
		/// Raises SessionResetted event.
		/// </summary>
		/// <param name="session">Session which is resetted.</param>
		internal void OnSessionResetted(object session)
		{
			if(this.SessionResetted != null){
				this.SessionResetted(session,new EventArgs());
			}
		}

		#endregion
	
		#endregion

	}
}