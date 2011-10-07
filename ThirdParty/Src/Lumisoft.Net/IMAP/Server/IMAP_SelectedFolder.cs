using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.IMAP.Server
{
    /// <summary>
    /// Holds IMAP selected folder info.
    /// </summary>
    public class IMAP_SelectedFolder
    {
        private string                 m_Folder    = "";
        private long                   m_FolderUID = 0;
        private bool                   m_ReadOnly  = false;
        private IMAP_MessageCollection m_pMessages = null;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        internal IMAP_SelectedFolder(string folder)
        {
            m_Folder    = folder;
            m_pMessages = new IMAP_MessageCollection();
        }


        #region method Update

        /// <summary>
        /// Updates current folder messages info with new messages info.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        internal string Update(IMAP_SelectedFolder folder)
        {
            StringBuilder retVal = new StringBuilder();
            long          maxUID = this.MessageUidNext - 1;

            long countExists = this.Messages.Count;
            long countRecent = this.RecentCount;

            // Add new messages
            for(int i=folder.Messages.Count-1;i>0;i--){
                IMAP_Message message = folder.Messages[i];
                // New message
                if(message.UID > maxUID){
                    m_pMessages.Add(
                        message.ID,
                        message.UID,
                        message.InternalDate,
                        message.Size,
                        message.Flags
                    );
                }
                // New messages ended
                else{
                    break;
                }
            }
                       
            // Remove deleted messages
            for(int i=0;i<m_pMessages.Count-1;i++){
                IMAP_Message message = m_pMessages[i];
                
                if(!folder.m_pMessages.ContainsUID(message.UID)){
                    retVal.Append("* " + message.SequenceNo + " EXPUNGE\r\n");
                    m_pMessages.Remove(message);
                    i--;
                }
            }

            if(countExists != this.Messages.Count){
                retVal.Append("* " + this.Messages.Count + " EXISTS\r\n");
            }
            if(countRecent != this.RecentCount){
                retVal.Append("* " + this.RecentCount + " RECENT\r\n");
            }

            return retVal.ToString();
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets selected folder name.
        /// </summary>
        public string Folder
        {
            get{ return m_Folder; }
        }

        /// <summary>
        /// Gets folder UID(UIDVADILITY) value.
        /// </summary>
        public long FolderUID
        {
            get{ return m_FolderUID; }

            set{ m_FolderUID = value; }
        }

        /// <summary>
        /// Gets or sets if folder is read only.
        /// </summary>
        public bool ReadOnly
        {
            get{ return m_ReadOnly; }

            set{ m_ReadOnly = value; }
        }
                
        /// <summary>
        /// Gets selected folder messages info.
        /// </summary>
        public IMAP_MessageCollection Messages
        {
            get{ return m_pMessages; }
        }

        /// <summary>
        /// Gets number of messages with \UNSEEN flags in the collection.
        /// </summary>
        public int UnSeenCount
        {
            get{ 
                int count = 0;
                foreach(IMAP_Message message in m_pMessages){
                    if((message.Flags & IMAP_MessageFlags.Seen) == 0){
                        count++;
                    }
                }
                return count; 
            }
        }

        /// <summary>
        /// Gets number of messages with \RECENT flags in the collection.
        /// </summary>
        public int RecentCount
        {
            get{ 
                int count = 0;
                foreach(IMAP_Message message in m_pMessages){
                    if((message.Flags & IMAP_MessageFlags.Recent) != 0){
                        count++;
                    }
                }
                return count; 
            }
        }

        /// <summary>
        /// Gets number of messages with \DELETED flags in the collection.
        /// </summary>
        public int DeletedCount
        {
            get{ 
                int count = 0;
                foreach(IMAP_Message message in m_pMessages){
                    if((message.Flags & IMAP_MessageFlags.Deleted) != 0){
                        count++;
                    }
                }
                return count; 
            }
        }

        /// <summary>
        /// Gets first message index in the collection which has not \SEEN flag set.
        /// </summary>
        public int FirstUnseen
        {
            get{ 
                int index = 1;
                foreach(IMAP_Message message in m_pMessages){
                    if((message.Flags & IMAP_MessageFlags.Seen) == 0){
                        return index;                   
                    }
                    index++;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets next new message predicted UID.
        /// </summary>
        public long MessageUidNext
        {
            get{ 
                if(m_pMessages.Count > 0){
                    return m_pMessages[m_pMessages.Count - 1].UID + 1;
                }
                else{
                    return 1; 
                }
            }
        }

        #endregion

    }
}
