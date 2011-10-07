using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace LumiSoft.Net.SIP.Proxy
{
    /// <summary>
    /// 
    /// </summary>
    public class SIP_Route
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SIP_Route()
        {
        }


        #region Properties Implementation

        /// <summary>
        /// Gets regex match pattern.
        /// </summary>
        public string MatchPattern
        {
            get{ return ""; }
        }

        /// <summary>
        /// Gets matched URI.
        /// </summary>
        public string Uri
        {
            get{ return ""; }
        }

        /// <summary>
        /// Gets SIP targets for <b>Uri</b>.
        /// </summary>
        public string[] Targets
        {
            get{ return null; }
        }

        /// <summary>
        /// Gets targets processing mode.
        /// </summary>
        public SIP_ForkingMode ProcessMode
        {
            get{ return SIP_ForkingMode.Parallel; }
        }

        /// <summary>
        /// Gets if user needs to authenticate to use this route.
        /// </summary>
        public bool RequireAuthentication
        {
            get{ return true; }
        }

        /// <summary>
        /// Gets targets credentials.
        /// </summary>
        public NetworkCredential[] Credentials
        {
            get{ return null; }
        }

        #endregion

    }
}
