using System;
using System.Collections;

namespace LumiSoft.Net.IMAP.Server
{
	/// <summary>
	/// Provides data for GetFolderACL event.
	/// </summary>
	public class IMAP_GETACL_eArgs
	{
		private IMAP_Session m_pSession    = null;
		private string       m_pFolderName = "";
		private Hashtable    m_ACLs        = null;
		private string       m_ErrorText   = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="session">Owner IMAP session.</param>
		/// <param name="folderName">Folder name which ACL to get.</param>
		public IMAP_GETACL_eArgs(IMAP_Session session,string folderName)
		{
			m_pSession = session;
			m_pFolderName = folderName;

			m_ACLs = new Hashtable();
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
		/// Gets folder name which ACL to get.
		/// </summary>
		public string Folder
		{
			get{ return m_pFolderName; }
		}

		/// <summary>
		/// Gets ACL collection. Key = userName, Value = IMAP_ACL_Flags.
		/// </summary>
		public Hashtable ACL
		{
			get{ return m_ACLs; }
		}

		/// <summary>
		/// Gets or sets error text returned to connected client.
		/// </summary>
		public string ErrorText
		{
			get{ return m_ErrorText; }

			set{ m_ErrorText = value; }
		}

		#endregion

	}
}
