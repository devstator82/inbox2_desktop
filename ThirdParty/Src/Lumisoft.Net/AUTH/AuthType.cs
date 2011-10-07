using System;

namespace LumiSoft.Net.AUTH
{
	/// <summary>
	/// Authentication type.
	/// </summary>
	public enum AuthType
	{
		/// <summary>
		/// Clear text username/password authentication.
		/// </summary>
		PLAIN = 0,

		/// <summary>
		/// APOP.This is used by POP3 only. RFC 1939 7. APOP.
		/// </summary>
		APOP  = 1,
	
		/// <summary>
		/// CRAM-MD5 authentication. RFC 2195 AUTH CRAM-MD5.
		/// </summary>
		CRAM_MD5 = 3,

		/// <summary>
		/// DIGEST-MD5 authentication. RFC 2831 AUTH DIGEST-MD5.
		/// </summary>
		DIGEST_MD5 = 4,
	}
}
