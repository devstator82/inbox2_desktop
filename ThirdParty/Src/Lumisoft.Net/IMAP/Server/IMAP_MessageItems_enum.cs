using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Server
{
    /// <summary>
    /// Specifies message itmes.
    /// </summary>
    public enum IMAP_MessageItems_enum
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Message main header.
        /// </summary>
        Header = 2,

        /// <summary>
        /// IMAP ENVELOPE structure.
        /// </summary>
        Envelope = 4,

        /// <summary>
        /// IMAP BODYSTRUCTURE structure.
        /// </summary>
        BodyStructure = 8,

        /// <summary>
        /// Full message.
        /// </summary>
        Message = 16,
    }
}
