using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Server
{
    /// <summary>
    /// Provides data to event GetMessagesInfo.
    /// </summary>
    public class IMAP_eArgs_GetMessagesInfo
    {
        private IMAP_Session        m_pSession    = null;
        private IMAP_SelectedFolder m_pFolderInfo = null;
        private string              m_ErrorText   = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IMAP_eArgs_GetMessagesInfo(IMAP_Session session,IMAP_SelectedFolder folder)
        {
            m_pSession    = session;
            m_pFolderInfo = folder;
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
        /// Gets folder info.
        /// </summary>
        public IMAP_SelectedFolder FolderInfo
        {
            get{ return m_pFolderInfo; }
        }

        /// <summary>
		/// Gets or sets custom error text, which is returned to client. Null value means no error.
		/// </summary>
		public string ErrorText
		{
			get{ return m_ErrorText; }

			set{ m_ErrorText = value; }
		}

        #endregion

    }
}
