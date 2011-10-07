using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Client
{
    /// <summary>
    /// IMAP quota entry. Defined in RFC 2087.
    /// </summary>
    public class IMAP_Quota
    {
        private string m_QuotaRootName = "";
        private long   m_Messages      = -1;
        private long   m_MaxMessages   = -1;
        private long   m_Storage       = -1;
        private long   m_MaxStorage    = -1;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="quotaRootName">Quota root name.</param>
        /// <param name="messages">Number of current messages.</param>
        /// <param name="maxMessages">Number of maximum allowed messages.</param>
        /// <param name="storage">Current storage bytes.</param>
        /// <param name="maxStorage">Maximum allowed storage bytes.</param>
        public IMAP_Quota(string quotaRootName,long messages,long maxMessages,long storage,long maxStorage)
        {
            m_QuotaRootName = quotaRootName;
            m_Messages      = messages;
            m_MaxMessages   = maxMessages;
            m_Storage       = storage;
            m_MaxStorage    = maxStorage;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets quota root name.
        /// </summary>
        public string QuotaRootName
        {
            get{ return m_QuotaRootName; }
        }

        /// <summary>
        /// Gets current messages count. Returns -1 if messages and maximum messages quota is not defined.
        /// </summary>
        public long Messages
        {
            get{ return m_Messages; }
        }

        /// <summary>
        /// Gets maximum allowed messages count. Returns -1 if messages and maximum messages quota is not defined.
        /// </summary>
        public long MaximumMessages
        {
            get{ return m_MaxMessages; }
        }

        /// <summary>
        /// Gets current storage in bytes. Returns -1 if storage and maximum storage quota is not defined.
        /// </summary>
        public long Storage
        {
            get{ return m_Storage; }
        }

        /// <summary>
        /// Gets maximum allowed storage in bytes. Returns -1 if storage and maximum storage quota is not defined.
        /// </summary>
        public long MaximumStorage
        {
            get{ return m_MaxStorage; }
        }

        #endregion

    }
}
