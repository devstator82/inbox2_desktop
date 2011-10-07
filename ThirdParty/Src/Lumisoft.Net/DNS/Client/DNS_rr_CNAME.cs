using System;

namespace LumiSoft.Net.Dns.Client
{
	/// <summary>
	/// CNAME record class.
	/// </summary>
	[Serializable]
	public class DNS_rr_CNAME : DNS_rr_base
	{
		private string m_Alias = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="alias">Alias.</param>
		/// <param name="ttl">TTL value.</param>
		public DNS_rr_CNAME(string alias,int ttl) : base(QTYPE.CNAME,ttl)
		{
			m_Alias = alias;
        }


        #region static method Parse

        /// <summary>
        /// Parses resource record from reply data.
        /// </summary>
        /// <param name="reply">DNS server reply data.</param>
        /// <param name="offset">Current offset in reply data.</param>
        /// <param name="rdLength">Resource record data length.</param>
        /// <param name="ttl">Time to live in seconds.</param>
        public static DNS_rr_CNAME Parse(byte[] reply,ref int offset,int rdLength,int ttl)
        {
            string name = "";			
			if(Dns_Client.GetQName(reply,ref offset,ref name)){			
				return new DNS_rr_CNAME(name,ttl);
			}
            else{
                throw new ArgumentException("Invalid CNAME resource record data !");
            }
        }

        #endregion


        #region Properties Implementation

        /// <summary>
		/// Gets alias.
		/// </summary>
		public string Alias
		{
			get{ return m_Alias; }
		}

		#endregion

	}
}
