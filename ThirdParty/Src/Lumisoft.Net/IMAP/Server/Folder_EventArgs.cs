using System;

namespace LumiSoft.Net.IMAP.Server
{
	/// <summary>
	/// Provides data for IMAP events.
	/// </summary>
	public class Mailbox_EventArgs
	{
		private string m_Folder    = "";
		private string m_NewFolder = "";
		private string m_Error     = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="folder"></param>
		public Mailbox_EventArgs(string folder)
		{	
			m_Folder = folder;
		}

		/// <summary>
		/// Folder rename constructor.
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="newFolder"></param>
		public Mailbox_EventArgs(string folder,string newFolder)
		{	
			m_Folder    = folder;
			m_NewFolder = newFolder;
		}


		#region Properties Implementation

		/// <summary>
		/// Gets folder.
		/// </summary>
		public string Folder
		{
			get{ return m_Folder; }
		}

		/// <summary>
		/// Gets new folder name, this is available for rename only.
		/// </summary>
		public string NewFolder
		{
			get{ return m_NewFolder; }
		}

		/// <summary>
		/// Gets or sets custom error text, which is returned to client.
		/// </summary>
		public string ErrorText
		{
			get{ return m_Error; }

			set{ m_Error  = value; }
		}

		#endregion

	}
}
