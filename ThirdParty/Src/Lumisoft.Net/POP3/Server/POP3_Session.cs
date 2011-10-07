using System;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

using LumiSoft.Net;
using LumiSoft.Net.AUTH;

namespace LumiSoft.Net.POP3.Server
{
	/// <summary>
	/// POP3 Session.
	/// </summary>
	public class POP3_Session : SocketServerSession
	{	        
		private POP3_Server            m_pServer        = null;  
		private string                 m_UserName       = "";    // Holds USER command value
		private string                 m_MD5_prefix     = "";    // Session MD5 prefix for APOP command
		private int                    m_BadCmdCount    = 0;     // Holds number of bad commands.
		private POP3_MessageCollection m_POP3_Messages  = null;
		
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="sessionID">Session ID.</param>
        /// <param name="socket">Server connected socket.</param>
        /// <param name="bindInfo">BindInfo what accepted socket.</param>
        /// <param name="server">Reference to server.</param>
        internal POP3_Session(string sessionID,SocketEx socket,IPBindInfo bindInfo,POP3_Server server) : base(sessionID,socket,bindInfo,server)
        {
            m_pServer       = server;
            m_POP3_Messages = new POP3_MessageCollection();

            // Start session proccessing
			StartSession();
        }


		#region method StartSession

		/// <summary>
		/// Starts session.
		/// </summary>
		private void StartSession()
		{
			// Add session to session list
			m_pServer.AddSession(this);
	
			try{
				// Check if ip is allowed to connect this computer
				if(m_pServer.OnValidate_IpAddress(this.LocalEndPoint,this.RemoteEndPoint)){
                    //--- Dedicated SSL connection, switch to SSL -----------------------------------//
                    if(this.BindInfo.SslMode == SslMode.SSL){
                        try{
                            this.Socket.SwitchToSSL(this.BindInfo.Certificate);

                            if(this.Socket.Logger != null){
                                this.Socket.Logger.AddTextEntry("SSL negotiation completed successfully.");
                            }
                        }
                        catch(Exception x){
                            if(this.Socket.Logger != null){
                                this.Socket.Logger.AddTextEntry("SSL handshake failed ! " + x.Message);

                                EndSession();
                                return;
                            }
                        }
                    }
                    //-------------------------------------------------------------------------------//

					// Notify that server is ready
					m_MD5_prefix = "<" + Guid.NewGuid().ToString().ToLower() + ">";
                    if(m_pServer.GreetingText == ""){
					    this.Socket.WriteLine("+OK " + Net_Utils.GetLocalHostName(this.BindInfo.HostName) + " POP3 Server ready " + m_MD5_prefix);
                    }
                    else{
                        this.Socket.WriteLine("+OK " + m_pServer.GreetingText + " " + m_MD5_prefix);
                    }

					BeginRecieveCmd();
				}
				else{
					EndSession();
				}
			}
			catch(Exception x){
				OnError(x);
			}
		}

		#endregion

		#region method EndSession

		/// <summary>
		/// Ends session, closes socket.
		/// </summary>
		private void EndSession()
		{          
			try{
				// Write logs to log file, if needed
				if(m_pServer.LogCommands){
					this.Socket.Logger.Flush();
				}

				if(this.Socket != null){
					this.Socket.Shutdown(SocketShutdown.Both);
					this.Socket.Disconnect();
					//this.Socket = null;
				}
			}
			catch{ // We don't need to check errors here, because they only may be Socket closing errors.
			}
			finally{
				m_pServer.RemoveSession(this);
			}
		}

		#endregion


        #region method Kill

        /// <summary>
        /// Kill this session.
        /// </summary>
        public override void Kill()
        {
            EndSession();
        }

        #endregion

        #region method OnSessionTimeout

        /// <summary>
		/// Is called by server when session has timed out.
		/// </summary>
		internal protected override void OnSessionTimeout()
		{
			try{
				this.Socket.WriteLine("-ERR Session timeout, closing transmission channel");
			}
			catch{
			}

			EndSession();
		}

		#endregion

		#region method OnError

		/// <summary>
		/// Is called when error occures.
		/// </summary>
		/// <param name="x"></param>
		private void OnError(Exception x)
		{
			try{
                // We must see InnerException too, SocketException may be as inner exception.
                SocketException socketException = null;
                if(x is SocketException){
                    socketException = (SocketException)x;
                }
                else if(x.InnerException != null && x.InnerException is SocketException){
                    socketException = (SocketException)x.InnerException;
                }

				if(socketException != null){
					// Client disconnected without shutting down
					if(socketException.ErrorCode == 10054 || socketException.ErrorCode == 10053){
						if(m_pServer.LogCommands){
							this.Socket.Logger.AddTextEntry("Client aborted/disconnected");
						}

						EndSession();

						// Exception handled, return
						return;
					}
				}

                m_pServer.OnSysError("",x);                
			}
			catch(Exception ex){
				m_pServer.OnSysError("",ex);
			}
		}

		#endregion


		#region method BeginRecieveCmd
		
		/// <summary>
		/// Starts recieveing command.
		/// </summary>
		private void BeginRecieveCmd()
		{
			MemoryStream strm = new MemoryStream();
			this.Socket.BeginReadLine(strm,1024,strm,new SocketCallBack(this.EndRecieveCmd));
		}

		#endregion

		#region method EndRecieveCmd

		/// <summary>
		/// Is called if command is recieved.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="exception"></param>
		/// <param name="count"></param>
		/// <param name="tag"></param>
		private void EndRecieveCmd(SocketCallBackResult result,long count,Exception exception,object tag)
		{
			try{
				switch(result)
				{
					case SocketCallBackResult.Ok:
						MemoryStream strm = (MemoryStream)tag;

						string cmdLine = System.Text.Encoding.Default.GetString(strm.ToArray());

						// Exceute command
						if(SwitchCommand(cmdLine)){
							// Session end, close session
							EndSession();
						}
						break;

					case SocketCallBackResult.LengthExceeded:
						this.Socket.WriteLine("-ERR Line too long.");

						BeginRecieveCmd();
						break;

					case SocketCallBackResult.SocketClosed:
						EndSession();
						break;

					case SocketCallBackResult.Exception:
						OnError(exception);
						break;
				}
			}
			catch(Exception x){
				 OnError(x);
			}
		}

		#endregion
		
		
		#region method SwitchCommand

		/// <summary>
		/// Parses and executes POP3 commmand.
		/// </summary>
		/// <param name="POP3_commandTxt">POP3 command text.</param>
		/// <returns>Returns true,if session must be terminated.</returns>
		private bool SwitchCommand(string POP3_commandTxt)
		{
			//---- Parse command --------------------------------------------------//
			string[] cmdParts = POP3_commandTxt.TrimStart().Split(new char[]{' '});
			string POP3_command = cmdParts[0].ToUpper().Trim();
			string argsText = Core.GetArgsText(POP3_commandTxt,POP3_command);
			//---------------------------------------------------------------------//

			bool getNextCmd = true;

			switch(POP3_command)
			{
				case "USER":
					USER(argsText);
					getNextCmd = false;
					break;

				case "PASS":
					PASS(argsText);
					getNextCmd = false;
					break;
					
				case "STAT":
					STAT();
					getNextCmd = false;
					break;

				case "LIST":
					LIST(argsText);
					getNextCmd = false;
					break;

				case "RETR":					
					RETR(argsText);
					getNextCmd = false;
					break;

				case "DELE":
					DELE(argsText);
					getNextCmd = false;
					break;

				case "NOOP":
					NOOP();
					getNextCmd = false;
					break;

				case "RSET":
					RSET();
					getNextCmd = false;
					break;

				case "QUIT":
					QUIT();
					getNextCmd = false;
					return true;


				//----- Optional commands ----- //
				case "UIDL":
					UIDL(argsText);
					getNextCmd = false;
					break;

				case "APOP":
					APOP(argsText);
					getNextCmd = false;
					break;

				case "TOP":
					TOP(argsText);
					getNextCmd = false;
					break;

				case "AUTH":
					AUTH(argsText);
					getNextCmd = false;
					break;

				case "CAPA":
					CAPA(argsText);
					getNextCmd = false;
					break;

                case "STLS":
					STLS(argsText);
					getNextCmd = false;
					break;
										
				default:					
					this.Socket.WriteLine("-ERR Invalid command");

					//---- Check that maximum bad commands count isn't exceeded ---------------//
					if(m_BadCmdCount > m_pServer.MaxBadCommands-1){
						this.Socket.WriteLine("-ERR Too many bad commands, closing transmission channel");
						return true;
					}
					m_BadCmdCount++;
					//-------------------------------------------------------------------------//
					break;				
			}
			
			if(getNextCmd){
				BeginRecieveCmd();
			}
						
			return false;
		}

		#endregion


		#region method USER

		private void USER(string argsText)
		{
			/* RFC 1939 7. USER
			Arguments:
				a string identifying a mailbox (required), which is of
				significance ONLY to the server
				
			NOTE:
				If the POP3 server responds with a positive
				status indicator ("+OK"), then the client may issue
				either the PASS command to complete the authentication,
				or the QUIT command to terminate the POP3 session.
			 
			*/

			if(this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You are already authenticated",new SocketCallBack(this.EndSend));
				return;
			}
			if(m_UserName.Length > 0){
				this.Socket.BeginWriteLine("-ERR username is already specified, please specify password",new SocketCallBack(this.EndSend));
				return;
			}
            if((m_pServer.SupportedAuthentications & SaslAuthTypes.Plain) == 0){
                this.Socket.BeginWriteLine("-ERR USER/PASS command disabled",new SocketCallBack(this.EndSend));
				return;
            }

			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);

			// There must be only one parameter - userName
			if(argsText.Length > 0 && param.Length == 1){
				string userName = param[0];
							
				// Check if user isn't logged in already
				if(!m_pServer.IsUserLoggedIn(userName)){
					m_UserName = userName;

					// Send this line last, because it issues a new command and any assignments done
					// after this method may not become wisible to next command.
					this.Socket.BeginWriteLine("+OK User:'" + userName + "' ok",new SocketCallBack(this.EndSend));					
				}
				else{
					this.Socket.BeginWriteLine("-ERR User:'" + userName + "' already logged in",new SocketCallBack(this.EndSend));
				}
			}
			else{
				this.Socket.BeginWriteLine("-ERR Syntax error. Syntax:{USER username}",new SocketCallBack(this.EndSend));
			}
		}

		#endregion

		#region method PASS

		private void PASS(string argsText)
		{	
			/* RFC 7. PASS
			Arguments:
				a server/mailbox-specific password (required)
				
			Restrictions:
				may only be given in the AUTHORIZATION state immediately
				after a successful USER command
				
			NOTE:
				When the client issues the PASS command, the POP3 server
				uses the argument pair from the USER and PASS commands to
				determine if the client should be given access to the
				appropriate maildrop.
				
			Possible Responses:
				+OK maildrop locked and ready
				-ERR invalid password
				-ERR unable to lock maildrop
						
			*/

			if(this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You are already authenticated",new SocketCallBack(this.EndSend));
				return;
			}
			if(m_UserName.Length == 0){
				this.Socket.BeginWriteLine("-ERR please specify username first",new SocketCallBack(this.EndSend));
				return;
			}            
            if((m_pServer.SupportedAuthentications & SaslAuthTypes.Plain) == 0){
                this.Socket.BeginWriteLine("-ERR USER/PASS command disabled",new SocketCallBack(this.EndSend));
				return;
            }

			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);

			// There may be only one parameter - password
			if(param.Length == 1){
				string password = param[0];
									
				// Authenticate user
				AuthUser_EventArgs aArgs = m_pServer.OnAuthUser(this,m_UserName,password,"",AuthType.Plain);

                // There is custom error, return it
                if(aArgs.ErrorText != null){
                    this.Socket.BeginWriteLine("-ERR " + aArgs.ErrorText,new SocketCallBack(this.EndSend));
                    return;
                }
                
				if(aArgs.Validated){
	                this.SetUserName(m_UserName);

					// Get user messages info.
					m_pServer.OnGetMessagesInfo(this,m_POP3_Messages);

					this.Socket.BeginWriteLine("+OK Password ok",new SocketCallBack(this.EndSend));
				}
				else{						
					this.Socket.BeginWriteLine("-ERR UserName or Password is incorrect",new SocketCallBack(this.EndSend));					
					m_UserName = ""; // Reset userName !!!
				}
			}
			else{
				this.Socket.BeginWriteLine("-ERR Syntax error. Syntax:{PASS userName}",new SocketCallBack(this.EndSend));
			}
		}

		#endregion

		#region method STAT

		private void STAT()
		{	
			/* RFC 1939 5. STAT
			NOTE:
				The positive response consists of "+OK" followed by a single
				space, the number of messages in the maildrop, a single
				space, and the size of the maildrop in octets.
				
				Note that messages marked as deleted are not counted in
				either total.
			 
			Example:
				C: STAT
				S: +OK 2 320
			*/

			if(!this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You must authenticate first",new SocketCallBack(this.EndSend));
				return;
			}
		
			this.Socket.BeginWriteLine("+OK " + m_POP3_Messages.Count.ToString() + " " + m_POP3_Messages.GetTotalMessagesSize(),new SocketCallBack(this.EndSend));			
		}

		#endregion

		#region method LIST

		private void LIST(string argsText)
		{	
			/* RFC 1939 5. LIST
			Arguments:
				a message-number (optional), which, if present, may NOT
				refer to a message marked as deleted
			 
			NOTE:
				If an argument was given and the POP3 server issues a
				positive response with a line containing information for
				that message.

				If no argument was given and the POP3 server issues a
				positive response, then the response given is multi-line.
				
				Note that messages marked as deleted are not listed.
			
			Examples:
				C: LIST
				S: +OK 2 messages (320 octets)
				S: 1 120				
				S: 2 200
				S: .
				...
				C: LIST 2
				S: +OK 2 200
				...
				C: LIST 3
				S: -ERR no such message, only 2 messages in maildrop
			 
			*/

			if(!this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You must authenticate first",new SocketCallBack(this.EndSend));
				return;
			}

			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);

			// Argument isn't specified, multiline response.
			if(argsText.Length == 0){
                StringBuilder reply = new StringBuilder();
				reply.Append("+OK " + m_POP3_Messages.Count.ToString() + " messages\r\n");

				// Send message number and size for each message
                for(int i=0;i<m_POP3_Messages.Count;i++){
                    POP3_Message message = m_POP3_Messages[i];
                    if(!message.MarkedForDelete){
                        reply.Append((i + 1).ToString() + " " + message.Size + "\r\n");
                    }
                }

				// ".<CRLF>" - means end of list
				reply.Append(".\r\n");

				this.Socket.BeginWriteLine(reply.ToString(),null,new SocketCallBack(this.EndSend));
			}
			else{
				// If parameters specified,there may be only one parameter - messageNr
				if(param.Length == 1){
					// Check if messageNr is valid
					if(Core.IsNumber(param[0])){
						int messageNr = Convert.ToInt32(param[0]);
						if(m_POP3_Messages.MessageExists(messageNr)){
							POP3_Message msg = m_POP3_Messages[messageNr - 1];

							this.Socket.BeginWriteLine("+OK " + messageNr.ToString() + " " + msg.Size,new SocketCallBack(this.EndSend));
						}
						else{
							this.Socket.BeginWriteLine("-ERR no such message, or marked for deletion",new SocketCallBack(this.EndSend));
						}
					}
					else{
						this.Socket.BeginWriteLine("-ERR message-number is invalid",new SocketCallBack(this.EndSend));
					}
				}
				else{
					this.Socket.BeginWriteLine("-ERR Syntax error. Syntax:{LIST [messageNr]}",new SocketCallBack(this.EndSend));
				}
			}
		}

		#endregion

		#region method RETR
		
		private void RETR(string argsText)
		{
			/* RFC 1939 5. RETR
			Arguments:
				a message-number (required) which may NOT refer to a
				message marked as deleted
			 
			NOTE:
				If the POP3 server issues a positive response, then the
				response given is multi-line.  After the initial +OK, the
				POP3 server sends the message corresponding to the given
				message-number, being careful to byte-stuff the termination
				character (as with all multi-line responses).
				
			Example:
				C: RETR 1
				S: +OK 120 octets
				S: <the POP3 server sends the entire message here>
				S: .
			
			*/

			if(!this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You must authenticate first",new SocketCallBack(this.EndSend));
			}
	
			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);

			// There must be only one parameter - messageNr
			if(argsText.Length > 0 && param.Length == 1){
				// Check if messageNr is valid
				if(Core.IsNumber(param[0])){
					int messageNr = Convert.ToInt32(param[0]);					
					if(m_POP3_Messages.MessageExists(messageNr)){
						POP3_Message msg = m_POP3_Messages[messageNr - 1];
						
                        // Raise Event, request message
						POP3_eArgs_GetMessageStream eArgs = m_pServer.OnGetMessageStream(this,msg);		
						if(eArgs.MessageExists && eArgs.MessageStream != null){
                            this.Socket.WriteLine("+OK " + eArgs.MessageSize + " octets");
                                                        
							// Send message asynchronously to client
							this.Socket.BeginWritePeriodTerminated(eArgs.MessageStream,eArgs.CloseMessageStream,null,new SocketCallBack(this.EndSend));
						}
						else{									
							this.Socket.BeginWriteLine("-ERR no such message",new SocketCallBack(this.EndSend));
						}
					}
					else{
						this.Socket.BeginWriteLine("-ERR no such message",new SocketCallBack(this.EndSend));
					}
				}
				else{
					this.Socket.BeginWriteLine("-ERR message-number is invalid",new SocketCallBack(this.EndSend));
				}
			}
			else{
				this.Socket.BeginWriteLine("-ERR Syntax error. Syntax:{RETR messageNr}",new SocketCallBack(this.EndSend));
			}
		}

		#endregion

		#region method DELE

		private void DELE(string argsText)
		{	
			/* RFC 1939 5. DELE
			Arguments:
				a message-number (required) which may NOT refer to a
				message marked as deleted
			 
			NOTE:
				The POP3 server marks the message as deleted.  Any future
				reference to the message-number associated with the message
				in a POP3 command generates an error.  The POP3 server does
				not actually delete the message until the POP3 session
				enters the UPDATE state.
			*/

			if(!this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You must authenticate first",new SocketCallBack(this.EndSend));
				return;
			}

			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);

			// There must be only one parameter - messageNr
			if(argsText.Length > 0 && param.Length == 1){
				// Check if messageNr is valid
				if(Core.IsNumber(param[0])){
					int nr = Convert.ToInt32(param[0]);					
					if(m_POP3_Messages.MessageExists(nr)){
						POP3_Message msg = m_POP3_Messages[nr - 1];
						msg.MarkedForDelete = true;

						this.Socket.BeginWriteLine("+OK marked for delete",new SocketCallBack(this.EndSend));
					}
					else{
						this.Socket.BeginWriteLine("-ERR no such message",new SocketCallBack(this.EndSend));
					}
				}
				else{
					this.Socket.BeginWriteLine("-ERR message-number is invalid",new SocketCallBack(this.EndSend));
				}
			}
			else{
				this.Socket.BeginWriteLine("-ERR Syntax error. Syntax:{DELE messageNr}",new SocketCallBack(this.EndSend));
			}
		}

		#endregion

		#region method NOOP

		private void NOOP()
		{
			/* RFC 1939 5. NOOP
			NOTE:
				The POP3 server does nothing, it merely replies with a
				positive response.
			*/

			if(!this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You must authenticate first",new SocketCallBack(this.EndSend));
				return;
			}

			this.Socket.BeginWriteLine("+OK",new SocketCallBack(this.EndSend));
		}

		#endregion

		#region method RSET

		private void RSET()
		{
			/* RFC 1939 5. RSET
			Discussion:
				If any messages have been marked as deleted by the POP3
				server, they are unmarked.  The POP3 server then replies
				with a positive response.
			*/

			if(!this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You must authenticate first",new SocketCallBack(this.EndSend));
				return;
			}

			Reset();

			// Raise SessionResetted event
			m_pServer.OnSessionResetted(this);

			this.Socket.BeginWriteLine("+OK",new SocketCallBack(this.EndSend));
		}

		#endregion

		#region method QUIT

		private void QUIT()
		{
			/* RFC 1939 6. QUIT
			NOTE:
				The POP3 server removes all messages marked as deleted
				from the maildrop and replies as to the status of this
				operation.  If there is an error, such as a resource
				shortage, encountered while removing messages, the
				maildrop may result in having some or none of the messages
				marked as deleted be removed.  In no case may the server
				remove any messages not marked as deleted.

				Whether the removal was successful or not, the server
				then releases any exclusive-access lock on the maildrop
				and closes the TCP connection.
			*/					
			Update();

			this.Socket.WriteLine("+OK POP3 server signing off");			
		}

		#endregion


		//--- Optional commands

		#region method TOP

		private void TOP(string argsText)
		{		
			/* RFC 1939 7. TOP
			Arguments:
				a message-number (required) which may NOT refer to to a
				message marked as deleted, and a non-negative number
				of lines (required)
		
			NOTE:
				If the POP3 server issues a positive response, then the
				response given is multi-line.  After the initial +OK, the
				POP3 server sends the headers of the message, the blank
				line separating the headers from the body, and then the
				number of lines of the indicated message's body, being
				careful to byte-stuff the termination character (as with
				all multi-line responses).
			
			Examples:
				C: TOP 1 10
				S: +OK
				S: <the POP3 server sends the headers of the
					message, a blank line, and the first 10 lines
					of the body of the message>
				S: .
                ...
				C: TOP 100 3
				S: -ERR no such message
			 
			*/

			if(!this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You must authenticate first",new SocketCallBack(this.EndSend));
			}

			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);
			
			// There must be at two parameters - messageNr and nrLines
			if(param.Length == 2){
				// Check if messageNr and nrLines is valid
				if(Core.IsNumber(param[0]) && Core.IsNumber(param[1])){
					int messageNr = Convert.ToInt32(param[0]);
					if(m_POP3_Messages.MessageExists(messageNr)){
						POP3_Message msg = m_POP3_Messages[messageNr - 1];

						byte[] lines = m_pServer.OnGetTopLines(this,msg,Convert.ToInt32(param[1]));
						if(lines != null){
                            this.Socket.WriteLine("+OK " + lines.Length + " octets");

							// Send message asynchronously to client
							this.Socket.BeginWritePeriodTerminated(new MemoryStream(lines),null,new SocketCallBack(this.EndSend));
						}
						else{
							this.Socket.BeginWriteLine("-ERR no such message",new SocketCallBack(this.EndSend));
						}
					}
					else{
						this.Socket.BeginWriteLine("-ERR no such message",new SocketCallBack(this.EndSend));
					}
				}
				else{
					this.Socket.BeginWriteLine("-ERR message-number or number of lines is invalid",new SocketCallBack(this.EndSend));
				}
			}
			else{
				this.Socket.BeginWriteLine("-ERR Syntax error. Syntax:{TOP messageNr nrLines}",new SocketCallBack(this.EndSend));
			}
		}

		#endregion

		#region method UIDL

		private void UIDL(string argsText)
		{
			/* RFC 1939 UIDL [msg]
			Arguments:
			    a message-number (optional), which, if present, may NOT
				refer to a message marked as deleted
				
			NOTE:
				If an argument was given and the POP3 server issues a positive
				response with a line containing information for that message.

				If no argument was given and the POP3 server issues a positive
				response, then the response given is multi-line.  After the
				initial +OK, for each message in the maildrop, the POP3 server
				responds with a line containing information for that message.	
				
			Examples:
				C: UIDL
				S: +OK
				S: 1 whqtswO00WBw418f9t5JxYwZ
				S: 2 QhdPYR:00WBw1Ph7x7
				S: .
				...
				C: UIDL 2
				S: +OK 2 QhdPYR:00WBw1Ph7x7
				...
				C: UIDL 3
				S: -ERR no such message
			*/

			if(!this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You must authenticate first",new SocketCallBack(this.EndSend));
				return;
			}

			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);

			// Argument isn't specified, multiline response.
			if(argsText.Length == 0){
                StringBuilder reply = new StringBuilder();
				reply.Append("+OK\r\n");

                // Send message number and size for each message
                for(int i=0;i<m_POP3_Messages.Count;i++){
                    POP3_Message message = m_POP3_Messages[i];
                    if(!message.MarkedForDelete){
                        reply.Append((i + 1).ToString() + " " + message.UID + "\r\n");
                    }
                }

				// ".<CRLF>" - means end of list
				reply.Append(".\r\n");

				this.Socket.BeginWriteLine(reply.ToString(),null,new SocketCallBack(this.EndSend));
			}
			else{
				// If parameters specified,there may be only one parameter - messageID
				if(param.Length == 1){
					// Check if messageNr is valid
					if(Core.IsNumber(param[0])){
						int messageNr = Convert.ToInt32(param[0]);
						if(m_POP3_Messages.MessageExists(messageNr)){
							POP3_Message msg = m_POP3_Messages[messageNr - 1];

							this.Socket.BeginWriteLine("+OK " + messageNr.ToString() + " " + msg.UID,new SocketCallBack(this.EndSend));
						}
						else{
							this.Socket.BeginWriteLine("-ERR no such message",new SocketCallBack(this.EndSend));
						}
					}
					else{
						this.Socket.BeginWriteLine("-ERR message-number is invalid",new SocketCallBack(this.EndSend));
					}
				}
				else{
					this.Socket.BeginWriteLine("-ERR Syntax error. Syntax:{UIDL [messageNr]}",new SocketCallBack(this.EndSend));
				}
			}	
		}

		#endregion

		#region method APOP

		private void APOP(string argsText)
		{
			/* RFC 1939 7. APOP
			Arguments:
				a string identifying a mailbox and a MD5 digest string
				(both required)
				
			NOTE:
				A POP3 server which implements the APOP command will
				include a timestamp in its banner greeting.  The syntax of
				the timestamp corresponds to the `msg-id' in [RFC822], and
				MUST be different each time the POP3 server issues a banner
				greeting.
				
			Examples:
				S: +OK POP3 server ready <1896.697170952@dbc.mtview.ca.us>
				C: APOP mrose c4c9334bac560ecc979e58001b3e22fb
				S: +OK maildrop has 1 message (369 octets)

				In this example, the shared  secret  is  the  string  `tan-
				staaf'.  Hence, the MD5 algorithm is applied to the string

				<1896.697170952@dbc.mtview.ca.us>tanstaaf
				 
				which produces a digest value of
		            c4c9334bac560ecc979e58001b3e22fb
			 
			*/

			if(this.Authenticated){
				this.Socket.BeginWriteLine("-ERR You are already authenticated",new SocketCallBack(this.EndSend));
				return;
			}

			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);

			// There must be two params
			if(param.Length == 2){
				string userName   = param[0];
				string md5HexHash = param[1];

				// Check if user isn't logged in already
				if(m_pServer.IsUserLoggedIn(userName)){
					this.Socket.BeginWriteLine("-ERR User:'" + userName + "' already logged in",new SocketCallBack(this.EndSend));
					return;
				}

				// Authenticate user
				AuthUser_EventArgs aArgs = m_pServer.OnAuthUser(this,userName,md5HexHash,m_MD5_prefix,AuthType.APOP);
				if(aArgs.Validated){
                    this.SetUserName(userName);

					// Get user messages info.
					m_pServer.OnGetMessagesInfo(this,m_POP3_Messages);

					this.Socket.BeginWriteLine("+OK authentication was successful",new SocketCallBack(this.EndSend));
				}
				else{
					this.Socket.BeginWriteLine("-ERR authentication failed",new SocketCallBack(this.EndSend));
				}
			}
			else{
				this.Socket.BeginWriteLine("-ERR syntax error. Syntax:{APOP userName md5HexHash}",new SocketCallBack(this.EndSend));
			}
		}

		#endregion

		#region method AUTH

		private void AUTH(string argsText)
		{
			/* Rfc 1734
				
				AUTH mechanism

					Arguments:
						a string identifying an IMAP4 authentication mechanism,
						such as defined by [IMAP4-AUTH].  Any use of the string
						"imap" used in a server authentication identity in the
						definition of an authentication mechanism is replaced with
						the string "pop".
						
					Possible Responses:
						+OK maildrop locked and ready
						-ERR authentication exchange failed

					Restrictions:
						may only be given in the AUTHORIZATION state

					Discussion:
						The AUTH command indicates an authentication mechanism to
						the server.  If the server supports the requested
						authentication mechanism, it performs an authentication
						protocol exchange to authenticate and identify the user.
						Optionally, it also negotiates a protection mechanism for
						subsequent protocol interactions.  If the requested
						authentication mechanism is not supported, the server						
						should reject the AUTH command by sending a negative
						response.

						The authentication protocol exchange consists of a series
						of server challenges and client answers that are specific
						to the authentication mechanism.  A server challenge,
						otherwise known as a ready response, is a line consisting
						of a "+" character followed by a single space and a BASE64
						encoded string.  The client answer consists of a line
						containing a BASE64 encoded string.  If the client wishes
						to cancel an authentication exchange, it should issue a
						line with a single "*".  If the server receives such an
						answer, it must reject the AUTH command by sending a
						negative response.

						A protection mechanism provides integrity and privacy
						protection to the protocol session.  If a protection
						mechanism is negotiated, it is applied to all subsequent
						data sent over the connection.  The protection mechanism
						takes effect immediately following the CRLF that concludes
						the authentication exchange for the client, and the CRLF of
						the positive response for the server.  Once the protection
						mechanism is in effect, the stream of command and response
						octets is processed into buffers of ciphertext.  Each
						buffer is transferred over the connection as a stream of
						octets prepended with a four octet field in network byte
						order that represents the length of the following data.
						The maximum ciphertext buffer length is defined by the
						protection mechanism.

						The server is not required to support any particular
						authentication mechanism, nor are authentication mechanisms
						required to support any protection mechanisms.  If an AUTH
						command fails with a negative response, the session remains
						in the AUTHORIZATION state and client may try another
						authentication mechanism by issuing another AUTH command,
						or may attempt to authenticate by using the USER/PASS or
						APOP commands.  In other words, the client may request
						authentication types in decreasing order of preference,
						with the USER/PASS or APOP command as a last resort.

						Should the client successfully complete the authentication
						exchange, the POP3 server issues a positive response and
						the POP3 session enters the TRANSACTION state.
						
				Examples:
							S: +OK POP3 server ready
							C: AUTH KERBEROS_V4
							S: + AmFYig==
							C: BAcAQU5EUkVXLkNNVS5FRFUAOCAsho84kLN3/IJmrMG+25a4DT
								+nZImJjnTNHJUtxAA+o0KPKfHEcAFs9a3CL5Oebe/ydHJUwYFd
								WwuQ1MWiy6IesKvjL5rL9WjXUb9MwT9bpObYLGOKi1Qh
							S: + or//EoAADZI=
							C: DiAF5A4gA+oOIALuBkAAmw==
							S: +OK Kerberos V4 authentication successful
								...
							C: AUTH FOOBAR
							S: -ERR Unrecognized authentication type
			 
			*/
			if(this.Authenticated){
				this.Socket.BeginWriteLine("-ERR already authenticated",new SocketCallBack(this.EndSend));
				return;
			}
			
				
			//------ Parse parameters -------------------------------------//
			string userName = "";
			string password = "";
			AuthUser_EventArgs aArgs = null;

			string[] param = TextUtils.SplitQuotedString(argsText,' ',true);
			switch(param[0].ToUpper())
			{
				case "PLAIN":
					this.Socket.BeginWriteLine("-ERR Unrecognized authentication type.",new SocketCallBack(this.EndSend));
					break;

				case "LOGIN":

					#region LOGIN authentication

				    //---- AUTH = LOGIN ------------------------------
					/* Login
					C: AUTH LOGIN-MD5
					S: + VXNlcm5hbWU6
					C: username_in_base64
					S: + UGFzc3dvcmQ6
					C: password_in_base64
					
					   VXNlcm5hbWU6 base64_decoded= USERNAME
					   UGFzc3dvcmQ6 base64_decoded= PASSWORD
					*/
					// Note: all strings are base64 strings eg. VXNlcm5hbWU6 = UserName.
			
					
					// Query UserName
					this.Socket.WriteLine("+ VXNlcm5hbWU6");

					string userNameLine = this.Socket.ReadLine();
					// Encode username from base64
					if(userNameLine.Length > 0){
						userName = System.Text.Encoding.Default.GetString(Convert.FromBase64String(userNameLine));
					}
						
					// Query Password
					this.Socket.WriteLine("+ UGFzc3dvcmQ6");

					string passwordLine = this.Socket.ReadLine();
					// Encode password from base64
					if(passwordLine.Length > 0){
						password = System.Text.Encoding.Default.GetString(Convert.FromBase64String(passwordLine));
					}
						
					aArgs = m_pServer.OnAuthUser(this,userName,password,"",AuthType.Plain);

                    // There is custom error, return it
                    if(aArgs.ErrorText != null){
                        this.Socket.BeginWriteLine("-ERR " + aArgs.ErrorText,new SocketCallBack(this.EndSend));
                        return;
                    }

					if(aArgs.Validated){
						this.Socket.BeginWriteLine("+OK Authentication successful.",new SocketCallBack(this.EndSend));
						
						this.SetUserName(userName);

						// Get user messages info.
						m_pServer.OnGetMessagesInfo(this,m_POP3_Messages);
					}
					else{
						this.Socket.BeginWriteLine("-ERR Authentication failed",new SocketCallBack(this.EndSend));
					}

					#endregion

					break;

				case "CRAM-MD5":
					
					#region CRAM-MD5 authentication

					/* Cram-M5
					C: AUTH CRAM-MD5
					S: + <md5_calculation_hash_in_base64>
					C: base64(decoded:username password_hash)
					*/
					
					string md5Hash = "<" + Guid.NewGuid().ToString().ToLower() + ">";
					this.Socket.WriteLine("+ " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(md5Hash)));

					string reply = this.Socket.ReadLine();

					reply = System.Text.Encoding.Default.GetString(Convert.FromBase64String(reply));
					string[] replyArgs = reply.Split(' ');
					userName = replyArgs[0];
					
					aArgs = m_pServer.OnAuthUser(this,userName,replyArgs[1],md5Hash,AuthType.CRAM_MD5);

                    // There is custom error, return it
                    if(aArgs.ErrorText != null){
                        this.Socket.BeginWriteLine("-ERR " + aArgs.ErrorText,new SocketCallBack(this.EndSend));
                        return;
                    }

					if(aArgs.Validated){
						this.Socket.BeginWriteLine("+OK Authentication successful.",new SocketCallBack(this.EndSend));
						
                        this.SetUserName(userName);

						// Get user messages info.
						m_pServer.OnGetMessagesInfo(this,m_POP3_Messages);
					}
					else{
						this.Socket.BeginWriteLine("-ERR Authentication failed",new SocketCallBack(this.EndSend));
					}

					#endregion

					break;

				case "DIGEST-MD5":

					#region DIGEST-MD5 authentication

					/* RFC 2831 AUTH DIGEST-MD5
					 * 
					 * Example:
					 * 
					 * C: AUTH DIGEST-MD5
					 * S: + base64(realm="elwood.innosoft.com",nonce="OA6MG9tEQGm2hh",qop="auth",algorithm=md5-sess)
					 * C: base64(username="chris",realm="elwood.innosoft.com",nonce="OA6MG9tEQGm2hh",
					 *    nc=00000001,cnonce="OA6MHXh6VqTrRk",digest-uri="imap/elwood.innosoft.com",
                     *    response=d388dad90d4bbd760a152321f2143af7,qop=auth)
					 * S: + base64(rspauth=ea40f60335c427b5527b84dbabcdfffd)
					 * C:
					 * S: +OK Authentication successful.
					*/

					string realm = this.BindInfo.HostName;
					string nonce = AuthHelper.GenerateNonce();

					this.Socket.WriteLine("+ " + AuthHelper.Base64en(AuthHelper.Create_Digest_Md5_ServerResponse(realm,nonce)));

					string clientResponse = AuthHelper.Base64de(this.Socket.ReadLine());					
					// Check that realm and nonce in client response are same as we specified
					if(clientResponse.IndexOf("realm=\"" + realm + "\"") > - 1 && clientResponse.IndexOf("nonce=\"" + nonce + "\"") > - 1){
						// Parse user name and password compare value
				//		string userName  = "";
						string passwData = "";
						string cnonce = ""; 
						foreach(string clntRespParam in clientResponse.Split(',')){
							if(clntRespParam.StartsWith("username=")){
								userName = clntRespParam.Split(new char[]{'='},2)[1].Replace("\"","");
							}
							else if(clntRespParam.StartsWith("response=")){
								passwData = clntRespParam.Split(new char[]{'='},2)[1];
							}							
							else if(clntRespParam.StartsWith("cnonce=")){
								cnonce = clntRespParam.Split(new char[]{'='},2)[1].Replace("\"","");
							}
						}

						aArgs = m_pServer.OnAuthUser(this,userName,passwData,clientResponse,AuthType.DIGEST_MD5);

                        // There is custom error, return it
                        if(aArgs.ErrorText != null){
                            this.Socket.BeginWriteLine("-ERR " + aArgs.ErrorText,new SocketCallBack(this.EndSend));
                            return;
                        }

						if(aArgs.Validated){
							// Send server computed password hash
							this.Socket.WriteLine("+ " + AuthHelper.Base64en("rspauth=" + aArgs.ReturnData));
					
							// We must got empty line here
							clientResponse = this.Socket.ReadLine();
							if(clientResponse == ""){
								this.Socket.BeginWriteLine("+OK Authentication successful.",new SocketCallBack(this.EndSend));

                                this.SetUserName(userName);
							}
							else{
								this.Socket.BeginWriteLine("-ERR Authentication failed",new SocketCallBack(this.EndSend));
							}
						}
						else{
							this.Socket.BeginWriteLine("-ERR Authentication failed",new SocketCallBack(this.EndSend));
						}
					}
					else{
						this.Socket.BeginWriteLine("-ERR Authentication failed",new SocketCallBack(this.EndSend));
					}

					#endregion

					break;

				default:
					this.Socket.BeginWriteLine("-ERR Unrecognized authentication type.",new SocketCallBack(this.EndSend));
					break;
			}
			//-----------------------------------------------------------------//
		}

		#endregion

		#region method CAPA

		private void CAPA(string argsText)
		{
			/* Rfc 2449 5.  The CAPA Command
			
				The POP3 CAPA command returns a list of capabilities supported by the
				POP3 server.  It is available in both the AUTHORIZATION and
				TRANSACTION states.

				A capability description MUST document in which states the capability
				is announced, and in which states the commands are valid.

				Capabilities available in the AUTHORIZATION state MUST be announced
				in both states.

				If a capability is announced in both states, but the argument might
				differ after authentication, this possibility MUST be stated in the
				capability description.

				(These requirements allow a client to issue only one CAPA command if
				it does not use any TRANSACTION-only capabilities, or any
				capabilities whose values may differ after authentication.)

				If the authentication step negotiates an integrity protection layer,
				the client SHOULD reissue the CAPA command after authenticating, to
				check for active down-negotiation attacks.

				Each capability may enable additional protocol commands, additional
				parameters and responses for existing commands, or describe an aspect
				of server behavior.  These details are specified in the description
				of the capability.
				
				Section 3 describes the CAPA response using [ABNF].  When a
				capability response describes an optional command, the <capa-tag>
				SHOULD be identical to the command keyword.  CAPA response tags are
				case-insensitive.

				CAPA

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

			string capaResponse = "";
			capaResponse += "+OK Capability list follows\r\n";
			capaResponse += "PIPELINING\r\n";
			capaResponse += "UIDL\r\n";
			capaResponse += "TOP\r\n";
            if((m_pServer.SupportedAuthentications & SaslAuthTypes.Plain) != 0){
                capaResponse += "USER\r\n";
            }
            capaResponse += "SASL";
            if((m_pServer.SupportedAuthentications & SaslAuthTypes.Cram_md5) != 0){
                capaResponse += " CRAM-MD5";
            }
            if((m_pServer.SupportedAuthentications & SaslAuthTypes.Digest_md5) != 0){
                capaResponse += " DIGEST-MD5";
            }            
            if((m_pServer.SupportedAuthentications & SaslAuthTypes.Login) != 0){
                capaResponse += " LOGIN";
            }
            capaResponse += "\r\n";
            if(!this.Socket.SSL && this.BindInfo.Certificate != null){
                capaResponse += "STLS\r\n";
            }
			capaResponse += ".\r\n";
			

			this.Socket.BeginWriteLine(capaResponse,new SocketCallBack(this.EndSend));
		}

		#endregion

        #region method STLS

        private void STLS(string argsText)
        {
            /* RFC 2595 4. POP3 STARTTLS extension.
                 Arguments: none

                 Restrictions:
                     Only permitted in AUTHORIZATION state.

                 Discussion:
                     A TLS negotiation begins immediately after the CRLF at the
                     end of the +OK response from the server.  A -ERR response
                     MAY result if a security layer is already active.  Once a
                     client issues a STLS command, it MUST NOT issue further
                     commands until a server response is seen and the TLS
                     negotiation is complete.

                     The STLS command is only permitted in AUTHORIZATION state
                     and the server remains in AUTHORIZATION state, even if
                     client credentials are supplied during the TLS negotiation.
                     The AUTH command [POP-AUTH] with the EXTERNAL mechanism
                     [SASL] MAY be used to authenticate once TLS client
                     credentials are successfully exchanged, but servers
                     supporting the STLS command are not required to support the
                     EXTERNAL mechanism.

                     Once TLS has been started, the client MUST discard cached
                     information about server capabilities and SHOULD re-issue
                     the CAPA command.  This is necessary to protect against
                     man-in-the-middle attacks which alter the capabilities list
                     prior to STLS.  The server MAY advertise different
                     capabilities after STLS.

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

            if(this.Authenticated){
                this.Socket.WriteLine("-ERR STLS command is only permitted in AUTHORIZATION state !");
                return;
            }

            if(this.BindInfo.Certificate == null){
                this.Socket.WriteLine("-ERR TLS not available, SSL certificate isn't specified !");
				return;
            }

            this.Socket.WriteLine("+OK Ready to start TLS");

            try{          
                this.Socket.SwitchToSSL(this.BindInfo.Certificate);

                if(m_pServer.LogCommands){
                    this.Socket.Logger.AddTextEntry("TLS negotiation completed successfully.");
                }
            }
            catch(Exception x){
                this.Socket.WriteLine("-ERR TLS handshake failed ! " + x.Message);
            }

            Reset();

            BeginRecieveCmd();
        }

        #endregion


        #region method Reset

        private void Reset()
		{		
			/* RFC 1939 5. RSET
			Discussion:
				If any messages have been marked as deleted by the POP3
				server, they are unmarked.
			*/
			m_POP3_Messages.ResetDeletedFlag();
		}

		#endregion

		#region method Update

		private void Update()
		{
			/* RFC 1939 6.
			NOTE:
				When the client issues the QUIT command from the TRANSACTION state,
				the POP3 session enters the UPDATE state.  (Note that if the client
				issues the QUIT command from the AUTHORIZATION state, the POP3
				session terminates but does NOT enter the UPDATE state.)

				If a session terminates for some reason other than a client-issued
				QUIT command, the POP3 session does NOT enter the UPDATE state and
				MUST not remove any messages from the maildrop.
			*/

			if(this.Authenticated){
                lock(m_POP3_Messages){
				    // Delete all message which are marked for deletion ---//
				    foreach(POP3_Message msg in m_POP3_Messages){
					    if(msg.MarkedForDelete){
						    m_pServer.OnDeleteMessage(this,msg);
					    }
				    }
                    //-----------------------------------------------------//
                }				
			}
		}

		#endregion


		#region method EndSend
		
		/// <summary>
		/// Is called when asynchronous send completes.
		/// </summary>
		/// <param name="result">If true, then send was successfull.</param>
		/// <param name="count">Count sended.</param>
		/// <param name="exception">Exception happend on send. NOTE: available only is result=false.</param>
		/// <param name="tag">User data.</param>
		private void EndSend(SocketCallBackResult result,long count,Exception exception,object tag)
		{
			try{
				switch(result)
				{
					case SocketCallBackResult.Ok:
						BeginRecieveCmd();
						break;

					case SocketCallBackResult.SocketClosed:
						EndSession();
						break;

					case SocketCallBackResult.Exception:
						OnError(exception);
						break;
				}
			}
			catch(Exception x){
				OnError(x);
			}
		}

		#endregion		


		#region Properties Implementation

		#endregion
		
	}
}
