using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using LumiSoft.Net.IO;
using LumiSoft.Net.Mail;

namespace LumiSoft.Net.MIME
{
    /// <summary>
    /// This class represents MIME message/rfc822 body. Defined in RFC 2046 5.2.1.
    /// </summary>
    public class MIME_b_MessageRfc822 : MIME_b
    {
        private Mail_Message m_pMessage = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MIME_b_MessageRfc822() : base(new MIME_h_ContentType("message/rfc822"))
        {
            m_pMessage = new Mail_Message();
        }


        #region static method Parse

        /// <summary>
        /// Parses body from the specified stream
        /// </summary>
        /// <param name="owner">Owner MIME entity.</param>
        /// <param name="mediaType">MIME media type. For example: text/plain.</param>
        /// <param name="stream">Stream from where to read body.</param>
        /// <returns>Returns parsed body.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>stream</b>, <b>mediaType</b> or <b>stream</b> is null reference.</exception>
        /// <exception cref="ParseException">Is raised when any parsing errors.</exception>
        protected static new MIME_b Parse(MIME_Entity owner,string mediaType,SmartStream stream)
        {
            if(owner == null){
                throw new ArgumentNullException("owner");
            }
            if(mediaType == null){
                throw new ArgumentNullException("mediaType");
            }
            if(stream == null){
                throw new ArgumentNullException("stream");
            }

            MIME_b_MessageRfc822 retVal = new MIME_b_MessageRfc822();
            retVal.m_pMessage = Mail_Message.ParseFromStream(stream);

            return retVal;
        }

        #endregion


        #region method ToStream

        /// <summary>
        /// Stores MIME entity body to the specified stream.
        /// </summary>
        /// <param name="stream">Stream where to store body data.</param>
        /// <param name="headerWordEncoder">Header 8-bit words ecnoder. Value null means that words are not encoded.</param>
        /// <param name="headerParmetersCharset">Charset to use to encode 8-bit header parameters. Value null means parameters not encoded.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>stream</b> is null reference.</exception>
        internal protected override void ToStream(Stream stream,MIME_Encoding_EncodedWord headerWordEncoder,Encoding headerParmetersCharset)
        {
            if(stream == null){
                throw new ArgumentNullException("stream");
            }

            m_pMessage.ToStream(stream,headerWordEncoder,headerParmetersCharset);
        }

        #endregion


        #region Properties implementation
        
        /// <summary>
        /// Gets if body has modified.
        /// </summary>
        public override bool IsModified
        {
            get{ return m_pMessage.IsModified; }
        }
    
        /// <summary>
        /// Gets embbed mail message.
        /// </summary>
        public Mail_Message Message
        {
            get{ return m_pMessage; }
        }

        #endregion
    }
}
