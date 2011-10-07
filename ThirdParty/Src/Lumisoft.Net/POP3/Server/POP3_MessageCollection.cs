using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.POP3.Server
{
    /// <summary>
    /// POP3 messages info collection.
    /// </summary>
    public class POP3_MessageCollection : IEnumerable
    {
        private List<POP3_Message> m_pMessages = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public POP3_MessageCollection()
        {
            m_pMessages = new List<POP3_Message>();
        }


        #region method Add

        /// <summary>
        /// Adds new message info to the collection.
        /// </summary>
        /// <param name="id">Message ID.</param>
        /// <param name="uid">Message UID.</param>
        /// <param name="size">Message size in bytes.</param>
        public POP3_Message Add(string id,string uid,long size)
        {
            return Add(id,uid,size,null);
        }

        /// <summary>
        /// Adds new message info to the collection.
        /// </summary>
        /// <param name="id">Message ID.</param>
        /// <param name="uid">Message UID.</param>
        /// <param name="size">Message size in bytes.</param>
        /// <param name="tag">Message user data.</param>
        public POP3_Message Add(string id,string uid,long size,object tag)
        {
            POP3_Message message = new POP3_Message(this,id,uid,size);
            m_pMessages.Add(message);
            message.Tag = tag;

            return message;
        }

        #endregion

        #region method Remove

        /// <summary>
        /// Removes specified message from the collection.
        /// </summary>
        /// <param name="message">Message to remove.</param>
        public void Remove(POP3_Message message)
        {
            m_pMessages.Remove(message);
        }

        #endregion

        #region method ContainsUID

        /// <summary>
        /// Gets if collection contains message with the specified UID.
        /// </summary>
        /// <param name="uid">Message UID to check.</param>
        /// <returns></returns>
        public bool ContainsUID(string uid)
        {
            foreach(POP3_Message message in m_pMessages){
                if(message.UID.ToLower() == uid.ToLower()){
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region method Clear

        /// <summary>
        /// Removes all messages from the collection.
        /// </summary>
        public void Clear()
        {
            m_pMessages.Clear();
        }

        #endregion


        #region method MessageExists

		/// <summary>
		/// Checks if message exists. NOTE marked for delete messages returns false.
		/// </summary>
		/// <param name="sequenceNo">Message 1 based sequence number.</param>
		/// <returns></returns>
		public bool MessageExists(int sequenceNo)
		{
            if(sequenceNo > 0 && sequenceNo <= m_pMessages.Count){
				if(!m_pMessages[sequenceNo - 1].MarkedForDelete){
					return true;
				}
			}
			
			return false;			
		}

		#endregion

        #region method GetTotalMessagesSize

		/// <summary>
		/// Gets messages total sizes. NOTE messages marked for deletion is excluded.
		/// </summary>
		/// <returns></returns>
		public long GetTotalMessagesSize()
		{
			long totalSize = 0;
			foreach(POP3_Message msg in m_pMessages){
				if(!msg.MarkedForDelete){
					totalSize += msg.Size;
				}
			}

			return totalSize;
		}

		#endregion

        #region method ResetDeletedFlag

        /// <summary>
        /// Resets deleted flags on all messages.
        /// </summary>
        public void ResetDeletedFlag()
        {
            foreach(POP3_Message msg in m_pMessages){
				msg.MarkedForDelete = false;
			}
        }

        #endregion


        #region Interface IEnumerator

        /// <summary>
		/// Gets enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return m_pMessages.GetEnumerator();
		}

		#endregion

        #region Properties Implementation

        /// <summary>
        /// Gets number of messages in the collection.
        /// </summary>
        public int Count
        {
            get{ return m_pMessages.Count; }
        }
                     
        /// <summary>
        /// Gets a POP3_Message object in the collection by index number.
        /// </summary>
        /// <param name="index">An Int32 value that specifies the position of the POP3_Message object in the POP3_MessageCollection collection.</param>
        public POP3_Message this[int index]
        {
            get{ return m_pMessages[index]; }
        }

        #endregion

    }
}
