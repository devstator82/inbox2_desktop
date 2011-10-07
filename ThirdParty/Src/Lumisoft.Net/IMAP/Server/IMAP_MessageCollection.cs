using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Server
{
    /// <summary>
    /// IMAP messages info collection.
    /// </summary>
    public class IMAP_MessageCollection : IEnumerable
    {
        private SortedList<long,IMAP_Message> m_pMessages = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IMAP_MessageCollection()
        {
            m_pMessages = new SortedList<long,IMAP_Message>();
        }


        #region method Add

        /// <summary>
        /// Adds new message info to the collection.
        /// </summary>
        /// <param name="id">Message ID.</param>
        /// <param name="uid">Message IMAP UID value.</param>
        /// <param name="internalDate">Message store date.</param>
        /// <param name="size">Message size in bytes.</param>
        /// <param name="flags">Message flags.</param>
        /// <returns>Returns added IMAp message info.</returns>
        public IMAP_Message Add(string id,long uid,DateTime internalDate,long size,IMAP_MessageFlags flags)
        {
            if(uid < 1){
                throw new ArgumentException("Message UID value must be > 0 !");
            }

            IMAP_Message message = new IMAP_Message(this,id,uid,internalDate,size,flags);              
            m_pMessages.Add(uid,message);
           
            return message;
        }

        #endregion

        #region method Remove

        /// <summary>
        /// Removes specified IMAP message from the collection.
        /// </summary>
        /// <param name="message">IMAP message to remove.</param>
        public void Remove(IMAP_Message message)
        {
            m_pMessages.Remove(message.UID);
        }

        #endregion

        #region method ContainsUID

        /// <summary>
        /// Gets collection contains specified message with specified UID.
        /// </summary>
        /// <param name="uid">Message UID.</param>
        /// <returns></returns>
        public bool ContainsUID(long uid)
        {
            return m_pMessages.ContainsKey(uid);
        }

        #endregion

        #region method IndexOf

        /// <summary>
        /// Gets index of specified message in the collection.
        /// </summary>
        /// <param name="message">Message indesx to get.</param>
        /// <returns>Returns index of specified message in the collection or -1 if message doesn't belong to this collection.</returns>
        public int IndexOf(IMAP_Message message)
        {
            return m_pMessages.IndexOfKey(message.UID);
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

        #region method GetWithFlags

        /// <summary>
        /// Gets messages which has specified flags set.
        /// </summary>
        /// <param name="flags">Flags to match.</param>
        /// <returns></returns>
        public IMAP_Message[] GetWithFlags(IMAP_MessageFlags flags)
        {
            List<IMAP_Message> retVal = new List<IMAP_Message>();
            foreach(IMAP_Message message in m_pMessages.Values){
                if((message.Flags & flags) != 0){
                    retVal.Add(message);
                }
            }
            return retVal.ToArray();
        }

        #endregion



        #region Interface IEnumerator

        /// <summary>
		/// Gets enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return m_pMessages.Values.GetEnumerator();
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
        /// Gets a IMAP_Message object in the collection by index number.
        /// </summary>
        /// <param name="index">An Int32 value that specifies the position of the IMAP_Message object in the IMAP_MessageCollection collection.</param>
        public IMAP_Message this[int index]
        {
            get{ return m_pMessages.Values[index]; }
        }
                
        #endregion

    }
}
