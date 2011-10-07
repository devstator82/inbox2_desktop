using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Client
{
    /// <summary>
    /// IMAP ACL entry. Defined in RFC 2086.
    /// </summary>
    public class IMAP_Acl
    {
        private string         m_Name   = "";
        private IMAP_ACL_Flags m_Rights = IMAP_ACL_Flags.None;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="name">Authentication identifier name. Normally this is user or group name.</param>
        /// <param name="rights">Rights associated with this ACL entry.</param>
        public IMAP_Acl(string name,IMAP_ACL_Flags rights)
        {
            m_Name   = name;
            m_Rights = rights;
        }


        #region static method Parse

        /// <summary>
        /// Parses ACL entry from IMAP ACL response string.
        /// </summary>
        /// <param name="aclResponseString">IMAP ACL response string.</param>
        /// <returns></returns>
        internal static IMAP_Acl Parse(string aclResponseString)
        {
            string[] args = TextUtils.SplitQuotedString(aclResponseString,' ',true);
            return new IMAP_Acl(args[1],LumiSoft.Net.IMAP.Server.IMAP_Utils.ACL_From_String(args[2]));
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets authentication identifier name. Normally this is user or group name.
        /// </summary>
        public string Name
        {
            get{ return m_Name; }
        }

        /// <summary>
        /// Gets the rights associated with this ACL entry.
        /// </summary>
        public IMAP_ACL_Flags Rights
        {
            get{ return m_Rights; }
        }

        #endregion

    }
}
