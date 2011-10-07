using System;

namespace LumiSoft.Net.IMAP.Server
{
	/// <summary>
	/// IMAP folder.
	/// </summary>
	public class IMAP_Folder
	{
		private string m_Folder     = "";
		private bool   m_Selectable = true;		

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="folder">Full path to folder, path separator = '/'. Eg. Inbox/myFolder .</param>
		/// <param name="selectable">Gets or sets if folder is selectable(SELECT command can select this folder).</param>
		public IMAP_Folder(string folder,bool selectable)
		{
			m_Folder     = folder;
			m_Selectable = selectable;
		}


		#region Properties Implementation

		/// <summary>
		/// Gets IMAP folder name. Eg. Inbox, Inbox/myFolder, ... .
		/// </summary>
		public string Folder
		{
			get{ return m_Folder; }
		}
		
		/// <summary>
		/// Gets or sets if folder is selectable (SELECT command can select this folder).
		/// </summary>
		public bool Selectable
		{
			get{ return m_Selectable; }

			set{ m_Selectable = value; }
		}

		#endregion

	}
}
