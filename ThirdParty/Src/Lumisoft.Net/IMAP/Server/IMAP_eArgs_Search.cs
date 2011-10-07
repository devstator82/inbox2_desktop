using System;

namespace LumiSoft.Net.IMAP.Server
{
	/// <summary>
	/// Provides data from IMAP Search command.
	/// </summary>
	public class IMAP_eArgs_Search
	{
		private IMAP_Session       m_pSession  = null;
		private string             m_Folder    = "";
		private IMAP_Messages      m_pMessages = null;
		private IMAP_SearchMatcher m_pMatcher  = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="session">IMAP session what calls this search.</param>
		/// <param name="folder">IMAP folder name which messages to search.</param>
		/// <param name="matcher">Matcher what must be used to check if message matches searching criterial.</param>
		public IMAP_eArgs_Search(IMAP_Session session,string folder,IMAP_SearchMatcher matcher)
		{
			m_pSession  = session;
			m_Folder    = folder;
			m_pMatcher  = matcher;
			
			m_pMessages = new IMAP_Messages(folder);
		}

        
		#region Properties Implementation

		/// <summary>
		/// Gets current session.
		/// </summary>
		public IMAP_Session Session
		{
			get{ return m_pSession; }
		}

		/// <summary>
		/// Gets folder name which messages to search.
		/// </summary>
		public string Folder
		{
			get{ return m_Folder; }
		}

		/// <summary>
		/// Gets or sets messges what match searching criterial. 
		/// </summary>
		public IMAP_Messages MatchingMessages
		{
			get{ return m_pMessages; }
		}

		/// <summary>
		/// Gets matcher what must be used to check if message matches searching criterial.
		/// </summary>
		public IMAP_SearchMatcher Matcher
		{
			get{ return m_pMatcher; }
		}

		#endregion

	}
}
