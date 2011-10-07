using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net
{   
    /// <summary>
    /// Implements buffered writer for socket.
    /// </summary>
    public class SocketBufferedWriter
    {
        private SocketEx     m_pSocket           = null;
        private int          m_BufferSize        = 8000;
        private byte[]       m_Buffer            = null;
        private int          m_AvailableInBuffer = 0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="socket">Socket where to write data.</param>
        public SocketBufferedWriter(SocketEx socket)
        {
            m_pSocket = socket;

            m_Buffer = new byte[m_BufferSize];
        }


        #region method Flush

        /// <summary>
        /// Forces to send all data in buffer to destination host.
        /// </summary>
        public void Flush()
        {            
            if(m_AvailableInBuffer > 0){
                m_pSocket.Write(m_Buffer,0,m_AvailableInBuffer);
                m_AvailableInBuffer = 0;
            }
        }

        #endregion

        #region method Write

        /// <summary>
        /// Queues specified data to write buffer. If write buffer is full, buffered data will be sent to detination host.
        /// </summary>
        /// <param name="data">Data to queue.</param>
        public void Write(string data)
        {
            Write(System.Text.Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Queues specified data to write buffer. If write buffer is full, buffered data will be sent to detination host.
        /// </summary>
        /// <param name="data">Data to queue.</param>
        public void Write(byte[] data)
        {
            // There is no room to accomodate data to buffer
            if((m_AvailableInBuffer + data.Length) > m_BufferSize){
                // Send buffer data
                m_pSocket.Write(m_Buffer,0,m_AvailableInBuffer);
                m_AvailableInBuffer = 0;

                // Store new data to buffer
                if(data.Length < m_BufferSize){
                    Array.Copy(data,m_Buffer,data.Length);
                    m_AvailableInBuffer = data.Length;
                }
                // Buffer is smaller than data, send it directly
                else{
                    m_pSocket.Write(data);
                }
            }
            // Store data to buffer
            else{
                Array.Copy(data,0,m_Buffer,m_AvailableInBuffer,data.Length);
                m_AvailableInBuffer += data.Length;
            }
        }

        #endregion

    }
}
