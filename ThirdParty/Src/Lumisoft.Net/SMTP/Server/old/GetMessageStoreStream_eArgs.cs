using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LumiSoft.Net.SMTP.Server
{
    /// <summary>
    /// This class provides data for the GetMessageStoreStream event.
    /// </summary>
    public class GetMessageStoreStream_eArgs
    {
        private SMTP_Session m_pSession     = null;
        private Stream       m_pStoreStream = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="session">Reference to calling SMTP sesssion.</param>
        public GetMessageStoreStream_eArgs(SMTP_Session session)
        {
            m_pSession = session;
            m_pStoreStream = new MemoryStream();
        }


        #region Properties Implementation

        /// <summary>
		/// Gets reference to smtp session.
		/// </summary>
		public SMTP_Session Session
		{
			get{ return m_pSession; }
		}

        /// <summary>
        /// Gets or sets Stream where to store incoming message. Storing starts from stream current position.
        /// </summary>
        public Stream StoreStream
        {
            get{ return m_pStoreStream; }

            set{
                if(value == null){
                    throw new ArgumentNullException("Property StoreStream value can't be null !");
                }
 
                m_pStoreStream = value; 
            }
        }

        #endregion

    }
}
