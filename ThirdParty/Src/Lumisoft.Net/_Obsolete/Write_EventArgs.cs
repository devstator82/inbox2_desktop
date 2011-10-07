using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IO
{
    /// <summary>
    /// This class provides data for BeginWriteCallback delegate.
    /// </summary>
    public class Write_EventArgs
    {
        private Exception m_pException = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="exception">Exception happened during write or null if operation was successfull.</param>
        internal Write_EventArgs(Exception exception)
        {
            m_pException = exception;
        }


        #region Properties Impelemntation

        /// <summary>
        /// Gets exception happened during write or null if operation was successfull.
        /// </summary>
        public Exception Exception
        {
            get{ return m_pException; }
        }

        #endregion

    }
}
