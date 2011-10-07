using System;

namespace LumiSoft.Net.IMAP.Server
{
	/// <summary>
	/// Provides data for SetFolderACL event.
	/// </summary>
	public class IMAP_SETACL_eArgs
	{
		private IMAP_Session       m_pSession     = null;
		private string             m_pFolderName  = "";
		private string             m_UserName     = "";
		private IMAP_Flags_SetType m_FlagsSetType = IMAP_Flags_SetType.Replace;
		private IMAP_ACL_Flags     m_ACL_Flags    = IMAP_ACL_Flags.None;
		private string             m_ErrorText    = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="session">Owner IMAP session.</param>
		/// <param name="folderName">Folder name which ACL to set.</param>
		/// <param name="userName">User name which ACL to set.</param>
		/// <param name="flagsSetType">Specifies how flags must be stored.</param>
		/// <param name="aclFlags">Flags which to store.</param>
		public IMAP_SETACL_eArgs(IMAP_Session session,string folderName,string userName,IMAP_Flags_SetType flagsSetType,IMAP_ACL_Flags aclFlags)
		{
			m_pSession     = session;
			m_pFolderName  = folderName;
			m_UserName     = userName;
			m_FlagsSetType = flagsSetType;
			m_ACL_Flags    = aclFlags;
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
		/// Gets folder name which ACL to set.
		/// </summary>
		public string Folder
		{
			get{ return m_pFolderName; }
		}

		/// <summary>
		/// Gets user name which ACL to set.
		/// </summary>
		public string UserName
		{
			get{ return m_UserName; }
		}

		/// <summary>
		/// Gets how ACL flags must be stored.
		/// </summary>
		public IMAP_Flags_SetType FlagsSetType
		{
			get{ return m_FlagsSetType; }
		}

		/// <summary>
		/// Gets ACL flags. NOTE: See this.FlagsSetType how to store flags.
		/// </summary>
		public IMAP_ACL_Flags ACL
		{
			get{ return m_ACL_Flags; }
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
