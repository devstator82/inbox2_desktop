using System;

namespace LumiSoft.Net.IMAP.Server
{
	/// <summary>
	/// Summary description for SharedRootFolders_EventArgs.
	/// </summary>
	public class SharedRootFolders_EventArgs
	{
		private IMAP_Session m_pSession          = null;
		private string[]     m_SharedRootFolders = null;
		private string[]     m_PublicRootFolders = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SharedRootFolders_EventArgs(IMAP_Session session)
		{
			m_pSession = session;
		}


		#region Properties Implementation

		/// <summary>
		/// Gets reference to smtp session.
		/// </summary>
		public IMAP_Session Session
		{
			get{ return m_pSession; }
		}

		/// <summary>
		/// Gets or sets users shared root folders. Ususaly there is only one root folder 'Shared Folders'.
		/// </summary>
		public string[] SharedRootFolders
		{
			get{ return m_SharedRootFolders; }

			set{ m_SharedRootFolders = value; }
		}

		/// <summary>
		/// Gets or sets public root folders. Ususaly there is only one root folder 'Public Folders'.
		/// </summary>
		public string[] PublicRootFolders
		{
			get{ return m_PublicRootFolders; }

			set{ m_PublicRootFolders = value; }
		}

		#endregion
	}
}
