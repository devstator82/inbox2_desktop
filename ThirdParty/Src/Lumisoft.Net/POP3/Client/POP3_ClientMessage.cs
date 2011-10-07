using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.POP3.Client
{
    /// <summary>
    /// This class represents POP3 client message.
    /// </summary>
    public class POP3_ClientMessage
    {
        private POP3_Client m_Pop3Client          = null;
        private int         m_SequenceNumber      = 1;
        private string      m_UID                 = "";
        private int         m_Size                = 0;
        private bool        m_IsMarkedForDeletion = false;
        private bool        m_IsDisposed          = false;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="pop3">Owner POP3 client.</param>
        /// <param name="seqNumber">Message 1 based sequence number.</param>
        /// <param name="size">Message size in bytes.</param>
        internal POP3_ClientMessage(POP3_Client pop3,int seqNumber,int size)
        {
            m_Pop3Client     = pop3;
            m_SequenceNumber = seqNumber;
            m_Size           = size;
        }


        #region method MarkForDeletion

        /// <summary>
        /// Marks message as deleted.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 serveer returns error.</exception>
        public void MarkForDeletion()
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(this.IsMarkedForDeletion){
                return;
            }
            m_IsMarkedForDeletion = true;

            m_Pop3Client.MarkMessageForDeletion(this.SequenceNumber);
        }

        #endregion

        #region mehtod HeaderToString

        /// <summary>
        /// Gets message header as string.
        /// </summary>
        /// <returns>Returns message header as string.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when message is marked for deletion and this method is accessed.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 serveer returns error.</exception>
        public string HeaderToString()
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(this.IsMarkedForDeletion){
                throw new InvalidOperationException("Can't access message, it's marked for deletion.");
            }

            return Encoding.Default.GetString(HeaderToByte());
        }

        #endregion

        #region method HeaderToByte

        /// <summary>
        /// Gets message header as byte[] data.
        /// </summary>
        /// <returns>Returns message header as byte[] data.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when message is marked for deletion and this method is accessed.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 serveer returns error.</exception>
        public byte[] HeaderToByte()
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(this.IsMarkedForDeletion){
                throw new InvalidOperationException("Can't access message, it's marked for deletion.");
            }

            MemoryStream retVal = new MemoryStream();
            MessageTopLinesToStream(retVal,0);

            return retVal.ToArray();
        }

        #endregion

        #region method HeaderToStream

        /// <summary>
        /// Stores message header to the specified stream.
        /// </summary>
        /// <param name="stream">Stream where to store data.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when argument <b>stream</b> value is null.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 serveer returns error.</exception>
        public void HeaderToStream(Stream stream)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(stream == null){
                throw new ArgumentNullException("Argument 'stream' value can't be null.");
            }
            if(this.IsMarkedForDeletion){
                throw new InvalidOperationException("Can't access message, it's marked for deletion.");
            }

            MessageTopLinesToStream(stream,0);
        }

        #endregion

        #region method MessageToByte

        /// <summary>
        /// Gets message as byte[] data.
        /// </summary>
        /// <returns>Returns message as byte[] data.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when message is marked for deletion and this method is accessed.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 serveer returns error.</exception>
        public byte[] MessageToByte()
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(this.IsMarkedForDeletion){
                throw new InvalidOperationException("Can't access message, it's marked for deletion.");
            }

            MemoryStream retVal = new MemoryStream();
            MessageToStream(retVal);

            return retVal.ToArray();
        }

        #endregion

        #region method MessageToStream

        /// <summary>
        /// Stores message to specified stream.
        /// </summary>
        /// <param name="stream">Stream where to store message.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when argument <b>stream</b> value is null.</exception>
        /// <exception cref="InvalidOperationException">Is raised when message is marked for deletion and this method is accessed.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 serveer returns error.</exception>
        public void MessageToStream(Stream stream)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(stream == null){
                throw new ArgumentNullException("Argument 'stream' value can't be null.");
            }
            if(this.IsMarkedForDeletion){
                throw new InvalidOperationException("Can't access message, it's marked for deletion.");
            }

            m_Pop3Client.GetMessage(this.SequenceNumber,stream);
        }

        #endregion

        #region method MessageTopLinesToByte

        /// <summary>
        /// Gets message header + specified number lines of message body.
        /// </summary>
        /// <param name="lineCount">Number of lines to get from message body.</param>
        /// <returns>Returns message header + specified number lines of message body.</returns>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentException">Is raised when <b>numberOfLines</b> is negative value.</exception>
        /// <exception cref="InvalidOperationException">Is raised when message is marked for deletion and this method is accessed.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 serveer returns error.</exception>
        public byte[] MessageTopLinesToByte(int lineCount)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(lineCount < 0){
                throw new ArgumentException("Argument 'lineCount' value must be >= 0.");
            }
            if(this.IsMarkedForDeletion){
                throw new InvalidOperationException("Can't access message, it's marked for deletion.");
            }

            MemoryStream retVal = new MemoryStream();
            MessageTopLinesToStream(retVal,lineCount);

            return retVal.ToArray();
        }

        #endregion

        #region method MessageTopLinesToStream

        /// <summary>
        /// Stores message header + specified number lines of message body to the specified stream.
        /// </summary>
        /// <param name="stream">Stream where to store data.</param>
        /// <param name="lineCount">Number of lines to get from message body.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when argument <b>stream</b> value is null.</exception>
        /// <exception cref="InvalidOperationException">Is raised when message is marked for deletion and this method is accessed.</exception>
        /// <exception cref="POP3_ClientException">Is raised when POP3 serveer returns error.</exception>
        public void MessageTopLinesToStream(Stream stream,int lineCount)
        {
            if(this.IsDisposed){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            if(stream == null){
                throw new ArgumentNullException("Argument 'stream' value can't be null.");
            }
            if(this.IsMarkedForDeletion){
                throw new InvalidOperationException("Can't access message, it's marked for deletion.");
            }

            m_Pop3Client.GetTopOfMessage(this.SequenceNumber,stream,lineCount);            
        }

        #endregion


        #region method Dispose

        /// <summary>
        /// Disposes message.
        /// </summary>
        internal void Dispose()
        {
            if(m_IsDisposed){
                return;
            }

            m_IsDisposed = true;
            m_Pop3Client = null;
        }

        #endregion

        #region method SetUID

        /// <summary>
        /// Sets message UID value.
        /// </summary>
        /// <param name="uid">UID value.</param>
        internal void SetUID(string uid)
        {
            m_UID = uid;
        }

        #endregion

        #region method SetMarkedForDeletion

        /// <summary>
        /// Sets IsMarkedForDeletion flag value.
        /// </summary>
        /// <param name="isMarkedForDeletion">New IsMarkedForDeletion value.</param>
        internal void SetMarkedForDeletion(bool isMarkedForDeletion)
        {
            m_IsMarkedForDeletion = isMarkedForDeletion;
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets if POP3 message is Disposed.
        /// </summary>
        public bool IsDisposed
        {
            get{ return m_IsDisposed; }
        }

        /// <summary>
        /// Gets message 1 based sequence number.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public int SequenceNumber
        {
            get{               
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return m_SequenceNumber; 
            }
        }

        /// <summary>
        /// Gets message UID. NOTE: Before accessing this property, check that server supports UIDL command.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="NotSupportedException">Is raised when POP3 server doesnt support UIDL command.</exception>
        public string UID
        {
            get{
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                if(!m_Pop3Client.IsUidlSupported){
                    throw new NotSupportedException("POP3 server doesn't support UIDL command.");
                }

                return m_UID; 
            }
        }

        /// <summary>
        /// Gets message size in bytes.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public int Size
        {
            get{ 
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                
                return m_Size;
            }
        }

        /// <summary>
        /// Gets if message is marked for deletion.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public bool IsMarkedForDeletion
        {
            get{
                if(this.IsDisposed){
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                
                return m_IsMarkedForDeletion; 
            }
        }

        #endregion

    }
}
