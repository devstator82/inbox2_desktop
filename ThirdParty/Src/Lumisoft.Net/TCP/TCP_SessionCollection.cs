using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.TCP
{
    /// <summary>
    /// This class implements TCP session collection.
    /// </summary>
    public class TCP_SessionCollection<T> where T : TCP_Session
    {
        private Dictionary<string,T> m_pItems = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal TCP_SessionCollection()
        {
            m_pItems = new Dictionary<string,T>();
        }


        #region method Add

        /// <summary>
        /// Adds specified TCP session to the colletion.
        /// </summary>
        /// <param name="session">TCP server session to add.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>session</b> is null.</exception>
        internal void Add(T session)
        {
            if(session == null){
                throw new ArgumentNullException("session");
            }

            lock(m_pItems){
                m_pItems.Add(session.ID,session);
            }
        }

        #endregion

        #region method Remove

        /// <summary>
        /// Removes specified TCP server session from the collection.
        /// </summary>
        /// <param name="session">TCP server session to remove.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>session</b> is null.</exception>
        internal void Remove(T session)
        {
            if(session == null){
                throw new ArgumentNullException("session");
            }

            lock(m_pItems){
                m_pItems.Remove(session.ID);
            }
        }

        #endregion

        #region method Clear

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        internal void Clear()
        {
            lock(m_pItems){
                m_pItems.Clear();
            }
        }

        #endregion


        #region method ToArray

        /// <summary>
        /// Copies all TCP server session to new array. This method is thread-safe.
        /// </summary>
        /// <returns>Returns TCP sessions array.</returns>
        public T[] ToArray()
        {
            lock(m_pItems){
                T[] retVal = new T[m_pItems.Count];
                m_pItems.Values.CopyTo(retVal,0);

                return retVal;
            }
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets number of items in the collection.
        /// </summary>
        public int Count
        {
            get{ return m_pItems.Count; }
        }

        /// <summary>
        /// Gets TCP session with the specified ID.
        /// </summary>
        /// <param name="id">Session ID.</param>
        /// <returns>Returns TCP session with the specified ID.</returns>
        public T this[string id]
        {
            get{ return m_pItems[id]; }
        }

        #endregion

    }
}
