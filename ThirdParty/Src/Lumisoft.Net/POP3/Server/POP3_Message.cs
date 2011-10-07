using System;

namespace LumiSoft.Net.POP3.Server
{
	/// <summary>
	/// Holds POP3_Message info (ID,Size,...).
	/// </summary>
	public class POP3_Message
	{
		private POP3_MessageCollection m_pOwner          = null;
		private string                 m_ID              = "";    // Holds message ID.
		private string                 m_UID             = "";
		private long                   m_Size            = 0;     // Holds message size.
		private bool                   m_MarkedForDelete = false; // Holds marked for delete flag.
		private object                 m_Tag             = null;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="onwer">Owner collection.</param>
		/// <param name="id">Message ID.</param>
		/// <param name="uid">Message UID.</param>
		/// <param name="size">Message size in bytes.</param>
		internal POP3_Message(POP3_MessageCollection onwer,string id,string uid,long size)
		{	
			m_pOwner = onwer;
            m_ID     = id;
            m_UID    = uid;
            m_Size   = size;
		}


		#region Properties Implementation

		/// <summary>
		/// Gets or sets message ID.
		/// </summary>
		public string ID
		{
			get{ return m_ID; }

			set{ m_ID = value; }
		}

		/// <summary>
		/// Gets or sets message UID. This UID is reported in UIDL command.
		/// </summary>
		public string UID
		{
			get{ return m_UID; }

			set{ m_UID = value; }
		}

		/// <summary>
		/// Gets or sets message size in bytes.
		/// </summary>
		public long Size
		{
			get{ return m_Size; }

			set{ m_Size = value; }
		}

		/// <summary>
		/// Gets or sets message state flag.
		/// </summary>
		public bool MarkedForDelete
		{
			get{ return m_MarkedForDelete; }

			set{ m_MarkedForDelete = value; }
		}

		/// <summary>
		/// Gets or sets user data for message.
		/// </summary>
		public object Tag
		{
			get{ return m_Tag; }

			set{ m_Tag = value; }
		}

		#endregion

	}
}
