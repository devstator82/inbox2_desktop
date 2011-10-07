using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IO
{
    /// <summary>
    /// This class provides data to asynchronous write from stream methods callback.
    /// </summary>
    public class WriteStream_EventArgs
    {
        private Exception m_pException   = null;
        private Stream    m_pStream      = null;
        private int       m_CountReaded  = 0;
        private int       m_CountWritten = 0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="exception">Exception happened during write or null if operation was successfull.</param>
        /// <param name="stream">Stream which data was written.</param>
        /// <param name="countReaded">Number of bytes readed from <b>stream</b>.</param>
        /// <param name="countWritten">Number of bytes written to source stream.</param>
        internal WriteStream_EventArgs(Exception exception,Stream stream,int countReaded,int countWritten)
        {
            m_pException   = exception;
            m_pStream      = stream;
            m_CountReaded  = countReaded;
            m_CountWritten = countWritten;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets exception happened during write or null if operation was successfull.
        /// </summary>
        public Exception Exception
        {
            get{ return m_pException; }
        }

        /// <summary>
        /// Gets stream what data was written.
        /// </summary>
        public Stream Stream
        {
            get{ return m_pStream; }
        }

        /// <summary>
        /// Gets number of bytes readed from <b>Stream</b>.
        /// </summary>
        public int CountReaded
        {
            get{ return m_CountReaded; }
        }

        /// <summary>
        /// Gets number of bytes written to source stream.
        /// </summary>
        public int CountWritten
        {
            get{ return m_CountWritten; }
        }

        #endregion

    }
}
