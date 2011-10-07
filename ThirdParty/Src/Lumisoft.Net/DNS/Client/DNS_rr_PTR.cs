using System;

namespace LumiSoft.Net.Dns.Client
{
	/// <summary>
	/// PTR record class.
	/// </summary>
	[Serializable]
	public class DNS_rr_PTR : DNS_rr_base
	{
		private string m_DomainName = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="domainName">DomainName.</param>
		/// <param name="ttl">TTL value.</param>
		public DNS_rr_PTR(string domainName,int ttl) : base(QTYPE.PTR,ttl)
		{
			m_DomainName = domainName;
		}


        #region static method Parse

        /// <summary>
        /// Parses resource record from reply data.
        /// </summary>
        /// <param name="reply">DNS server reply data.</param>
        /// <param name="offset">Current offset in reply data.</param>
        /// <param name="rdLength">Resource record data length.</param>
        /// <param name="ttl">Time to live in seconds.</param>
        public static DNS_rr_PTR Parse(byte[] reply,ref int offset,int rdLength,int ttl)
        {
            string name = "";
			if(Dns_Client.GetQName(reply,ref offset,ref name)){
			    return new DNS_rr_PTR(name,ttl);
            }
            else{
                throw new ArgumentException("Invalid PTR resource record data !");
            }
        }

        #endregion


        #region Properties Implementation

        /// <summary>
		/// Gets domain name.
		/// </summary>
		public string DomainName
		{
			get{ return m_DomainName; }
		}

		#endregion

	}
}
