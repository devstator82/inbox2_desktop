using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using LumiSoft.Net.IO;

namespace LumiSoft.Net.MIME
{
    /// <summary>
    /// This class is base class for singlepart media bodies like: text,video,audio,image.
    /// </summary>
    public abstract class MIME_b_SinglepartBase : MIME_b
    {
        private bool       m_IsModified         = false;
        private string     m_MediaType          = "";
        private FileStream m_pEncodedDataStream = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <exception cref="ArgumentNullException">Is raised when <b>mediaType</b> is null reference.</exception>
        public MIME_b_SinglepartBase(string mediaType) : base(new MIME_h_ContentType(mediaType))
        {
            if(mediaType == null){
                throw new ArgumentNullException("mediaType");
            }

            m_MediaType = mediaType;

            m_pEncodedDataStream = new FileStream(Path.GetTempFileName(),FileMode.Create,FileAccess.ReadWrite,FileShare.None,32000,FileOptions.DeleteOnClose);
        }
               

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

            Net_Utils.StreamCopy(GetEncodedDataStream(),stream,32000);
        }

        #endregion


        #region method GetEncodedDataStream

        /// <summary>
        /// Gets body encoded data stream.
        /// </summary>
        /// <returns>Returns body encoded data stream.</returns>
        public Stream GetEncodedDataStream()
        {
            m_pEncodedDataStream.Position = 0;

            return m_pEncodedDataStream;
        }

        #endregion

        #region method SetEncodedData

        /// <summary>
        /// Sets body encoded data from specified stream.
        /// </summary>
        /// <param name="contentTransferEncoding">Content-Transfer-Encoding in what encoding <b>stream</b> data is.</param>
        /// <param name="stream">Stream data to add.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>contentTransferEncoding</b> or <b>stream</b> is null reference.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the argumennts has invalid value.</exception>
        public void SetEncodedData(string contentTransferEncoding,Stream stream)
        {
            if(contentTransferEncoding == null){
                throw new ArgumentNullException("contentTransferEncoding");
            }
            if(contentTransferEncoding == string.Empty){
                throw new ArgumentException("Argument 'contentTransferEncoding' value must be specified.");
            }
            if(stream == null){
                throw new ArgumentNullException("stream");
            }

            m_pEncodedDataStream.SetLength(0);
            Net_Utils.StreamCopy(stream,m_pEncodedDataStream,32000);
            
            // If body won't end with CRLF, add CRLF.
            if(m_pEncodedDataStream.Length >= 2){
                m_pEncodedDataStream.Position = m_pEncodedDataStream.Length - 2;
            }
            if(m_pEncodedDataStream.ReadByte() != '\r' && m_pEncodedDataStream.ReadByte() != '\n'){
                m_pEncodedDataStream.Write(new byte[]{(byte)'\r',(byte)'\n'},0,2);
            }
            this.Entity.ContentTransferEncoding = contentTransferEncoding;

            m_IsModified = true;
        }

        #endregion

        #region method GetDataStream

        /// <summary>
        /// Gets body decoded data stream.
        /// </summary>
        /// <returns>Returns body decoded data stream.</returns>
        /// <exception cref="NotSupportedException">Is raised when body contains not supported Content-Transfer-Encoding.</exception>
        /// <remarks>The returned stream should be clossed/disposed as soon as it's not needed any more.</remarks>
        public Stream GetDataStream()
        { 
            /* RFC 2045 6.1.
                This is the default value -- that is, "Content-Transfer-Encoding: 7BIT" is assumed if the
                Content-Transfer-Encoding header field is not present.
            */
            string transferEncoding = MIME_TransferEncodings.SevenBit;
            if(this.Entity.ContentTransferEncoding != null){
                transferEncoding = this.Entity.ContentTransferEncoding.ToLowerInvariant();
            }

            m_pEncodedDataStream.Position = 0;            
            if(transferEncoding == MIME_TransferEncodings.QuotedPrintable){                
                return new QuotedPrintableStream(new SmartStream(m_pEncodedDataStream,false),FileAccess.Read);
            }
            else if(transferEncoding == MIME_TransferEncodings.Base64){
                return new Base64Stream(m_pEncodedDataStream,false,true,FileAccess.Read);
            }            
            else if(transferEncoding == MIME_TransferEncodings.Binary){
                return new ReadWriteControlledStream(m_pEncodedDataStream,FileAccess.Read);
            }
            else if(transferEncoding == MIME_TransferEncodings.EightBit){
                return new ReadWriteControlledStream(m_pEncodedDataStream,FileAccess.Read);
            }
            else if(transferEncoding == MIME_TransferEncodings.SevenBit){
                return new ReadWriteControlledStream(m_pEncodedDataStream,FileAccess.Read);
            }
            else{
                throw new NotSupportedException("Not supported Content-Transfer-Encoding '" + this.Entity.ContentTransferEncoding + "'.");
            }
        }

        #endregion

        #region method SetData

        /// <summary>
        /// Sets body data from the specified stream.
        /// </summary>
        /// <param name="stream">Source stream.</param>
        /// <param name="transferEncoding">Specifies content-transfer-encoding to use to encode data.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>stream</b> or <b>transferEncoding</b> is null reference.</exception>
        public void SetData(Stream stream,string transferEncoding)
        {           
            if(stream == null){
                throw new ArgumentNullException("stream");
            }
            if(transferEncoding == null){
                throw new ArgumentNullException("transferEncoding");
            }

            if(transferEncoding == MIME_TransferEncodings.QuotedPrintable){
                using(FileStream fs = File.Create(Path.GetTempFileName())){
                    QuotedPrintableStream encoder = new QuotedPrintableStream(new SmartStream(fs,false),FileAccess.ReadWrite);
                    Net_Utils.StreamCopy(stream,encoder,32000);
                    encoder.Flush();
                    fs.Position = 0;
                    SetEncodedData(transferEncoding,fs);
                }
            }
            else if(transferEncoding == MIME_TransferEncodings.Base64){
                using(FileStream fs = File.Create(Path.GetTempFileName())){
                    Base64Stream encoder = new Base64Stream(fs,false,true,FileAccess.ReadWrite);                                     
                    Net_Utils.StreamCopy(stream,encoder,32000);
                    encoder.Finish();
                    fs.Position = 0;
                    SetEncodedData(transferEncoding,fs);
                }
            }            
            else if(transferEncoding == MIME_TransferEncodings.Binary){
                SetEncodedData(transferEncoding,stream);
            }
            else if(transferEncoding == MIME_TransferEncodings.EightBit){
                SetEncodedData(transferEncoding,stream);
            }
            else if(transferEncoding == MIME_TransferEncodings.SevenBit){
                SetEncodedData(transferEncoding,stream);
            }
            else{
                throw new NotSupportedException("Not supported Content-Transfer-Encoding '" + transferEncoding + "'.");
            }
        }

        #endregion

        #region method SetBodyDataFromFile

        /// <summary>
        /// Sets body data from the specified file.
        /// </summary>
        /// <param name="file">File name with optional path.</param>
        /// <param name="transferEncoding">Specifies content-transfer-encoding to use to encode data.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>file</b> is null reference.</exception>
        public void SetBodyDataFromFile(string file,string transferEncoding)
        {
            if(file == null){
                throw new ArgumentNullException("file");
            }
            
            using(FileStream fs = File.OpenRead(file)){
                SetData(fs,transferEncoding);
            }            
        }

        #endregion


        #region Properties implementation

        /// <summary>
        /// Gets if body has modified.
        /// </summary>
        public override bool IsModified
        {
            get{ return m_IsModified; }
        }
                
        /// <summary>
        /// Gets encoded body data size in bytes.
        /// </summary>
        public int EncodedDataSize
        {
            get{ return (int)m_pEncodedDataStream.Length; }
        }

        /// <summary>
        /// Gets body encoded data. 
        /// </summary>
        /// <remarks>NOTE: Use this property with care, because body data may be very big and you may run out of memory.
        /// For bigger data use <see cref="GetEncodedDataStream"/> method instead.</remarks>
        public byte[] EncodedData
        {
            get{ 
                MemoryStream ms = new MemoryStream();
                Net_Utils.StreamCopy(this.GetEncodedDataStream(),ms,32000);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets body decoded data.
        /// </summary>
        /// <remarks>NOTE: Use this property with care, because body data may be very big and you may run out of memory.
        /// For bigger data use <see cref="GetDataStream"/> method instead.</remarks>
        /// <exception cref="NotSupportedException">Is raised when body contains not supported Content-Transfer-Encoding.</exception>
        public byte[] Data
        {
            get{
                MemoryStream ms = new MemoryStream();
                Net_Utils.StreamCopy(this.GetDataStream(),ms,32000);

                return ms.ToArray(); 
            }
        }


        /// <summary>
        /// Gets encoded data stream.
        /// </summary>
        protected Stream EncodedStream
        {
            get{ return m_pEncodedDataStream; }
        }

        #endregion
    }
}
