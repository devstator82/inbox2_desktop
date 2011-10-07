using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Client
{
    /// <summary>
    /// IMAP namespace. Defined in RFC 2342.
    /// </summary>
    public class IMAP_Namespace
    {
        private string m_Name      = "";
        private string m_Delimiter = "";

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="name">Namespace name.</param>
        /// <param name="delimiter">Namespace hierarchy delimiter.</param>
        public IMAP_Namespace(string name,string delimiter)
        {
            m_Name      = name;
            m_Delimiter = delimiter;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets namespace name.
        /// </summary>
        public string Name
        {
            get{ return m_Name; }
        }

        /// <summary>
        /// Gets namespace hierarchy delimiter.
        /// </summary>
        public string Delimiter
        {
            get{ return m_Delimiter; }
        }

        #endregion

    }
}
