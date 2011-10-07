using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.SMTP
{
    /// <summary>
    /// This value implements SMTP Notify value. Defined in RFC 1891.
    /// </summary>
    public enum SMTP_Notify
    {
        /// <summary>
        /// Notify value not specified.
        /// </summary>
        /// <remarks>
        /// For compatibility with SMTP clients that do not use the NOTIFY
        /// facility, the absence of a NOTIFY parameter in a RCPT command may be
        /// interpreted as either NOTIFY=FAILURE or NOTIFY=FAILURE,DELAY.
        /// </remarks>
        NotSpecified = 0,

        /// <summary>
        /// DSN should not be returned to the sender under any conditions.
        /// </summary>
        Never =  0xFF,

        /// <summary>
        /// DSN should be sent on successful delivery.
        /// </summary>
        Success = 2,

        /// <summary>
        /// DSN should be sent on delivery failure.
        /// </summary>
        Failure = 4,

        /// <summary>
        /// This value indicates the sender's willingness to receive "delayed" DSNs.
        /// </summary>
        Delay = 8,
    }
}
