using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.SDP
{
    /// <summary>
    /// SDP media.
    /// </summary>
    public class SDP_Media
    {
        private SDP_MediaDescription m_pMediaDescription = null;
        private string               m_Title             = null;
        private SDP_ConnectionData   m_pConnectionData   = null;
        private string               m_EncryptionKey     = null;
        private List<SDP_Attribute>  m_pAttributes       = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SDP_Media()
        {
            m_pAttributes = new List<SDP_Attribute>();
        }


        #region method ToValue

        /// <summary>
        /// Converts media entity to corresponding media lines. Attributes included.
        /// </summary>
        /// <returns></returns>
        public string ToValue()
        {
            /*
                m=  (media name and transport address)
                i=* (media title)
                c=* (connection information -- optional if included at session level)
                b=* (zero or more bandwidth information lines)
                k=* (encryption key)
                a=* (zero or more media attribute lines)
            */

            StringBuilder retVal = new StringBuilder();

            // m Media description
            if(this.MediaDescription != null){
                retVal.Append(this.MediaDescription.ToValue());
            }
            // i media title
            if(!string.IsNullOrEmpty(this.Title)){
                retVal.AppendLine("i=" + this.Title);
            }
            // c Connection Data
            if(this.ConnectionData != null){
                retVal.Append(this.ConnectionData.ToValue());
            }
            // a Attributes
            foreach(SDP_Attribute attribute in this.Attributes){
                retVal.Append(attribute.ToValue());
            }

            return retVal.ToString();
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets or sets media description.
        /// </summary>
        public SDP_MediaDescription MediaDescription
        {
            get{ return m_pMediaDescription; }

            set{ m_pMediaDescription = value; }
        }

        /// <summary>
        /// Gets or sets media title.
        /// </summary>
        public string Title
        {
            get{ return m_Title; }

            set{ m_Title = value; }
        }

        /// <summary>
        /// Gets or sets connection data. This is optional value if SDP message specifies this value,
        /// null means not specified.
        /// </summary>
        public SDP_ConnectionData ConnectionData
        {
            get{ return m_pConnectionData; }

            set{ m_pConnectionData = value; }
        }

        /// <summary>
        /// Gets or sets media encryption key info.
        /// </summary>
        public string EncryptionKey
        {
            get{ return m_EncryptionKey; }

            set{ m_EncryptionKey = value; }
        }

        /// <summary>
        /// Gets media attributes collection. This is optional value, Count == 0 means not specified.
        /// </summary>
        public List<SDP_Attribute> Attributes
        {
            get{ return m_pAttributes; }
        }

        #endregion

    }
}
