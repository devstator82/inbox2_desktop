using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Principal;

using LumiSoft.Net;
using LumiSoft.Net.IO;
using LumiSoft.Net.TCP;
using LumiSoft.Net.SMTP.Client;
using LumiSoft.Net.Dns.Client;
using LumiSoft.Net.Log;

namespace LumiSoft.Net.SMTP.Relay
{
    /// <summary>
    /// This class implements SMTP relay server session.
    /// </summary>
    public class Relay_Session : TCP_Session
    {
        #region class Relay_Target

        /// <summary>
        /// This class holds relay target information.
        /// </summary>
        private class Relay_Target
        {
            private IPEndPoint m_pTarget  = null;
            private SslMode    m_SslMode  = SslMode.None;
            private string     m_UserName = null;
            private string     m_Password = null;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="target">Target host IP end point.</param>
            public Relay_Target(IPEndPoint target)
            {
                m_pTarget = target;
            }

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="target">Target host IP end point.</param>
            /// <param name="sslMode">SSL mode.</param>
            /// <param name="userName">Target host user name.</param>
            /// <param name="password">Target host password.</param>
            public Relay_Target(IPEndPoint target,SslMode sslMode,string userName,string password)
            {
                m_pTarget  = target;
                m_SslMode  = sslMode;
                m_UserName = userName;
                m_Password = password;
            }


            #region Properties Implementation

            /// <summary>
            /// Gets specified target IP end point.
            /// </summary>
            public IPEndPoint Target
            {
                get{ return m_pTarget; }
            }

            /// <summary>
            /// Gets target SSL mode.
            /// </summary>
            public SslMode SslMode
            {
                get{ return m_SslMode; }
            }

            /// <summary>
            /// Gets target server user name.
            /// </summary>
            public string UserName
            {
                get{ return m_UserName; }
            }

            /// <summary>
            /// Gets target server password.
            /// </summary>
            public string Password
            {
                get{ return m_Password; }
            }

            #endregion

        }

        #endregion

        private bool               m_IsDisposed     = false;
        private Relay_Server       m_pServer        = null;
        private IPBindInfo         m_pLocalBindInfo = null;
        private Relay_QueueItem    m_pRelayItem     = null;
        private Relay_SmartHost[]  m_pSmartHosts    = null;
        private Relay_Mode         m_RelayMode      = Relay_Mode.Dns;
        private string             m_SessionID      = "";
        private DateTime           m_SessionCreateTime;
        private SMTP_Client        m_pSmtpClient    = null;
        private List<Relay_Target> m_pTargets       = null;
        private Relay_Target       m_pActiveTarget  = null;
        
        /// <summary>
        /// Dns relay session constructor.
        /// </summary>
        /// <param name="server">Owner relay server.</param>
        /// <param name="localBindInfo">Local bind info.</param>
        /// <param name="realyItem">Relay item.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>server</b>,<b>localBindInfo</b> or <b>realyItem</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        internal Relay_Session(Relay_Server server,IPBindInfo localBindInfo,Relay_QueueItem realyItem)
        {
            if(server == null){
                throw new ArgumentNullException("server");
            }
            if(localBindInfo == null){
                throw new ArgumentNullException("localBindInfo");
            }
            if(realyItem == null){
                throw new ArgumentNullException("realyItem");
            }

            m_pServer        = server;
            m_pLocalBindInfo = localBindInfo;
            m_pRelayItem     = realyItem;

            m_SessionID         = Guid.NewGuid().ToString();
            m_SessionCreateTime = DateTime.Now;
            m_pTargets          = new List<Relay_Target>();
        }

        /// <summary>
        /// Smart host relay session constructor.
        /// </summary>
        /// <param name="server">Owner relay server.</param>
        /// <param name="localBindInfo">Local bind info.</param>
        /// <param name="realyItem">Relay item.</param>
        /// <param name="smartHosts">Smart hosts.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>server</b>,<b>localBindInfo</b>,<b>realyItem</b> or <b>smartHosts</b>is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        internal Relay_Session(Relay_Server server,IPBindInfo localBindInfo,Relay_QueueItem realyItem,Relay_SmartHost[] smartHosts)
        {
            if(server == null){
                throw new ArgumentNullException("server");
            }
            if(localBindInfo == null){
                throw new ArgumentNullException("localBindInfo");
            }
            if(realyItem == null){
                throw new ArgumentNullException("realyItem");
            }
            if(smartHosts == null){
                throw new ArgumentNullException("smartHosts");
            }

            m_pServer        = server;
            m_pLocalBindInfo = localBindInfo;
            m_pRelayItem     = realyItem;
            m_pSmartHosts    = smartHosts;
                        
            m_RelayMode         = Relay_Mode.SmartHost;
            m_SessionID         = Guid.NewGuid().ToString();
            m_SessionCreateTime = DateTime.Now;
            m_pTargets          = new List<Relay_Target>();
        }

        #region override method Dispose

        /// <summary>
        /// Completes relay session and does clean up. This method is thread-safe.
        /// </summary>
        public override void Dispose()
        {
            Dispose(new ObjectDisposedException(this.GetType().Name));
        }

        /// <summary>
        /// Completes relay session and does clean up. This method is thread-safe.
        /// </summary>
        /// <param name="exception">Exception happened or null if relay completed successfully.</param>
        public void Dispose(Exception exception)
        {
            try{
                lock(this){
                    if(m_IsDisposed){
                        return;
                    }
                    try{
                        m_pServer.OnSessionCompleted(this,exception);
                    }
                    catch{
                    }
                    m_pServer.Sessions.Remove(this);
                    m_IsDisposed = true;
                        
                    m_pLocalBindInfo = null;
                    m_pRelayItem = null;
                    m_pSmartHosts = null;
                    if(m_pSmtpClient != null){
                        m_pSmtpClient.Dispose();
                        m_pSmtpClient = null;
                    }
                    m_pTargets = null;
                    if(m_pActiveTarget != null){
                        m_pServer.RemoveIpUsage(m_pActiveTarget.Target.Address);
                        m_pActiveTarget = null;
                    }
                    m_pServer = null;
                }
            }
            catch(Exception x){
                if(m_pServer != null){
                    m_pServer.OnError(x);
                }
            }
        }

        #endregion


        #region method Start

        /// <summary>
        /// Start processing relay message.
        /// </summary>
        /// <param name="state">User data.</param>
        internal void Start(object state)
        {
            try{
                m_pSmtpClient = new SMTP_Client();
                m_pSmtpClient.LocalHostName = m_pLocalBindInfo.HostName;
                if(m_pServer.Logger != null){
                    m_pSmtpClient.Logger = new Logger();
                    m_pSmtpClient.Logger.WriteLog += new EventHandler<WriteLogEventArgs>(SmtpClient_WriteLog);
                }

                LogText("Starting to relay message '" + m_pRelayItem.MessageID + "' from '" + m_pRelayItem.From + "' to '" + m_pRelayItem.To + "'.");

                // Get all possible target hosts for active recipient.
                List<string> targetHosts = new List<string>();                
                if(m_RelayMode == Relay_Mode.Dns){
                    foreach(string host in SMTP_Client.GetDomainHosts(m_pRelayItem.To)){
                        try{
                            foreach(IPAddress ip in Dns_Client.Resolve(host)){
                                m_pTargets.Add(new Relay_Target(new IPEndPoint(ip,25)));                                
                            }
                        }
                        catch{
                            // Failed to resolve host name.
                            
                            LogText("Failed to resolve host '" + host + "' name.");
                        }
                    }
                }
                else if(m_RelayMode == Relay_Mode.SmartHost){
                    foreach(Relay_SmartHost smartHost in m_pSmartHosts){
                        try{
                            m_pTargets.Add(new Relay_Target(new IPEndPoint(Dns_Client.Resolve(smartHost.Host)[0],smartHost.Port),smartHost.SslMode,smartHost.UserName,smartHost.Password));                            
                        }
                        catch{
                            // Failed to resolve smart host name.
                            
                            LogText("Failed to resolve smart host '" + smartHost.Host + "' name.");
                        }
                    }
                }
                                
                BeginConnect();
            }
            catch(Exception x){
                Dispose(x);
            }
        }
                
        #endregion


        #region override method Disconnect

        /// <summary>
        /// Closes relay connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        public override void Disconnect()
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
                return;
            }

            m_pSmtpClient.Disconnect();
        }

        /// <summary>
        /// Closes relay connection.
        /// </summary>
        /// <param name="text">Text to send to the connected host.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        public void Disconnect(string text)
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
                return;
            }

            m_pSmtpClient.TcpStream.WriteLine(text);
            Disconnect();
        }

        #endregion
        
        
        #region method BeginConnect

        /// <summary>
        /// Starts connecting to best target. 
        /// </summary>
        private void BeginConnect()
        {
            // No tagets, abort relay.
            if(m_pTargets.Count == 0){
                LogText("No relay target(s) for '" + m_pRelayItem.To + "', aborting.");
                Dispose(new Exception("No relay target(s) for '" + m_pRelayItem.To + "', aborting."));
                return;
            }

            // If maximum connections to specified target exceeded and there are more targets, try to get limit free target.            
            if(m_pServer.MaxConnectionsPerIP > 0){
                // For DNS or load-balnced smart host relay, search free target if any.
                if(m_pServer.RelayMode == Relay_Mode.Dns || m_pServer.SmartHostsBalanceMode == BalanceMode.LoadBalance){
                    foreach(Relay_Target t in m_pTargets){
                        // We found free target, stop searching.
                        if(m_pServer.TryAddIpUsage(m_pTargets[0].Target.Address)){
                            m_pActiveTarget = t;
                            m_pTargets.Remove(t);
                            break;
                        }
                    }
                }
                // Smart host fail-over mode, just check if it's free.
                else{
                    // Smart host IP limit not reached.
                    if(m_pServer.TryAddIpUsage(m_pTargets[0].Target.Address)){
                        m_pActiveTarget = m_pTargets[0];
                        m_pTargets.RemoveAt(0);
                    }
                }                
            }
            // Just get first target.
            else{
                m_pActiveTarget = m_pTargets[0];
                m_pTargets.RemoveAt(0);
            }

            // If all targets has exeeded maximum allowed connection per IP address, end relay session, 
            // next relay cycle will try to relay again.
            if(m_pActiveTarget == null){
                LogText("All targets has exeeded maximum allowed connection per IP address, skip relay.");
                Dispose(new Exception("All targets has exeeded maximum allowed connection per IP address, skip relay."));
                return;
            }

            m_pSmtpClient.BeginConnect(new IPEndPoint(m_pLocalBindInfo.IP,0),m_pActiveTarget.Target,false,new AsyncCallback(this.ConnectCallback),null);
        }

        #endregion

        #region method ConnectCallback

        /// <summary>
        /// This method is called when asynchronous Connect method completes.
        /// </summary>
        /// <param name="ar">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        private void ConnectCallback(IAsyncResult ar)
        {
            try{
                m_pSmtpClient.EndConnect(ar);                
                                
                // Start TLS requested, start switching to SSL.
                if(m_pActiveTarget.SslMode == SslMode.TLS){
                    m_pSmtpClient.BeginStartTLS(new AsyncCallback(this.StartTlsCallback),null);
                }
                // Authentication requested, start authenticating.
                else if(!string.IsNullOrEmpty(m_pActiveTarget.UserName)){
                    m_pSmtpClient.BeginAuthenticate(m_pActiveTarget.UserName,m_pActiveTarget.Password,new AsyncCallback(this.AuthenticateCallback),null);
                }
                else{
                    long messageSize = -1;
                    try{
                        messageSize = m_pRelayItem.MessageStream.Length - m_pRelayItem.MessageStream.Position;
                    }
                    catch{
                        // Stream doesn't support seeking.
                    }

                    m_pSmtpClient.BeginMailFrom(this.From,messageSize,new AsyncCallback(this.MailFromCallback),null);
                }
            }
            catch(Exception x){
                try{
                    // Release IP usage.
                    m_pServer.RemoveIpUsage(m_pActiveTarget.Target.Address);
                    m_pActiveTarget = null;

                    // Connect failed, if there are more target IPs, try next one.
                    if(!this.IsDisposed && !this.IsConnected && m_pTargets.Count > 0){
                        BeginConnect();
                    }
                    else{
                        Dispose(x);
                    }
                }
                catch(Exception xx){
                    Dispose(xx);
                }
            }
        }

        #endregion

        #region method StartTlsCallback

        /// <summary>
        /// This method is called when asynchronous <b>StartTLS</b> method completes.
        /// </summary>
        /// <param name="ar">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        private void StartTlsCallback(IAsyncResult ar)
        {
            try{
                m_pSmtpClient.EndStartTLS(ar);

                // Authentication requested, start authenticating.
                if(!string.IsNullOrEmpty(m_pActiveTarget.UserName)){
                    m_pSmtpClient.BeginAuthenticate(m_pActiveTarget.UserName,m_pActiveTarget.Password,new AsyncCallback(this.AuthenticateCallback),null);
                }
                else{
                    long messageSize = -1;
                    try{
                        messageSize = m_pRelayItem.MessageStream.Length - m_pRelayItem.MessageStream.Position;
                    }
                    catch{
                        // Stream doesn't support seeking.
                    }

                    m_pSmtpClient.BeginMailFrom(this.From,messageSize,new AsyncCallback(this.MailFromCallback),null);
                }
            }
            catch(Exception x){
                Dispose(x);
            }
        }

        #endregion

        #region method AuthenticateCallback

        /// <summary>
        /// This method is called when asynchronous <b>Authenticate</b> method completes.
        /// </summary>
        /// <param name="ar">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        private void AuthenticateCallback(IAsyncResult ar)
        {
            try{
                m_pSmtpClient.EndAuthenticate(ar);

                long messageSize = -1;
                try{
                    messageSize = m_pRelayItem.MessageStream.Length - m_pRelayItem.MessageStream.Position;
                }
                catch{
                    // Stream doesn't support seeking.
                }

                m_pSmtpClient.BeginMailFrom(this.From,messageSize,new AsyncCallback(this.MailFromCallback),null);
            }
            catch(Exception x){
                Dispose(x);
            }
        }

        #endregion

        #region method MailFromCallback

        /// <summary>
        /// This method is called when asynchronous MailFrom method completes.
        /// </summary>
        /// <param name="ar">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        private void MailFromCallback(IAsyncResult ar)
        {
            try{
                m_pSmtpClient.EndMailFrom(ar);

                m_pSmtpClient.BeginRcptTo(this.To,new AsyncCallback(this.RcptToCallback),null);
            }
            catch(Exception x){
                Dispose(x);
            }
        }

        #endregion

        #region method RcptToCallback

        /// <summary>
        /// This method is called when asynchronous RcptTo method completes.
        /// </summary>
        /// <param name="ar">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        private void RcptToCallback(IAsyncResult ar)
        {
            try{
                m_pSmtpClient.EndRcptTo(ar);

                m_pSmtpClient.BeginSendMessage(m_pRelayItem.MessageStream,new AsyncCallback(this.SendMessageCallback),null);
            }
            catch(Exception x){
                Dispose(x);
            }
        }

        #endregion

        #region method SendMessageCallback

        /// <summary>
        /// This method is called when asynchronous SendMessage method completes.
        /// </summary>
        /// <param name="ar">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        private void SendMessageCallback(IAsyncResult ar)
        {
            try{
                m_pSmtpClient.EndSendMessage(ar);

                // Message relayed successfully.
                Dispose(null);
            }
            catch(Exception x){
                Dispose(x);
            }
        }

        #endregion


        #region method SmtpClient_WriteLog

        /// <summary>
        /// Thsi method is called when SMTP client has new log entry available.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void SmtpClient_WriteLog(object sender,WriteLogEventArgs e)
        {
            try{
                if(m_pServer.Logger == null){
                }
                else if(e.LogEntry.EntryType == LogEntryType.Read){
                    m_pServer.Logger.AddRead(m_SessionID,e.LogEntry.UserIdentity,e.LogEntry.Size,e.LogEntry.Text,e.LogEntry.LocalEndPoint,e.LogEntry.RemoteEndPoint);
                }
                else if(e.LogEntry.EntryType == LogEntryType.Text){
                    m_pServer.Logger.AddText(m_SessionID,e.LogEntry.UserIdentity,e.LogEntry.Text,e.LogEntry.LocalEndPoint,e.LogEntry.RemoteEndPoint);
                }
                else if(e.LogEntry.EntryType == LogEntryType.Write){
                    m_pServer.Logger.AddWrite(m_SessionID,e.LogEntry.UserIdentity,e.LogEntry.Size,e.LogEntry.Text,e.LogEntry.LocalEndPoint,e.LogEntry.RemoteEndPoint);
                }
            }
            catch{
            }
        }

        #endregion

        #region method LogText

        /// <summary>
        /// Logs specified text if logging enabled.
        /// </summary>
        /// <param name="text">Text to log.</param>
        private void LogText(string text)
        {
            if(m_pServer.Logger != null){
                GenericIdentity identity = null;
                try{
                    identity = this.AuthenticatedUserIdentity;
                }
                catch{
                }
                IPEndPoint localEP  = null;
                IPEndPoint remoteEP = null;
                try{
                    localEP  = m_pSmtpClient.LocalEndPoint;
                    remoteEP = m_pSmtpClient.RemoteEndPoint;
                }
                catch{
                }
                m_pServer.Logger.AddText(m_SessionID,identity,text,localEP,remoteEP);
            }
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets if this object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get{ return m_IsDisposed; }
        }

        /// <summary>
        /// Gets time when relay session created.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public DateTime SessionCreateTime
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_SessionCreateTime; 
            }
        }

        /// <summary>
        /// Gets how many seconds has left before timout is triggered.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public int ExpectedTimeout
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return (int)(m_pServer.SessionIdleTimeout - ((DateTime.Now.Ticks - this.TcpStream.LastActivity.Ticks) / 10000));
            }
        }

        /// <summary>
        /// Gets from address.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public string From
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pRelayItem.From; 
            }
        }

        /// <summary>
        /// Gets target recipient.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public string To
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                
                return m_pRelayItem.To; 
            }
        }

        /// <summary>
        /// Gets message ID which is being relayed now.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public string MessageID
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pRelayItem.MessageID; 
            }
        }

        /// <summary>
        /// Gets message what is being relayed now.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public Stream MessageStream
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pRelayItem.MessageStream; 
            }
        }

        /// <summary>
        /// Gets relay queue which session it is.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public Relay_Queue Queue
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pRelayItem.Queue; 
            }
        }

        /// <summary>
        /// Gets user data what was procided to Relay_Queue.QueueMessage method.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public object QueueTag
        {
            get{               
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pRelayItem.Tag; 
            }
        }


        /// <summary>
        /// Gets session authenticated user identity, returns null if not authenticated.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when this property is accessed and relay session is not connected.</exception>
        public override GenericIdentity AuthenticatedUserIdentity
        {
            get{ 
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                if(!m_pSmtpClient.IsConnected){
				    throw new InvalidOperationException("You must connect first.");
			    }

                return m_pSmtpClient.AuthenticatedUserIdentity; 
            }
        }

        /// <summary>
        /// Gets if session is connected.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public override bool IsConnected
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pSmtpClient.IsConnected; 
            }
        }

        /// <summary>
        /// Gets session ID.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public override string ID
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_SessionID; 
            }
        }

        /// <summary>
        /// Gets the time when session was connected.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public override DateTime ConnectTime
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pSmtpClient.ConnectTime; 
            }
        }

        /// <summary>
        /// Gets the last time when data was sent or received.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public override DateTime LastActivity
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pSmtpClient.LastActivity; 
            }
        }

        /// <summary>
        /// Gets session local IP end point.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public override IPEndPoint LocalEndPoint
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pSmtpClient.LocalEndPoint; 
            }
        }

        /// <summary>
        /// Gets session remote IP end point.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public override IPEndPoint RemoteEndPoint
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pSmtpClient.RemoteEndPoint; 
            }
        }

        /// <summary>
        /// Gets TCP stream which must be used to send/receive data through this session.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public override SmartStream TcpStream
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_pSmtpClient.TcpStream; 
            }
        }

        #endregion

    }
}
