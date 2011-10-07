using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.SMTP.Relay
{
    /// <summary>
    /// Thsi class holds Relay_Queue queued item.
    /// </summary>
    public class Relay_QueueItem
    {
        private Relay_Queue m_pQueue         = null;
        private string      m_From           = "";
        private string      m_To             = "";
        private string      m_MessageID      = "";
        private Stream      m_pMessageStream = null;
        private object      m_pTag           = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="queue">Item owner queue.</param>
        /// <param name="from">Sender address.</param>
        /// <param name="to">Target recipient address.</param>
        /// <param name="messageID">Message ID.</param>
        /// <param name="message">Raw mime message. Message reading starts from current position.</param>
        /// <param name="tag">User data.</param>
        internal Relay_QueueItem(Relay_Queue queue,string from,string to,string messageID,Stream message,object tag)
        {
            m_pQueue         = queue;
            m_From           = from;
            m_To             = to;
            m_MessageID      = messageID;
            m_pMessageStream = message;
            m_pTag           = tag;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets this relay item owner queue.
        /// </summary>
        public Relay_Queue Queue
        {
            get{ return m_pQueue; }
        }

        /// <summary>
        /// Gets from address.
        /// </summary>
        public string From
        {
            get{ return m_From; }
        }

        /// <summary>
        /// Gets target recipient.
        /// </summary>
        public string To
        {
            get{ return m_To; }
        }

        /// <summary>
        /// Gets message ID which is being relayed now.
        /// </summary>
        public string MessageID
        {
            get{ return m_MessageID; }
        }

        /// <summary>
        /// Gets raw mime message which must be relayed.
        /// </summary>
        public Stream MessageStream
        {
            get{ return m_pMessageStream; }
        }

        /// <summary>
        /// Gets or sets user data.
        /// </summary>
        public object Tag
        {
            get{ return m_pTag; }

            set{ m_pTag = value; }
        }

        #endregion

    }
}
