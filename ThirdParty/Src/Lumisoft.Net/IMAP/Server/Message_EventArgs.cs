using System;

namespace LumiSoft.Net.IMAP.Server
{
	/// <summary>
	/// Provides data for message related events.
	/// </summary>
	public class Message_EventArgs
	{
		private IMAP_Message m_pMessage     = null;
		private string       m_Folder       = "";
		private string       m_CopyLocation = "";
		private byte[]	     m_MessageData  = null;
		private bool         m_HeadersOnly  = false;
		private string       m_ErrorText    = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="folder">IMAP folder which message is.</param>
		/// <param name="msg"></param>
		public Message_EventArgs(string folder,IMAP_Message msg)
		{				
			m_Folder   = folder;
			m_pMessage = msg;
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="folder">IMAP folder which message is.</param>
		/// <param name="msg"></param>
		/// <param name="copyLocation"></param>
		public Message_EventArgs(string folder,IMAP_Message msg,string copyLocation)
		{				
			m_Folder       = folder;
			m_pMessage     = msg;
			m_CopyLocation = copyLocation;
		}

		/// <summary>
		/// GetMessage constructor.
		/// </summary>
		/// <param name="folder">IMAP folder which message is.</param>
		/// <param name="msg"></param>
		/// <param name="headersOnly">Specifies if messages headers or full message is needed.</param>
		public Message_EventArgs(string folder,IMAP_Message msg,bool headersOnly)
		{				
			m_Folder      = folder;
			m_pMessage    = msg;
			m_HeadersOnly = headersOnly;
		}


		#region Properties Implementation

		/// <summary>
		/// Gets IMAP folder.
		/// </summary>
		public string Folder
		{
			get{ return m_Folder; }
		}

		/// <summary>
		/// Gets IMAP message info.
		/// </summary>
		public IMAP_Message Message
		{
			get{ return m_pMessage; }
		}

		/// <summary>
		/// Gets message new location. NOTE: this is available for copy command only.
		/// </summary>
		public string CopyLocation
		{
			get{ return m_CopyLocation; }
		}

		/// <summary>
		/// Gets or sets message data. NOTE: this is available for GetMessage and StoreMessage event only.
		/// </summary>
		public byte[] MessageData
		{
			get{ return m_MessageData; }

			set{ m_MessageData = value; }
		}

		/// <summary>
		/// Gets if message headers or full message wanted. NOTE: this is available for GetMessage event only.
		/// </summary>
		public bool HeadersOnly
		{
			get{ return m_HeadersOnly; }
		}

		/// <summary>
		/// Gets or sets custom error text, which is returned to client.
		/// </summary>
		public string ErrorText
		{
			get{ return m_ErrorText; }

			set{ m_ErrorText = value; }
		}

		#endregion

	}
}
