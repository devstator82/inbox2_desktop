using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IO
{
    /// <summary>
    /// This class proviedes data to asynchronous read line callback method.
    /// NOTE: ReadLine_EventArgs is reused for next read line call, so don't store references to this class.
    /// </summary>
    public class ReadLine_EventArgs
    {
        private Exception m_pException  = null;
        private int       m_ReadedCount = 0;
        private byte[]    m_pLineBuffer = null;
        private int       m_Count       = 0;
        private object    m_pTag        = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal ReadLine_EventArgs()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="exception">Exception what happened while reading data or null if read line was successfull.</param>
        /// <param name="readedCount">Specifies how many raw bytes was readed.</param>
        /// <param name="data">Line data buffer.</param>
        /// <param name="count">Specifies how many bytes stored to <b>data</b>.</param>
        /// <param name="tag">User data.</param>
        internal ReadLine_EventArgs(Exception exception,int readedCount,byte[] data,int count,object tag)
        {
            m_pException  = exception;
            m_ReadedCount = readedCount;
            m_pLineBuffer = data;
            m_Count       = count;
            m_pTag        = tag;
        }


        #region method DataToString

        /// <summary>
        /// Converts byte[] line data to the specified encoding string.
        /// </summary>
        /// <param name="encoding">Encoding to use for convert.</param>
        /// <returns>Returns line data as string.</returns>
        public string DataToString(Encoding encoding)
        {
            if(encoding == null){
                throw new ArgumentNullException("encoding");
            }

            if(m_pLineBuffer == null){
                return null;
            }
            else{
                return encoding.GetString(m_pLineBuffer,0,m_Count);
            }
        }

        #endregion


        internal void Reuse(Exception exception,int readedCount,byte[] data,int count,object tag)
        {
            m_pException  = exception;
            m_ReadedCount = readedCount;
            m_pLineBuffer = data;
            m_Count       = count;
            m_pTag        = tag;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets exception what happened while reading line. Returns null if read line completed sucessfully.
        /// </summary>
        public Exception Exception
        {
            get{ return m_pException; }
        }

        /// <summary>
        /// Gets number of bytes actualy readed from source stream. Returns 0 if end of stream reached
        /// and no more data. This value includes any readed byte, including line feed, ... .
        /// </summary>
        public int ReadedCount
        {
            get{ return m_ReadedCount; }
        }

        /// <summary>
        /// Gets <b>buffer</b> argumnet what was passed to BeginReadLine mehtod.
        /// </summary>
        public byte[] LineBuffer
        {
            get{ return m_pLineBuffer; }
        }

        /// <summary>
        /// Gets number of bytes stored to <b>LineBuffer</b>.
        /// </summary>
        public int Count
        {
            get{ return m_Count; }
        }

        /// <summary>
        /// Gets readed line data or null if end of stream reached and no more data.
        /// </summary>
        public byte[] Data
        {
            get{
                byte[] retVal = new byte[m_Count];
                Array.Copy(m_pLineBuffer,retVal,m_Count);

                return retVal; 
            }
        }

        /// <summary>
        /// Gets readed line data as string with system <b>default</b> encoding or returns null if end of stream reached and no more data.
        /// </summary>
        public string DataStringDefault
        {
            get{ return DataToString(Encoding.Default); }
        }

        /// <summary>
        /// Gets readed line data as string with <b>ASCII</b> encoding or returns null if end of stream reached and no more data.
        /// </summary>
        public string DataStringAscii
        {
            get{ return DataToString(Encoding.ASCII); }
        }

        /// <summary>
        /// Gets readed line data as string with <b>UTF8</b> encoding or returns null if end of stream reached and no more data.
        /// </summary>
        public string DataStringUtf8
        {
            get{ return DataToString(Encoding.UTF8); }
        }

        /// <summary>
        /// Gets <b>tag</b> argument what was pased to BeginReadLine method.
        /// </summary>
        public object Tag
        {
            get{ return m_pTag; }
        }

        #endregion

    }
}
