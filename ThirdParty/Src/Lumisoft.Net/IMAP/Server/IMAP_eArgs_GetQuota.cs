using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Server
{
    /// <summary>
    /// Provides data for GetUserQuota event.
    /// </summary>
    public class IMAP_eArgs_GetQuota
    {
        private IMAP_Session m_pSession       = null;
        private long         m_MaxMailboxSize = 0;
        private long         m_MailboxSize    = 0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="session">Owner IMAP session.</param>
        public IMAP_eArgs_GetQuota(IMAP_Session session)
        {
            m_pSession = session;
        }


        #region Properties Implementation

        /// <summary>
		/// Gets current IMAP session.
		/// </summary>
		public IMAP_Session Session
		{
			get{ return m_pSession; }
		}

        /// <summary>
        /// Gets user name.
        /// </summary>
        public string UserName
        {
            get{ return m_pSession.UserName; }
        }

        /// <summary>
        /// Gets or sets maximum mailbox size.
        /// </summary>
        public long MaxMailboxSize
        {
            get{ return m_MaxMailboxSize; }

            set{ m_MaxMailboxSize = value; }
        }

        /// <summary>
        /// Gets or sets current mailbox size.
        /// </summary>
        public long MailboxSize
        {
            get{ return m_MailboxSize; }

            set{ m_MailboxSize = value; }
        }

        #endregion

    }
}
