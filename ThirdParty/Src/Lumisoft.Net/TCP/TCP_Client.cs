using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Remoting.Messaging;

using LumiSoft.Net.IO;
using LumiSoft.Net.Log;

namespace LumiSoft.Net.TCP
{    
    /// <summary>
    /// This class implements generic TCP client.
    /// </summary>
    public class TCP_Client : TCP_Session
    {
        private bool                                m_IsDisposed           = false;
        private bool                                m_IsConnected          = false;
        private string                              m_ID                   = "";
        private DateTime                            m_ConnectTime;
        private IPEndPoint                          m_pLocalEP             = null;
        private IPEndPoint                          m_pRemoteEP            = null;
        private bool                                m_IsSecure             = false;
        private SmartStream                         m_pTcpStream           = null;
        private Logger                              m_pLogger              = null;
        private RemoteCertificateValidationCallback m_pCertificateCallback = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TCP_Client()
        {
        }

        #region method Dispose

        /// <summary>
        /// Cleans up any resources being used. This method is thread-safe.
        /// </summary>
        public override void Dispose()
        {
            lock(this){
                if(m_IsDisposed){
                    return;
                }
                try{
                    Disconnect();
                }
                catch{
                }
                m_IsDisposed = true;
            }
        }

        #endregion

        
        #region method BeginConnect
        
        /// <summary>
        /// Internal helper method for asynchronous Connect method.
        /// </summary>
        /// <param name="host">Host name or IP address.</param>
        /// <param name="port">Port to connect.</param>
        /// <param name="ssl">Specifies if connects to SSL end point.</param>
        private delegate void BeginConnectHostDelegate(string host,int port,bool ssl);

        /// <summary>
        /// Internal helper method for asynchronous Connect method.
        /// </summary>
        /// <param name="localEP">Local IP end point to use for connect.</param>
        /// <param name="remoteEP">Remote IP end point where to connect.</param>
        /// <param name="ssl">Specifies if connects to SSL end point.</param>
        private delegate void BeginConnectEPDelegate(IPEndPoint localEP,IPEndPoint remoteEP,bool ssl);

        /// <summary>
        /// Starts connection to the specified host.
        /// </summary>
        /// <param name="host">Host name or IP address.</param>
        /// <param name="port">Port to connect.</param>
        /// <param name="callback">Callback to call when the connect operation is complete.</param>
        /// <param name="state">User data.</param>
        /// <returns>An IAsyncResult that references the asynchronous connection.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is already connected.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public IAsyncResult BeginConnect(string host,int port,AsyncCallback callback,object state)
        {            
            return BeginConnect(host,port,false,callback,state);
        }
                
        /// <summary>
        /// Starts connection to the specified host.
        /// </summary>
        /// <param name="host">Host name or IP address.</param>
        /// <param name="port">Port to connect.</param>
        /// <param name="ssl">Specifies if connects to SSL end point.</param>
        /// <param name="callback">Callback to call when the connect operation is complete.</param>
        /// <param name="state">User data.</param>
        /// <returns>An IAsyncResult that references the asynchronous connection.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is already connected.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public IAsyncResult BeginConnect(string host,int port,bool ssl,AsyncCallback callback,object state)
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(m_IsConnected){
                throw new InvalidOperationException("TCP client is already connected.");
            }
            if(string.IsNullOrEmpty(host)){
                throw new ArgumentException("Argument 'host' value may not be null or empty.");
            }
            if(port < 1){
                throw new ArgumentException("Argument 'port' value must be >= 1.");
            }

            BeginConnectHostDelegate asyncMethod = new BeginConnectHostDelegate(this.Connect);
            AsyncResultState asyncState = new AsyncResultState(this,asyncMethod,callback,state);
            asyncState.SetAsyncResult(asyncMethod.BeginInvoke(host,port,ssl,new AsyncCallback(asyncState.CompletedCallback),null));

            return asyncState;
        }

        /// <summary>
        /// Starts connection to the specified remote end point.
        /// </summary>
        /// <param name="remoteEP">Remote IP end point where to connect.</param>
        /// <param name="ssl">Specifies if connects to SSL end point.</param>
        /// <param name="callback">Callback to call when the connect operation is complete.</param>
        /// <param name="state">User data.</param>
        /// <returns>An IAsyncResult that references the asynchronous connection.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is already connected.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>remoteEP</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public IAsyncResult BeginConnect(IPEndPoint remoteEP,bool ssl,AsyncCallback callback,object state)
        {
            return BeginConnect(null,remoteEP,ssl,callback,state);
        }

        /// <summary>
        /// Starts connection to the specified remote end point.
        /// </summary>
        /// <param name="localEP">Local IP end point to use for connect.</param>
        /// <param name="remoteEP">Remote IP end point where to connect.</param>
        /// <param name="ssl">Specifies if connects to SSL end point.</param>
        /// <param name="callback">Callback to call when the connect operation is complete.</param>
        /// <param name="state">User data.</param>
        /// <returns>An IAsyncResult that references the asynchronous connection.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is already connected.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>remoteEP</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public IAsyncResult BeginConnect(IPEndPoint localEP,IPEndPoint remoteEP,bool ssl,AsyncCallback callback,object state)
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(m_IsConnected){
                throw new InvalidOperationException("TCP client is already connected.");
            }
            if(remoteEP == null){
                throw new ArgumentNullException("remoteEP");
            }
            
            BeginConnectEPDelegate asyncMethod = new BeginConnectEPDelegate(this.Connect);
            AsyncResultState asyncState = new AsyncResultState(this,asyncMethod,callback,state);
            asyncState.SetAsyncResult(asyncMethod.BeginInvoke(localEP,remoteEP,ssl,new AsyncCallback(asyncState.CompletedCallback),null));

            return asyncState;
        }

        #endregion

        #region method EndConnect

        /// <summary>
        /// Ends a pending asynchronous connection request.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>asyncResult</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when argument <b>asyncResult</b> was not returned by a call to the <b>BeginConnect</b> method.</exception>
        /// <exception cref="InvalidOperationException">Is raised when <b>EndConnect</b> was previously called for the asynchronous connection.</exception>
        public void EndConnect(IAsyncResult asyncResult)
        {   
            if(m_IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(asyncResult == null){
                throw new ArgumentNullException("asyncResult");
            }
                        
            AsyncResultState castedAsyncResult = asyncResult as AsyncResultState;
            if(castedAsyncResult == null || castedAsyncResult.AsyncObject != this){                
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginConnect method.");
            }
            if(castedAsyncResult.IsEndCalled){
                throw new InvalidOperationException("EndConnect was previously called for the asynchronous operation.");
            }
             
            castedAsyncResult.IsEndCalled = true;
            if(castedAsyncResult.AsyncDelegate is BeginConnectHostDelegate){
                ((BeginConnectHostDelegate)castedAsyncResult.AsyncDelegate).EndInvoke(castedAsyncResult.AsyncResult);
            }
            else if(castedAsyncResult.AsyncDelegate is BeginConnectEPDelegate){
                ((BeginConnectEPDelegate)castedAsyncResult.AsyncDelegate).EndInvoke(castedAsyncResult.AsyncResult);
            }
            else{
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginConnect method.");
            }
        }

        #endregion

        #region method Connect

        /// <summary>
        /// Connects to the specified host. If the hostname resolves to more than one IP address, 
        /// all IP addresses will be tried for connection, until one of them connects.
        /// </summary>
        /// <param name="host">Host name or IP address.</param>
        /// <param name="port">Port to connect.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is already connected.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public void Connect(string host,int port)
        {
            Connect(host,port,false);
        }

        /// <summary>
        /// Connects to the specified host. If the hostname resolves to more than one IP address, 
        /// all IP addresses will be tried for connection, until one of them connects.
        /// </summary>
        /// <param name="host">Host name or IP address.</param>
        /// <param name="port">Port to connect.</param>
        /// <param name="ssl">Specifies if connects to SSL end point.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is already connected.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public void Connect(string host,int port,bool ssl)
        {            
            if(m_IsDisposed){
                throw new ObjectDisposedException("TCP_Client");
            }
            if(m_IsConnected){
                throw new InvalidOperationException("TCP client is already connected.");
            }
            if(string.IsNullOrEmpty(host)){
                throw new ArgumentException("Argument 'host' value may not be null or empty.");
            }
            if(port < 1){
                throw new ArgumentException("Argument 'port' value must be >= 1.");
            }

            IPAddress[] ips = System.Net.Dns.GetHostAddresses(host);
            for(int i=0;i<ips.Length;i++){
                try{
					Connect(null, new IPEndPoint(ips[i], port), ssl);
                    break;
                }
                catch(Exception x){
                    if(this.IsConnected){
                        throw x;
                    }
                    // Connect failed for specified IP address, if there are some more IPs left, try next, otherwise forward exception.
                    else if(i == (ips.Length - 1)){
                        throw x;
                    }
                }
            }
        }

        /// <summary>
        /// Connects to the specified remote end point.
        /// </summary>
        /// <param name="remoteEP">Remote IP end point where to connect.</param>
        /// <param name="ssl">Specifies if connects to SSL end point.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is already connected.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>remoteEP</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public void Connect(IPEndPoint remoteEP,bool ssl)
        {
            Connect(null,remoteEP,ssl);
        }

        /// <summary>
        /// Connects to the specified remote end point.
        /// </summary>
        /// <param name="localEP">Local IP end point to use for connet.</param>
        /// <param name="remoteEP">Remote IP end point where to connect.</param>
        /// <param name="ssl">Specifies if connects to SSL end point.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is already connected.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>remoteEP</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public void Connect(IPEndPoint localEP,IPEndPoint remoteEP,bool ssl)
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException("TCP_Client");
            }
            if(m_IsConnected){
                throw new InvalidOperationException("TCP client is already connected.");
            }
            if(remoteEP == null){
                throw new ArgumentNullException("remoteEP");
            }
            
            Socket socket = null;
            if(remoteEP.AddressFamily == AddressFamily.InterNetwork){
                socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            }
            else if(remoteEP.AddressFamily == AddressFamily.InterNetworkV6){
                socket = new Socket(AddressFamily.InterNetworkV6,SocketType.Stream,ProtocolType.Tcp);
            }
            else{
                throw new ArgumentException("Remote end point has invalid AddressFamily.");
            }
        
            try{                
                socket.SendTimeout = 30000;
                socket.ReceiveTimeout = 30000;

                if(localEP != null){
                    socket.Bind(localEP);
                }

                LogAddText("Connecting to " + remoteEP.ToString() + ".");

                socket.Connect(remoteEP);

                m_IsConnected = true;
                m_ID          = Guid.NewGuid().ToString();
                m_ConnectTime = DateTime.Now;
                m_pLocalEP    = (IPEndPoint)socket.LocalEndPoint;
                m_pRemoteEP   = (IPEndPoint)socket.RemoteEndPoint;
                m_pTcpStream  = new SmartStream(new NetworkStream(socket,true),true);

                LogAddText("Connected, localEP='" + m_pLocalEP.ToString() + "'; remoteEP='" + remoteEP.ToString() + "'.");
                
                if(ssl){
					SwitchToSecure();
                }                
            }
            catch(Exception x){
                LogAddException("Exception: " + x.Message,x);

                // Switching to secure failed.
                if(m_IsConnected){
                    Disconnect();
                }
                // Bind or connect failed.
                else{
                    socket.Close();
                }                

                OnError(x);
                
                throw x;
            }

            OnConnected();
        }

        #endregion

        #region method Disconnect

        /// <summary>
        /// Disconnects connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public override void Disconnect()
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException("TCP_Client");
            }
            if(!m_IsConnected){
                throw new InvalidOperationException("TCP client is not connected.");
            }
            m_IsConnected = false;

            m_pLocalEP = null;
            m_pRemoteEP = null;
            m_pTcpStream.Dispose();
            m_IsSecure = false;
            m_pTcpStream = null;

            LogAddText("Disconnected.");
        }

        #endregion

        #region method BeginDisconnect

        /// <summary>
        /// Internal helper method for asynchronous Disconnect method.
        /// </summary>
        private delegate void DisconnectDelegate();

        /// <summary>
        /// Starts disconnecting connection.
        /// </summary>
        /// <param name="callback">Callback to call when the asynchronous operation is complete.</param>
        /// <param name="state">User data.</param>
        /// <returns>An IAsyncResult that references the asynchronous disconnect.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public IAsyncResult BeginDisconnect(AsyncCallback callback,object state)
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!m_IsConnected){
                throw new InvalidOperationException("TCP client is not connected.");
            }

            DisconnectDelegate asyncMethod = new DisconnectDelegate(this.Disconnect);
            AsyncResultState asyncState = new AsyncResultState(this,asyncMethod,callback,state);
            asyncState.SetAsyncResult(asyncMethod.BeginInvoke(new AsyncCallback(asyncState.CompletedCallback),null));

            return asyncState;
        }

        #endregion

        #region method EndDisconnect

        /// <summary>
        /// Ends a pending asynchronous disconnect request.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>asyncResult</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when argument <b>asyncResult</b> was not returned by a call to the <b>BeginDisconnect</b> method.</exception>
        /// <exception cref="InvalidOperationException">Is raised when <b>EndDisconnect</b> was previously called for the asynchronous connection.</exception>
        public void EndDisconnect(IAsyncResult asyncResult)
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(asyncResult == null){
                throw new ArgumentNullException("asyncResult");
            }
            
            AsyncResultState castedAsyncResult = asyncResult as AsyncResultState;
            if(castedAsyncResult == null || castedAsyncResult.AsyncObject != this){
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginDisconnect method.");
            }
            if(castedAsyncResult.IsEndCalled){
                throw new InvalidOperationException("EndDisconnect was previously called for the asynchronous connection.");
            }
             
            castedAsyncResult.IsEndCalled = true;
            if(castedAsyncResult.AsyncDelegate is DisconnectDelegate){
                ((DisconnectDelegate)castedAsyncResult.AsyncDelegate).EndInvoke(castedAsyncResult.AsyncResult);
            }
            else{
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginDisconnect method.");
            }
        }

        #endregion


        #region method SwitchToSecure

        /// <summary>
        /// Switches session to secure connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected or is already secure.</exception>
        protected void SwitchToSecure()
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException("TCP_Client");
            }
            if(!m_IsConnected){
                throw new InvalidOperationException("TCP client is not connected.");
            }
            if(m_IsSecure){
                throw new InvalidOperationException("TCP client is already secure.");
            }

            LogAddText("Switching to SSL.");

            // FIX ME: if ssl switching fails, it closes source stream or otherwise if ssl successful, source stream leaks.

            SslStream sslStream = new SslStream(m_pTcpStream.SourceStream,true,this.RemoteCertificateValidationCallback);
			sslStream.AuthenticateAsClient(String.Empty);

            // Close old stream, but leave source stream open.
            m_pTcpStream.IsOwner = false;
            m_pTcpStream.Dispose();

            m_IsSecure = true;
            m_pTcpStream = new SmartStream(sslStream,true);
        }

        #region method RemoteCertificateValidationCallback

        private bool RemoteCertificateValidationCallback(object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
        {
            // User will handle it.
            if(m_pCertificateCallback != null){
                return m_pCertificateCallback(sender,certificate,chain,sslPolicyErrors);
            }
            else{
                if(sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch){
                    return true;
                }

                // Do not allow this client to communicate with unauthenticated servers.
                return false;
            }
        }

        #endregion

        #endregion
               
        // Do we need it ?
        #region method OnError

        /// <summary>
        /// This must be called when unexpected error happens. When inheriting <b>TCP_Client</b> class, be sure that you call <b>OnError</b>
        /// method for each unexpected error.
        /// </summary>
        /// <param name="x">Exception happened.</param>
        protected void OnError(Exception x)
        {
            try{
                if(m_pLogger != null){
                    //m_pLogger.AddException(x);
                }
            }
            catch{
            }
        }

        #endregion

        #region virtual method OnConnected

        /// <summary>
        /// This method is called after TCP client has sucessfully connected.
        /// </summary>
        protected virtual void OnConnected()
        {
        }

        #endregion


        #region method ReadLine

        /// <summary>
        /// Reads and logs specified line from connected host.
        /// </summary>
        /// <returns>Returns readed line.</returns>
        public string ReadLine()
        {
            SmartStream.ReadLineAsyncOP args = new SmartStream.ReadLineAsyncOP(new byte[32000],SizeExceededAction.JunkAndThrowException);
            this.TcpStream.ReadLine(args,false);
            if(args.Error != null){
                throw args.Error;
            }
            string line = args.LineUtf8;
            if(args.BytesInBuffer > 0){
                LogAddRead(args.BytesInBuffer,line);
            }
            else{
                LogAddText("Remote host closed connection.");
            }

            return line;
        }

        #endregion

        #region method WriteLine

        /// <summary>
        /// Sends and logs specified line to connected host.
        /// </summary>
        /// <param name="line">Line to send.</param>
        protected void WriteLine(string line)
        {
            if(line == null){
                throw new ArgumentNullException("line");
            }

            int countWritten = this.TcpStream.WriteLine(line);
            LogAddWrite(countWritten,line);
        }

        #endregion


        #region mehtod LogAddRead

        /// <summary>
        /// Logs read operation.
        /// </summary>
        /// <param name="size">Number of bytes readed.</param>
        /// <param name="text">Log text.</param>
        protected void LogAddRead(long size,string text)
        {
            try{
                if(m_pLogger != null){
                    m_pLogger.AddRead(
                        this.ID,
                        this.AuthenticatedUserIdentity,
                        size,
                        text,                        
                        this.LocalEndPoint,
                        this.RemoteEndPoint
                    );
                }
            }
            catch{
                // We skip all logging errors, normally there shouldn't be any.
            }
        }

        #endregion

        #region method LogAddWrite

        /// <summary>
        /// Logs write operation.
        /// </summary>
        /// <param name="size">Number of bytes written.</param>
        /// <param name="text">Log text.</param>
        protected void LogAddWrite(long size,string text)
        {
            try{
                if(m_pLogger != null){
                    m_pLogger.AddWrite(
                        this.ID,
                        this.AuthenticatedUserIdentity,
                        size,
                        text,                        
                        this.LocalEndPoint,
                        this.RemoteEndPoint
                    );
                }
            }
            catch{
                // We skip all logging errors, normally there shouldn't be any.
            }
        }

        #endregion

        #region method LogAddText

        /// <summary>
        /// Logs free text entry.
        /// </summary>
        /// <param name="text">Log text.</param>
        protected void LogAddText(string text)
        {
            try{
                if(m_pLogger != null){
                    m_pLogger.AddText(
                        this.IsConnected ? this.ID : "",
                        this.IsConnected ? this.AuthenticatedUserIdentity : null,
                        text,                        
                        this.IsConnected ? this.LocalEndPoint : null,
                        this.IsConnected ? this.RemoteEndPoint : null
                    );
                }
            }
            catch{
                // We skip all logging errors, normally there shouldn't be any.
            }
        }

        #endregion

        #region method LogAddException

        /// <summary>
        /// Logs exception.
        /// </summary>
        /// <param name="text">Log text.</param>
        /// <param name="x">Exception happened.</param>
        protected void LogAddException(string text,Exception x)
        {
            try{
                if(m_pLogger != null){
                    m_pLogger.AddException(
                        this.IsConnected ? this.ID : "",
                        this.IsConnected ? this.AuthenticatedUserIdentity : null,
                        text,                        
                        this.IsConnected ? this.LocalEndPoint : null,
                        this.IsConnected ? this.RemoteEndPoint : null,
                        x
                    );
                }
            }
            catch{
                // We skip all logging errors, normally there shouldn't be any.
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
        /// Gets or sets TCP client logger. Value null means no logging.
        /// </summary>
        public Logger Logger
        {
            get{ return m_pLogger; }

            set{ m_pLogger = value; }
        }


        /// <summary>
        /// Gets if TCP client is connected.
        /// </summary>
        public override bool IsConnected
        {
            get{ return m_IsConnected; }
        }

        /// <summary>
        /// Gets session ID.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public override string ID
        {
            get{                
                if(m_IsDisposed){
                    throw new ObjectDisposedException("TCP_Client");
                }
                if(!m_IsConnected){
                    throw new InvalidOperationException("TCP client is not connected.");
                }

                return m_ID; 
            }
        }

        /// <summary>
        /// Gets the time when session was connected.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public override DateTime ConnectTime
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException("TCP_Client");
                }
                if(!m_IsConnected){
                    throw new InvalidOperationException("TCP client is not connected.");
                }

                return m_ConnectTime; 
            }
        }

        /// <summary>
        /// Gets the last time when data was sent or received.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public override DateTime LastActivity
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("TCP_Client");
                }
                if(!m_IsConnected){
                    throw new InvalidOperationException("TCP client is not connected.");
                }

                return m_pTcpStream.LastActivity; 
            }
        }

        /// <summary>
        /// Gets session local IP end point.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public override IPEndPoint LocalEndPoint
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("TCP_Client");
                }
                if(!m_IsConnected){
                    throw new InvalidOperationException("TCP client is not connected.");
                }

                return m_pLocalEP; 
            }
        }

        /// <summary>
        /// Gets session remote IP end point.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public override IPEndPoint RemoteEndPoint
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("TCP_Client");
                }
                if(!m_IsConnected){
                    throw new InvalidOperationException("TCP client is not connected.");
                }

                return m_pRemoteEP; 
            }
        }
        
        /// <summary>
        /// Gets if this session TCP connection is secure connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public override bool IsSecureConnection
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("TCP_Client");
                }
                if(!m_IsConnected){
                    throw new InvalidOperationException("TCP client is not connected.");
                }

                return m_IsSecure; 
            }
        }

        /// <summary>
        /// Gets TCP stream which must be used to send/receive data through this session.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when TCP client is not connected.</exception>
        public override SmartStream TcpStream
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("TCP_Client");
                }
                if(!m_IsConnected){
                    throw new InvalidOperationException("TCP client is not connected.");
                }

                return m_pTcpStream; 
            }
        }

        /// <summary>
        /// Gets or stes remote callback which is called when remote server certificate needs to be validated.
        /// Value null means not sepcified.
        /// </summary>
        public RemoteCertificateValidationCallback ValidateCertificateCallback
        {
            get{ return m_pCertificateCallback; }

            set{ m_pCertificateCallback = value; }
        }

        #endregion

    }
}
