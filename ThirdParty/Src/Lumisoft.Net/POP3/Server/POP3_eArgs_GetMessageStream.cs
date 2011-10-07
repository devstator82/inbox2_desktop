using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.POP3.Server
{
    /// <summary>
    /// Provides data to POP3 server event GetMessageStream.
    /// </summary>
    public class POP3_eArgs_GetMessageStream
    {
        private POP3_Session m_pSession           = null;
        private POP3_Message m_pMessageInfo       = null;
        private bool         m_CloseMessageStream = true;
        private Stream       m_MessageStream      = null;
        private long         m_MessageStartOffset = 0;
        private bool         m_MessageExists      = true;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="session">Reference to current POP3 session.</param>
        /// <param name="messageInfo">Message info what message items to get.</param>
        public POP3_eArgs_GetMessageStream(POP3_Session session,POP3_Message messageInfo)
        {
            m_pSession     = session;
            m_pMessageInfo = messageInfo;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets reference to current POP3 session.
        /// </summary>
        public POP3_Session Session
        {
            get{ return m_pSession; }
        }

        /// <summary>
        /// Gets message info what message stream to get.
        /// </summary>
        public POP3_Message MessageInfo
        {
            get{ return m_pMessageInfo; }
        }

        /// <summary>
        /// Gets or sets if message stream is closed automatically if all actions on it are completed.
        /// Default value is true.
        /// </summary>
        public bool CloseMessageStream
        {
            get{ return m_CloseMessageStream; }

            set{ m_CloseMessageStream = value; }
        }

        /// <summary>
        /// Gets or sets message stream. When setting this property Stream position must be where message begins.
        /// </summary>
        public Stream MessageStream
        {
            get{
                if(m_MessageStream != null){
                    m_MessageStream.Position = m_MessageStartOffset;
                }
                return m_MessageStream; 
            }

            set{
                if(value == null){
                    throw new ArgumentNullException("Property MessageStream value can't be null !");
                }
                if(!value.CanSeek){
                    throw new Exception("Stream must support seeking !");
                }

                m_MessageStream = value;
                m_MessageStartOffset = m_MessageStream.Position;
            }
        }

        /// <summary>
        /// Gets message size in bytes.
        /// </summary>
        public long MessageSize
        {
            get{
                if(m_MessageStream == null){
                    throw new Exception("You must set MessageStream property first to use this property !");
                }
                else{
                    return m_MessageStream.Length - m_MessageStream.Position;
                }
            }
        }

        /// <summary>
        /// Gets or sets if message exists. Set this false, if message actually doesn't exist any more.
        /// </summary>
        public bool MessageExists
        {
            get{ return m_MessageExists; }

            set{ m_MessageExists = value; }
        }

        #endregion

    }
}
