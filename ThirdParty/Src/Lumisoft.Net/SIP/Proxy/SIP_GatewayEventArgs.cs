using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.SIP.Proxy
{
    /// <summary>
    /// This class provides data for SIP_ProxyCore.GetGateways event.
    /// </summary>
    public class SIP_GatewayEventArgs
    {
        private string            m_UriScheme = "";
        private string            m_UserName  = "";
        private List<SIP_Gateway> m_pGateways = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="uriScheme">URI scheme which gateways to get.</param>
        /// <param name="userName">Authenticated user name.</param>
        /// <exception cref="ArgumentException">If any argument has invalid value.</exception>
        public SIP_GatewayEventArgs(string uriScheme,string userName)
        {
            if(string.IsNullOrEmpty(uriScheme)){
                throw new ArgumentException("Argument 'uriScheme' value can't be null or empty !");
            }

            m_UriScheme = uriScheme;
            m_UserName  = userName;
            m_pGateways = new List<SIP_Gateway>();
        }


        #region Properties Implementation

        /// <summary>
        /// Gets URI scheme which gateways to get.
        /// </summary>
        public string UriScheme
        {
            get{ return m_UriScheme; }
        }

        /// <summary>
        /// Gets authenticated user name.
        /// </summary>        
        public string UserName
        {
            get{ return m_UserName; }
        }

        /*
        /// <summary>
        /// Gets or sets if specified user has 
        /// </summary>
        public bool IsForbidden
        {
            get{ return false; }

            set{ }
        }*/

        /// <summary>
        /// Gets gateways collection.
        /// </summary>
        public List<SIP_Gateway> Gateways
        {
            get{ return m_pGateways; }
        }

        #endregion

    }
}
