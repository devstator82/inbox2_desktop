using System;

namespace LumiSoft.Net.SMTP.Server
{
	/// <summary>
	/// Provides data for the ValidateMailboxSize event.
	/// </summary>
	public class ValidateMailboxSize_EventArgs
	{
		private SMTP_Session m_pSession = null;
		private string       m_eAddress = "";
		private long         m_MsgSize  = 0;
		private bool         m_IsValid  = true;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="session">Reference to smtp session.</param>
		/// <param name="eAddress">Email address of recipient.</param>
		/// <param name="messageSize">Message size.</param>
		public ValidateMailboxSize_EventArgs(SMTP_Session session,string eAddress,long messageSize)
		{
			m_pSession = session;
			m_eAddress = eAddress;
			m_MsgSize  = messageSize;
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
		/// Email address which mailbox size to check.
		/// </summary>
		public string eAddress
		{
			get{ return m_eAddress; }
		}

		/// <summary>
		/// Message size.NOTE: value 0 means that size is unknown.
		/// </summary>
		public long MessageSize
		{
			get{ return m_MsgSize; }
		}

		/// <summary>
		/// Gets or sets if mailbox size is valid.
		/// </summary>
		public bool IsValid
		{
			get{ return m_IsValid; }

			set{ m_IsValid = value; }
		}

	//	public SMTP_Session aa
	//	{
	//		get{ return null; }
	//	}

		#endregion

	}
}
