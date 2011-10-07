using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Client
{
    /// <summary>
    /// IMAP namespaces info. Defined in RFC 2342.
    /// </summary>
    public class IMAP_NamespacesInfo
    {
        private IMAP_Namespace[] m_pPersonalNamespaces   = null;
        private IMAP_Namespace[] m_pOtherUsersNamespaces = null;
        private IMAP_Namespace[] m_pSharedNamespaces     = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="personalNamespaces">IMAP server "Personal Namespaces".</param>
        /// <param name="otherUsersNamespaces">IMAP server "Other Users Namespaces".</param>
        /// <param name="sharedNamespaces">IMAP server "Shared Namespaces".</param>
        public IMAP_NamespacesInfo(IMAP_Namespace[] personalNamespaces,IMAP_Namespace[] otherUsersNamespaces,IMAP_Namespace[] sharedNamespaces)
        {
            m_pPersonalNamespaces   = personalNamespaces;
            m_pOtherUsersNamespaces = otherUsersNamespaces;
            m_pSharedNamespaces     = sharedNamespaces;
        }


        #region Parse

        #region static method Parse

        /// <summary>
        /// Parses namespace info from IMAP NAMESPACE response string.
        /// </summary>
        /// <param name="namespaceString">IMAP NAMESPACE response string.</param>
        /// <returns></returns>
        internal static IMAP_NamespacesInfo Parse(string namespaceString)
        {
            StringReader r = new StringReader(namespaceString);
            // Skip NAMESPACE
            r.ReadWord();
            
            IMAP_Namespace[] personalNamespaces   = null;
            IMAP_Namespace[] otherUsersNamespaces = null;
            IMAP_Namespace[] sharedNamespaces     = null;
       
            // Personal namespace
            r.ReadToFirstChar();
            if(r.StartsWith("(")){
                personalNamespaces = ParseNamespaces(r.ReadParenthesized());
            }
            // NIL, skip it.
            else{
                r.ReadWord();
            }

            // Users Shared namespace
            r.ReadToFirstChar();
            if(r.StartsWith("(")){
                otherUsersNamespaces = ParseNamespaces(r.ReadParenthesized());
            }
            // NIL, skip it.
            else{
                r.ReadWord();
            }

            // Shared namespace
            r.ReadToFirstChar();
            if(r.StartsWith("(")){
                sharedNamespaces = ParseNamespaces(r.ReadParenthesized());
            }
            // NIL, skip it.
            else{
                r.ReadWord();
            }

            return new IMAP_NamespacesInfo(personalNamespaces,otherUsersNamespaces,sharedNamespaces);
        }

        #endregion

        #region static method ParseNamespaces

        private static IMAP_Namespace[] ParseNamespaces(string val)
        {
            StringReader r = new StringReader(val);
            r.ReadToFirstChar();

            List<IMAP_Namespace> namespaces = new List<IMAP_Namespace>();
            while(r.StartsWith("(")){
                namespaces.Add(ParseNamespace(r.ReadParenthesized()));
            }

            return namespaces.ToArray();
        }

        #endregion

        #region static method ParseNamespace

        private static IMAP_Namespace ParseNamespace(string val)
        {
            string[] parts = TextUtils.SplitQuotedString(val,' ',true);
            string name = "";
            if(parts.Length > 0){
                name = parts[0];
            }
            string delimiter = "";
            if(parts.Length > 1){
                delimiter = parts[1];
            }

            // Remove delimiter from end, if it's there.
            if(name.EndsWith(delimiter)){
                name = name.Substring(0,name.Length - delimiter.Length);
            }

            return new IMAP_Namespace(name,delimiter);
        }

        #endregion

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets IMAP server "Personal Namespaces". Returns null if namespace not defined.
        /// </summary>
        public IMAP_Namespace[] PersonalNamespaces
        {
            get{ return m_pPersonalNamespaces; }
        }

        /// <summary>
        /// Gets IMAP server "Other Users Namespaces". Returns null if namespace not defined.
        /// </summary>
        public IMAP_Namespace[] OtherUsersNamespaces
        {
            get{ return m_pOtherUsersNamespaces; }
        }

        /// <summary>
        /// Gets IMAP server "Shared Namespaces". Returns null if namespace not defined.
        /// </summary>
        public IMAP_Namespace[] SharedNamespaces
        {
            get{ return m_pSharedNamespaces; }
        }

        #endregion

    }
}
