using System;

namespace LumiSoft.Net.IMAP.Server
{
	/// <summary>
	/// Provides data for DeleteFolderACL event.
	/// </summary>
	public class IMAP_DELETEACL_eArgs
	{
		private IMAP_Session m_pSession    = null;
		private string       m_pFolderName = "";
		private string       m_UserName    = "";
		private string       m_ErrorText   = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="session">Owner IMAP session.</param>
		/// <param name="folderName">Folder name which ACL to delete.</param>
		/// <param name="userName">User name which ACL to delete.</param>
		public IMAP_DELETEACL_eArgs(IMAP_Session session,string folderName,string userName)
		{
			m_pSession    = session;
			m_pFolderName = folderName;
			m_UserName    = userName;
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
		/// Gets folder name which ACL to delete.
		/// </summary>
		public string Folder
		{
			get{ return m_pFolderName; }
		}

		/// <summary>
		/// Gets user name which ACL to delete.
		/// </summary>
		public string UserName
		{
			get{ return m_UserName; }
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
