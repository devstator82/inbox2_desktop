using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Security.Principal;

using LumiSoft.Net.IO;
using LumiSoft.Net.TCP;

namespace LumiSoft.Net.POP3.Client
{
	/// <summary>
	/// POP3 Client. Defined in RFC 1939.
	/// </summary>
	/// <example>
	/// <code>
	/// 
	/// /*
	///  To make this code to work, you need to import following namespaces:
	///  using LumiSoft.Net.Mime;
	///  using LumiSoft.Net.POP3.Client; 
	///  */
	/// 
	/// using(POP3_Client c = new POP3_Client()){
	///		c.Connect("ivx",WellKnownPorts.POP3);
	///		c.Authenticate("test","test",true);
	///				
	///		// Get first message if there is any
	///		if(c.Messages.Count > 0){
	///			// Do your suff
	///			
	///			// Parse message
	///			Mime m = Mime.Parse(c.Messages[0].MessageToByte());
	///			string from = m.MainEntity.From;
	///			string subject = m.MainEntity.Subject;			
	///			// ... 
	///		}		
	///	}
	/// </code>
	/// </example>
	public class POP3_Client : TCP_Client
	{
        private string                       m_GreetingText       = "";
		private string                       m_ApopHashKey        = "";
        private List<string>                 m_pExtCapabilities   = null;
        private bool                         m_IsUidlSupported    = false;
        private POP3_ClientMessageCollection m_pMessages          = null;
        private GenericIdentity              m_pAuthdUserIdentity = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public POP3_Client()
		{
	        m_pExtCapabilities = new List<string>();
		}

		#region override method Dispose

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();
		}

		#endregion


		#region override method Disconnect

		/// <summary>
		/// Closes connection to POP3 server.
		/// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected.</exception>
		public override void Disconnect()
		{
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
                throw new InvalidOperationException("POP3 client is not connected.");
            }

			try{
                // Send QUIT command to server.                
                WriteLine("QUIT");
			}
			catch{
			}

            try{
                base.Disconnect(); 
            }
            catch{
            }

            if(m_pMessages != null){
                m_pMessages.Dispose();
                m_pMessages = null;
            }            
            m_ApopHashKey = "";
            m_pExtCapabilities.Clear();
            m_IsUidlSupported = false;
		}

		#endregion
                
        #region method BeginStartTLS

        /// <summary>
        /// Internal helper method for asynchronous StartTLS method.
        /// </summary>
        private delegate void StartTLSDelegate();

        /// <summary>
        /// Starts switching to SSL.
        /// </summary>
        /// <returns>An IAsyncResult that references the asynchronous operation.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected or is authenticated or is already secure connection.</exception>
        public IAsyncResult BeginStartTLS(AsyncCallback callback,object state)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
				throw new InvalidOperationException("You must connect first.");
			}
			if(this.IsAuthenticated){
				throw new InvalidOperationException("The STLS command is only valid in non-authenticated state.");
			}
            if(this.IsSecureConnection){
                throw new InvalidOperationException("Connection is already secure.");
            }

            StartTLSDelegate asyncMethod = new StartTLSDelegate(this.StartTLS);
            AsyncResultState asyncState = new AsyncResultState(this,asyncMethod,callback,state);
            asyncState.SetAsyncResult(asyncMethod.BeginInvoke(new AsyncCallback(asyncState.CompletedCallback),null));

            return asyncState;
        }

        #endregion

        #region method EndStartTLS

        /// <summary>
        /// Ends a pending asynchronous StartTLS request.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>asyncResult</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when invalid <b>asyncResult</b> passed to this method.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 server returns error.</exception>
        public void EndStartTLS(IAsyncResult asyncResult)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(asyncResult == null){
                throw new ArgumentNullException("asyncResult");
            }

            AsyncResultState castedAsyncResult = asyncResult as AsyncResultState;
            if(castedAsyncResult == null || castedAsyncResult.AsyncObject != this){
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginReset method.");
            }
            if(castedAsyncResult.IsEndCalled){
                throw new InvalidOperationException("BeginReset was previously called for the asynchronous connection.");
            }
             
            castedAsyncResult.IsEndCalled = true;
            if(castedAsyncResult.AsyncDelegate is StartTLSDelegate){
                ((StartTLSDelegate)castedAsyncResult.AsyncDelegate).EndInvoke(castedAsyncResult.AsyncResult);
            }
            else{
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginReset method.");
            }
        }

        #endregion

        #region method StartTLS

        /// <summary>
        /// Switches POP3 connection to SSL.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected or is authenticated or is already secure connection.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 server returns error.</exception>
        public void StartTLS()
        {
            /* RFC 2595 4. POP3 STARTTLS extension.
                Arguments: none

                Restrictions:
                    Only permitted in AUTHORIZATION state.
             
                Possible Responses:
                     +OK -ERR

                 Examples:
                     C: STLS
                     S: +OK Begin TLS negotiation
                     <TLS negotiation, further commands are under TLS layer>
                       ...
                     C: STLS
                     S: -ERR Command not permitted when TLS active
            */

            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
				throw new InvalidOperationException("You must connect first.");
			}
			if(this.IsAuthenticated){
				throw new InvalidOperationException("The STLS command is only valid in non-authenticated state.");
			}
            if(this.IsSecureConnection){
                throw new InvalidOperationException("Connection is already secure.");
            }
                        
            WriteLine("STLS");
                        
            string line = ReadLine();
			if(!line.ToUpper().StartsWith("+OK")){
				throw new POP3_ClientException(line);
			}

            this.SwitchToSecure();
        }

        #endregion

        #region method BeginAuthenticate

        /// <summary>
        /// Internal helper method for asynchronous Authenticate method.
        /// </summary>
        private delegate void AuthenticateDelegate(string userName,string password,bool tryApop);

        /// <summary>
        /// Starts authentication.
        /// </summary>
		/// <param name="userName">User login name.</param>
		/// <param name="password">Password.</param>
		/// <param name="tryApop"> If true and POP3 server supports APOP, then APOP is used, otherwise normal login used.</param>
        /// <param name="callback">Callback to call when the asynchronous operation is complete.</param>
        /// <param name="state">User data.</param>
        /// <returns>An IAsyncResult that references the asynchronous operation.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected or is already authenticated.</exception>
        public IAsyncResult BeginAuthenticate(string userName,string password,bool tryApop,AsyncCallback callback,object state)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
				throw new InvalidOperationException("You must connect first.");
			}
			if(this.IsAuthenticated){
				throw new InvalidOperationException("Session is already authenticated.");
			}

            AuthenticateDelegate asyncMethod = new AuthenticateDelegate(this.Authenticate);
            AsyncResultState asyncState = new AsyncResultState(this,asyncMethod,callback,state);
            asyncState.SetAsyncResult(asyncMethod.BeginInvoke(userName,password,tryApop,new AsyncCallback(asyncState.CompletedCallback),null));

            return asyncState;
        }

        #endregion

        #region method EndAuthenticate

        /// <summary>
        /// Ends a pending asynchronous authentication request.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>asyncResult</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when invalid <b>asyncResult</b> passed to this method.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 server returns error.</exception>
        public void EndAuthenticate(IAsyncResult asyncResult)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(asyncResult == null){
                throw new ArgumentNullException("asyncResult");
            }

            AsyncResultState castedAsyncResult = asyncResult as AsyncResultState;
            if(castedAsyncResult == null || castedAsyncResult.AsyncObject != this){
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginAuthenticate method.");
            }
            if(castedAsyncResult.IsEndCalled){
                throw new InvalidOperationException("BeginAuthenticate was previously called for the asynchronous connection.");
            }
             
            castedAsyncResult.IsEndCalled = true;
            if(castedAsyncResult.AsyncDelegate is AuthenticateDelegate){
                ((AuthenticateDelegate)castedAsyncResult.AsyncDelegate).EndInvoke(castedAsyncResult.AsyncResult);
            }
            else{
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginAuthenticate method.");
            }
        }

        #endregion

        #region method Authenticate

        /// <summary>
		/// Authenticates user.
		/// </summary>
		/// <param name="userName">User login name.</param>
		/// <param name="password">Password.</param>
		/// <param name="tryApop"> If true and POP3 server supports APOP, then APOP is used, otherwise normal login used.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected or is already authenticated.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 server returns error.</exception>
		public void Authenticate(string userName,string password,bool tryApop)
		{
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
				throw new InvalidOperationException("You must connect first.");
			}
			if(this.IsAuthenticated){
				throw new InvalidOperationException("Session is already authenticated.");
			}
            
			// Supports APOP, use it.
			if(tryApop && m_ApopHashKey.Length > 0){
                string hexHash = Core.ComputeMd5(m_ApopHashKey + password,true);
                                
				int countWritten = this.TcpStream.WriteLine("APOP " + userName + " " + hexHash);
                LogAddWrite(countWritten,"APOP " + userName + " " + hexHash);

                string line = this.ReadLine();
				if(line.StartsWith("+OK")){
					m_pAuthdUserIdentity = new GenericIdentity(userName,"apop");
				}
				else{
					throw new POP3_ClientException(line);
				}
			}
            // Use normal LOGIN, don't support APOP.
			else{                 
				int countWritten = this.TcpStream.WriteLine("USER " + userName);
                LogAddWrite(countWritten,"USER " + userName);

                string line = this.ReadLine();
				if(line.StartsWith("+OK")){                    
					countWritten = this.TcpStream.WriteLine("PASS " + password);
                    LogAddWrite(countWritten,"PASS <***REMOVED***>");

					line = this.ReadLine();
					if(line.StartsWith("+OK")){
						m_pAuthdUserIdentity = new GenericIdentity(userName,"pop3-user/pass");
					}
					else{
						throw new POP3_ClientException(line);
					}
				}
				else{
					throw new POP3_ClientException(line);
				}				
			}
		}

		#endregion


        #region method BeginNoop

        /// <summary>
        /// Internal helper method for asynchronous Noop method.
        /// </summary>
        private delegate void NoopDelegate();

        /// <summary>
        /// Starts sending NOOP command to server. This method can be used for keeping connection alive(not timing out).
        /// </summary>
        /// <param name="callback">Callback to call when the asynchronous operation is complete.</param>
        /// <param name="state">User data.</param>
        /// <returns>An IAsyncResult that references the asynchronous operation.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected.</exception>
        public IAsyncResult BeginNoop(AsyncCallback callback,object state)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
				throw new InvalidOperationException("You must connect first.");
			}

            NoopDelegate asyncMethod = new NoopDelegate(this.Noop);
            AsyncResultState asyncState = new AsyncResultState(this,asyncMethod,callback,state);
            asyncState.SetAsyncResult(asyncMethod.BeginInvoke(new AsyncCallback(asyncState.CompletedCallback),null));

            return asyncState;
        }

        #endregion

        #region method EndNoop

        /// <summary>
        /// Ends a pending asynchronous Noop request.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>asyncResult</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when invalid <b>asyncResult</b> passed to this method.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 server returns error.</exception>
        public void EndNoop(IAsyncResult asyncResult)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(asyncResult == null){
                throw new ArgumentNullException("asyncResult");
            }

            AsyncResultState castedAsyncResult = asyncResult as AsyncResultState;
            if(castedAsyncResult == null || castedAsyncResult.AsyncObject != this){
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginNoop method.");
            }
            if(castedAsyncResult.IsEndCalled){
                throw new InvalidOperationException("BeginNoop was previously called for the asynchronous connection.");
            }
             
            castedAsyncResult.IsEndCalled = true;
            if(castedAsyncResult.AsyncDelegate is NoopDelegate){
                ((NoopDelegate)castedAsyncResult.AsyncDelegate).EndInvoke(castedAsyncResult.AsyncResult);
            }
            else{
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginNoop method.");
            }
        }

        #endregion

        #region method Noop

        /// <summary>
        /// Send NOOP command to server. This method can be used for keeping connection alive(not timing out).
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 server returns error.</exception>
        public void Noop()
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
                throw new InvalidOperationException("You must connect first.");
            }
			if(!this.IsAuthenticated){
				throw new InvalidOperationException("The NOOP command is only valid in TRANSACTION state.");
			}

            /* RFC 1939 5 NOOP.
                Arguments: none

                Restrictions:
                    may only be given in the TRANSACTION state

                Discussion:
                    The POP3 server does nothing, it merely replies with a
                    positive response.

                Possible Responses:
                    +OK

                Examples:
                    C: NOOP
                    S: +OK
            */

            WriteLine("NOOP");

			string line = ReadLine();
			if(!line.ToUpper().StartsWith("+OK")){
				throw new POP3_ClientException(line);
			}
        }

        #endregion

        #region method BeginReset

        /// <summary>
        /// Internal helper method for asynchronous Reset method.
        /// </summary>
        private delegate void ResetDelegate();

        /// <summary>
        /// Starts resetting session. Messages marked for deletion will be unmarked.
        /// </summary>
        /// <param name="callback">Callback to call when the asynchronous operation is complete.</param>
        /// <param name="state">User data.</param>
        /// <returns>An IAsyncResult that references the asynchronous operation.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected and authenticated.</exception>
        public IAsyncResult BeginReset(AsyncCallback callback,object state)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
				throw new InvalidOperationException("You must connect first.");
			}
			if(!this.IsAuthenticated){
				throw new InvalidOperationException("The RSET command is only valid in authenticated state.");
			}

            ResetDelegate asyncMethod = new ResetDelegate(this.Reset);
            AsyncResultState asyncState = new AsyncResultState(this,asyncMethod,callback,state);
            asyncState.SetAsyncResult(asyncMethod.BeginInvoke(new AsyncCallback(asyncState.CompletedCallback),null));

            return asyncState;
        }

        #endregion

        #region method EndReset

        /// <summary>
        /// Ends a pending asynchronous reset request.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that stores state information and any user defined data for this asynchronous operation.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when <b>asyncResult</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when invalid <b>asyncResult</b> passed to this method.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 server returns error.</exception>
        public void EndReset(IAsyncResult asyncResult)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(asyncResult == null){
                throw new ArgumentNullException("asyncResult");
            }

            AsyncResultState castedAsyncResult = asyncResult as AsyncResultState;
            if(castedAsyncResult == null || castedAsyncResult.AsyncObject != this){
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginReset method.");
            }
            if(castedAsyncResult.IsEndCalled){
                throw new InvalidOperationException("BeginReset was previously called for the asynchronous connection.");
            }
             
            castedAsyncResult.IsEndCalled = true;
            if(castedAsyncResult.AsyncDelegate is ResetDelegate){
                ((ResetDelegate)castedAsyncResult.AsyncDelegate).EndInvoke(castedAsyncResult.AsyncResult);
            }
            else{
                throw new ArgumentException("Argument asyncResult was not returned by a call to the BeginReset method.");
            }
        }

        #endregion

        #region method Reset

        /// <summary>
		/// Resets session. Messages marked for deletion will be unmarked.
		/// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when POP3 client is not connected and authenticated.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 server returns error.</exception>
		public void Reset()
		{
			if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(!this.IsConnected){
				throw new InvalidOperationException("You must connect first.");
			}
			if(!this.IsAuthenticated){
				throw new InvalidOperationException("The RSET command is only valid in TRANSACTION state.");
			}

            /* RFC 1939 5. RSET.
                Arguments: none

                Restrictions:
                    may only be given in the TRANSACTION state

                Discussion:
                    If any messages have been marked as deleted by the POP3
                    server, they are unmarked.  The POP3 server then replies
                    with a positive response.

                Possible Responses:
                    +OK

                Examples:
                    C: RSET
                    S: +OK maildrop has 2 messages (320 octets)
			*/

			WriteLine("RSET");            

			// Read first line of reply, check if it's ok.
			string line = ReadLine();
			if(!line.StartsWith("+OK")){
				throw new POP3_ClientException(line);
			}
       
            foreach(POP3_ClientMessage message in m_pMessages){
                message.SetMarkedForDeletion(false);
            }
		}

		#endregion


        #region override method OnConnected

        /// <summary>
        /// This method is called after TCP client has sucessfully connected.
        /// </summary>
        protected override void OnConnected()
        {
            // Read first line of reply, check if it's ok.
            string line = ReadLine();
	        if(line.ToUpper().StartsWith("+OK")){
                m_GreetingText = line.Substring(3).Trim();

			    // Try to read APOP hash key, if supports APOP.
				if(line.IndexOf("<") > -1 && line.IndexOf(">") > -1){
					m_ApopHashKey = line.Substring(line.IndexOf("<"),line.LastIndexOf(">") - line.IndexOf("<") + 1);
				}
			}
            else{
                throw new POP3_ClientException(line);
            }
            

            /* Try to get POP3 server supported capabilities, if command not supported, just skip tat command.
             
               RFC 2449 CAPA
                Arguments:
                    none

                Restrictions:
                    none

                Discussion:
                    An -ERR response indicates the capability command is not
                    implemented and the client will have to probe for
                    capabilities as before.

                    An +OK response is followed by a list of capabilities, one
                    per line.  Each capability name MAY be followed by a single
                    space and a space-separated list of parameters.  Each
                    capability line is limited to 512 octets (including the
                    CRLF).  The capability list is terminated by a line
                    containing a termination octet (".") and a CRLF pair.

                Possible Responses:
                    +OK -ERR

                Examples:
                    C: CAPA
                    S: +OK Capability list follows
                    S: TOP
                    S: USER
                    S: SASL CRAM-MD5 KERBEROS_V4
                    S: RESP-CODES
                    S: LOGIN-DELAY 900
                    S: PIPELINING
                    S: EXPIRE 60
                    S: UIDL
                    S: IMPLEMENTATION Shlemazle-Plotz-v302
                    S: .
            */

            WriteLine("CAPA");

            // Read server response.
            line = ReadLine();

            // CAPA command supported, read capabilities.
            if(line.ToUpper().StartsWith("+OK")){
                while(true){
                    line = ReadLine();
                    
                    // End of list reached.
                    if(line == "."){
                        break;
                    }
                    else{
                        if(!m_pExtCapabilities.Contains(line.ToUpper())){
                            m_pExtCapabilities.Add(line.ToUpper());
                        }
                    }
                }
            }
            else{
                // CAPA command not supported, so skip it.
            }
        }

        #endregion


        #region method MarkMessageForDeletion

        /// <summary>
        /// Marks specified message for deletion.
        /// </summary>
        /// <param name="sequenceNumber">Message sequence number.</param>
        internal void MarkMessageForDeletion(int sequenceNumber)
        {
            WriteLine("DELE " + sequenceNumber.ToString());

			// Read first line of reply, check if it's ok.
			string line = ReadLine();
			if(!line.StartsWith("+OK")){
				throw new POP3_ClientException(line);
			}
        }

        #endregion

        #region method GetMessage

        /// <summary>
        /// Stores specified message to the specified stream.
        /// </summary>
        /// <param name="sequenceNumber">Message 1 based sequence number.</param>
        /// <param name="stream">Stream where to store message.</param>
        public void GetMessage(int sequenceNumber,Stream stream)
        {
            WriteLine("RETR " + sequenceNumber.ToString());

			// Read first line of reply, check if it's ok.
			string line = ReadLine();
			if(line.StartsWith("+OK")){    
                SmartStream.ReadPeriodTerminatedAsyncOP readTermOP = new SmartStream.ReadPeriodTerminatedAsyncOP(stream,999999999,SizeExceededAction.ThrowException);
                this.TcpStream.ReadPeriodTerminated(readTermOP,false);
                if(readTermOP.Error != null){
                    throw readTermOP.Error;
                }
                LogAddWrite(readTermOP.BytesStored,"Readed " + readTermOP.BytesStored.ToString() + " bytes.");
			}
			else{
				throw new POP3_ClientException(line);
			}
        }

        #endregion

        #region method GetTopOfMessage

        /// <summary>
        /// Stores specified message header + specified lines of body to the specified stream.
        /// </summary>
        /// <param name="sequenceNumber">Message 1 based sequence number.</param>
        /// <param name="stream">Stream where to store data.</param>
        /// <param name="lineCount">Number of lines of message body to get.</param>
        internal void GetTopOfMessage(int sequenceNumber,Stream stream,int lineCount)
        {
            this.TcpStream.WriteLine("TOP " + sequenceNumber.ToString() + " " + lineCount.ToString());

			// Read first line of reply, check if it's ok.
			string line = ReadLine();
			if(line.StartsWith("+OK")){
                SmartStream.ReadPeriodTerminatedAsyncOP readTermOP = new SmartStream.ReadPeriodTerminatedAsyncOP(stream,999999999,SizeExceededAction.ThrowException);
                this.TcpStream.ReadPeriodTerminated(readTermOP,false);
                if(readTermOP.Error != null){
                    throw readTermOP.Error;
                }
                LogAddWrite(readTermOP.BytesStored,"Readed " + readTermOP.BytesStored.ToString() + " bytes.");
			}
			else{
				throw new POP3_ClientException(line);
			}
        }

        #endregion
        
        #region method FillMessages

        /// <summary>
        /// Fills messages info.
        /// </summary>
        public POP3_ClientMessageCollection List()
        {
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException(this.GetType().Name);
			}
			if (!this.IsConnected)
			{
				throw new InvalidOperationException("You must connect first.");
			}
			if (!this.IsAuthenticated)
			{
				throw new InvalidOperationException("You must authenticate first.");
			}			

            m_pMessages = new POP3_ClientMessageCollection(this);

            /*
                First make messages info, then try to add UIDL if server supports.
            */
                                   
			/* NOTE: If reply is +OK, this is multiline respone and is terminated with '.'.
			Examples:
				C: LIST
				S: +OK 2 messages (320 octets)
				S: 1 120				
				S: 2 200
				S: .
				...
				C: LIST 3
				S: -ERR no such message, only 2 messages in maildrop
			*/
                        
            WriteLine("LIST");

			// Read first line of reply, check if it's ok.
			string line = ReadLine();
			if(line.StartsWith("+OK")){
				// Read lines while get only '.' on line itshelf.
				while(true){
					line = ReadLine();

					// End of data
					if(line.Trim() == "."){
						break;
					}
					else{
                        string[] no_size = line.Trim().Split(new char[]{' '});
                        m_pMessages.Add(Convert.ToInt32(no_size[1]));
					}
				}
			}
			else{
				throw new POP3_ClientException(line);
			}

            // Try to fill messages UIDs.
            /* NOTE: If reply is +OK, this is multiline respone and is terminated with '.'.
			Examples:
				C: UIDL
				S: +OK
				S: 1 whqtswO00WBw418f9t5JxYwZ
				S: 2 QhdPYR:00WBw1Ph7x7
				S: .
				...
				C: UIDL 3
				S: -ERR no such message
			*/

            WriteLine("UIDL");

			// Read first line of reply, check if it's ok
			line = ReadLine();
			if(line.StartsWith("+OK")){
                m_IsUidlSupported = true;

				// Read lines while get only '.' on line itshelf.
				while(true){
					line = ReadLine();

					// End of data
					if(line.Trim() == "."){
						break;
					}
					else{
                        string[] no_uid = line.Trim().Split(new char[]{' '});                        
                        m_pMessages[Convert.ToInt32(no_uid[0]) - 1].SetUID(no_uid[1]);
					}
				}
			}
			else{
				m_IsUidlSupported = false;
			}

			return m_pMessages;
        }

        #endregion


        #region Properties Implementation
        
        /// <summary>
        /// Gets greeting text which was sent by POP3 server.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when this property is accessed and POP3 client is not connected.</exception>
        public string GreetingText
        {
            get{ 
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                if(!this.IsConnected){
                    throw new InvalidOperationException("You must connect first.");
                }

                return m_GreetingText; 
            }
        }

        /// <summary>
        /// Gets POP3 exteneded capabilities supported by POP3 server.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when this property is accessed and POP3 client is not connected.</exception>
        [Obsolete("USe ExtendedCapabilities instead !")]
        public string[] ExtenededCapabilities
        {
            get{ 
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                if(!this.IsConnected){
				    throw new InvalidOperationException("You must connect first.");
			    }

                return m_pExtCapabilities.ToArray(); 
            }
        }

        /// <summary>
        /// Gets POP3 exteneded capabilities supported by POP3 server.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when this property is accessed and POP3 client is not connected.</exception>
        public string[] ExtendedCapabilities
        {
            get{ 
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                if(!this.IsConnected){
				    throw new InvalidOperationException("You must connect first.");
			    }

                return m_pExtCapabilities.ToArray(); 
            }
        }

        /// <summary>
        /// Gets if POP3 server supports UIDL command.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when this property is accessed and 
        /// POP3 client is not connected and authenticated.</exception>
        public bool IsUidlSupported
        {
            get{ 
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                if(!this.IsConnected){
				    throw new InvalidOperationException("You must connect first.");
			    }
                if(!this.IsAuthenticated){
				    throw new InvalidOperationException("You must authenticate first.");
			    }

                return m_IsUidlSupported; 
            }
        }

        /// <summary>
        /// Gets session authenticated user identity, returns null if not authenticated.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when this property is accessed and POP3 client is not connected.</exception>
        public override GenericIdentity AuthenticatedUserIdentity
        {
            get{ 
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                if(!this.IsConnected){
				    throw new InvalidOperationException("You must connect first.");
			    }

                return m_pAuthdUserIdentity; 
            }
        }

		#endregion

	}
}
