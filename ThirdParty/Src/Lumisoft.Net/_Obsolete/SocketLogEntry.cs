using System;

namespace LumiSoft.Net
{
	/// <summary>
	/// Socket log entry.
	/// </summary>
	public class SocketLogEntry
	{
		private string             m_Text = "";
		private long               m_Size = 0;
		private SocketLogEntryType m_Type = SocketLogEntryType.FreeText;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="text">Log text.</param>
		/// <param name="size">Data size.</param>
		/// <param name="type">Log entry type</param>
		public SocketLogEntry(string text,long size,SocketLogEntryType type)
		{
			m_Text = text;
			m_Type = type;
			m_Size = size;
		}


		#region Properties Implementation

		/// <summary>
		/// Gets log text.
		/// </summary>
		public string Text
		{
			get{ return m_Text; }
		}

		/// <summary>
		/// Gets size of data readed or sent.
		/// </summary>
		public long Size
		{
			get{ return m_Size; }
		}

		/// <summary>
		/// Gets log entry type.
		/// </summary>
		public SocketLogEntryType Type
		{
			get{ return m_Type; }
		}

		#endregion
	}
}
