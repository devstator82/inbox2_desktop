using System;

namespace LumiSoft.Data.lsDB
{
	/// <summary>
	/// lsDB data types.
	/// </summary>
	public enum LDB_DataType
	{
		/// <summary>
		/// Unicode string.
		/// </summary>
		String = (int)'s',

		/// <summary>
		/// Long (64-bit integer).
		/// </summary>
		Long = (int)'l',

		/// <summary>
		/// Integer (32-bit integer).
		/// </summary>
		Int = (int)'i',

		/// <summary>
		/// Date time.
		/// </summary>
		DateTime = (int)'t',

		/// <summary>
		/// Boolean.
		/// </summary>
		Bool = (int)'b',
	}
}
