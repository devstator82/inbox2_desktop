using System;

namespace LumiSoft.Net.AUTH
{	
	/// <summary>
	/// SASL authentications
	/// </summary>
	public enum SaslAuthTypes
	{
		/// <summary>
		/// Non authentication
		/// </summary>
		None = 0,

        /// <summary>
        /// Plain text authentication. For POP3 USER/PASS commands, for IMAP LOGIN command.
        /// </summary>
        Plain = 1,

		/// <summary>
		/// LOGIN.
		/// </summary>
		Login = 2,

		/// <summary>
		/// CRAM-MD5
		/// </summary>
		Cram_md5 = 4,

		/// <summary>
		/// DIGEST-MD5.
		/// </summary>
		Digest_md5 = 8,

		/// <summary>
		/// All authentications.
		/// </summary>
		All = 0xF,
	}
}
