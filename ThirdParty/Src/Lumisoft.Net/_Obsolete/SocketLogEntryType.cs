using System;

namespace LumiSoft.Net
{
	/// <summary>
	/// Log entry type.
	/// </summary>
	public enum SocketLogEntryType
	{
		/// <summary>
		/// Data is readed from remote endpoint.
		/// </summary>
		ReadFromRemoteEP = 0,

		/// <summary>
		/// Data is sent to remote endpoint.
		/// </summary>
		SendToRemoteEP = 1,

		/// <summary>
		/// Comment log entry.
		/// </summary>
		FreeText = 2,
	}
}
