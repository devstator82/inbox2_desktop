using System;
using System.IO;
using System.Data;

namespace LumiSoft.Net.FTP.Server
{
	/// <summary>
	/// Provides data for the filesytem related events for FTP_Server.
	/// </summary>
	public class FileSysEntry_EventArgs
	{
		private FTP_Session m_pSession   = null;
		private string      m_Name       = "";
		private string      m_NewName    = "";
		private bool        m_Validated  = true;
		private Stream      m_FileStream = null;
		private DataSet     m_DsDirInfo  = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="newName"></param>
		/// <param name="session"></param>
		public FileSysEntry_EventArgs(FTP_Session session,string name,string newName)
		{			
			m_Name     = name;
			m_NewName  = newName;
            m_pSession = session;

			m_DsDirInfo  = new DataSet();
			DataTable dt = m_DsDirInfo.Tables.Add("DirInfo");
			dt.Columns.Add("Name");
			dt.Columns.Add("Date",typeof(DateTime));
			dt.Columns.Add("Size",typeof(long));
			dt.Columns.Add("IsDirectory",typeof(bool));
		}


		#region Properties Implementation

		/// <summary>
		/// Gets reference to FTP session.
		/// </summary>
		public FTP_Session Session
		{
			get{ return m_pSession; }
		}

		/// <summary>
		/// Gets directory or file name with path.
		/// </summary>
		public string Name
		{
			get{ return m_Name; }
		}

		/// <summary>
		/// Gets new directory or new file name with path. This filled for Rename event only.
		/// </summary>
		public string NewName
		{
			get{ return m_NewName; }
		}

		/// <summary>
		/// Gets or sets file stream.
		/// </summary>
		public Stream FileStream
		{
			get{ return m_FileStream; }

			set{ m_FileStream = value; }
		}

		/// <summary>
		/// Gets or sets if operation was successful. NOTE: default value is true.
		/// </summary>
		public bool Validated
		{
			get{ return m_Validated; }

			set{ m_Validated = value; }
		}

		/// <summary>
		/// Gets reference to dir listing info. Please Fill .Tables["DirInfo"] table with required fields.
		/// </summary>
		public DataSet DirInfo
		{
			get{ return m_DsDirInfo; }
		}	
	
		#endregion

	}
}
