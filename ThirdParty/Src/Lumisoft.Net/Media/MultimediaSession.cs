using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.Media
{
    /// <summary>
    /// Thsi class implements multimedia session.
    /// </summary>
    public class MultimediaSession
    {
        private string m_Description = "";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MultimediaSession()
        {
        }
        
        #region method GetSDP

        /// <summary>
        /// Gets current multimedia session SDP.
        /// </summary>
        /// <returns>Returns SDP.</returns>
        public SDP.SDP GetSDP()
        {
            // TODO:

            throw new NotImplementedException();
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets or sets multimedia session description.
        /// </summary>
        public string Description
        {
            get{ return m_Description; }

            set{ m_Description = value != null ? value : ""; }
        }

        /*
        public RTP_Session[] Sessions
        {
        }
        */

        #endregion
    }
}
