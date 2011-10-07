using System;

namespace LumiSoft.Net
{
	#region public enum ReadReplyCode

	/// <summary>
	/// Reply reading return codes.
	/// </summary>
	public enum ReadReplyCode
	{
		/// <summary>
		/// Read completed successfully.
		/// </summary>
		Ok             = 0,

		/// <summary>
		/// Read timed out.
		/// </summary>
		TimeOut        = 1,

		/// <summary>
		/// Maximum allowed Length exceeded.
		/// </summary>
		LengthExceeded = 2,

		/// <summary>
		/// Connected client closed connection.
		/// </summary>
		SocketClosed = 3,

		/// <summary>
		/// UnKnown error, eception raised.
		/// </summary>
		UnKnownError   = 4,
	}

	#endregion

	/// <summary>
	/// Summary description for ReadException.
	/// </summary>
	public class ReadException : System.Exception
	{
		private ReadReplyCode m_ReadReplyCode;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="code"></param>
		/// <param name="message"></param>
		public ReadException(ReadReplyCode code,string message) : base(message)
		{	
			m_ReadReplyCode = code;
		}

		#region Properties Implementation

		/// <summary>
		/// Gets read error.
		/// </summary>
		public ReadReplyCode ReadReplyCode
		{
			get{ return m_ReadReplyCode; }
		}

		#endregion

	}
}
