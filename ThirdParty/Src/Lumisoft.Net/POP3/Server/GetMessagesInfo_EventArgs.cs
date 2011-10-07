using System;

namespace LumiSoft.Net.POP3.Server
{
	/// <summary>
	/// Provides data for the GetMessgesList event.
	/// </summary>
	public class GetMessagesInfo_EventArgs
	{
		private POP3_Session           m_pSession       = null;
		private POP3_MessageCollection m_pPOP3_Messages = null;
		private string                 m_UserName       = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="session">Reference to pop3 session.</param>
		/// <param name="messages"></param>
		/// <param name="mailbox">Mailbox name.</param>
		public GetMessagesInfo_EventArgs(POP3_Session session,POP3_MessageCollection messages,string mailbox)
		{
			m_pSession       = session;
			m_pPOP3_Messages = messages;
			m_UserName       = mailbox;
		}

		#region Properties Implementation

		/// <summary>
		/// Gets reference to pop3 session.
		/// </summary>
		public POP3_Session Session
		{
			get{ return m_pSession; }
		}

		/// <summary>
		/// Gets referance to POP3 messages info.
		/// </summary>
		public POP3_MessageCollection Messages
		{
			get{ return m_pPOP3_Messages; }
		}

		/// <summary>
		/// User Name.
		/// </summary>
		public string UserName
		{
			get{ return m_UserName; }
		}

		/// <summary>
		/// Mailbox name.
		/// </summary>
		public string Mailbox
		{
			get{ return m_UserName; }
		}

		#endregion

	}
}
