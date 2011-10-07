using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace LumiSoft.Net.UDP
{
    /// <summary>
    /// This class provides data for <b>UdpServer.PacketReceived</b> event.
    /// </summary>
    public class UDP_PacketEventArgs : EventArgs
    {
        private UDP_Server m_pUdpServer = null;
        private Socket     m_pSocket    = null;
        private IPEndPoint m_pRemoteEP  = null;
        private byte[]     m_pData      = null;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="server">UDP server which received packet.</param>
        /// <param name="socket">Socket which received packet.</param>
        /// <param name="remoteEP">Remote end point which sent data.</param>
        /// <param name="data">UDP data.</param>
        internal UDP_PacketEventArgs(UDP_Server server,Socket socket,IPEndPoint remoteEP,byte[] data)
        {
            m_pUdpServer = server;
            m_pSocket    = socket;
            m_pRemoteEP  = remoteEP;
            m_pData      = data;
        }


        #region method SendReply

        /// <summary>
        /// Sends reply to received packet. This method uses same local end point to send packet which
        /// received packet, this ensures right NAT traversal.
        /// </summary>
        /// <param name="data">Data buffer.</param>
        /// <param name="offset">Offset in the buffer.</param>
        /// <param name="count">Number of bytes to send.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>data</b> is null.</exception>
        public void SendReply(byte[] data,int offset,int count)
        {
            if(data == null){
                throw new ArgumentNullException("data");
            }

            IPEndPoint localEP = null;
            m_pUdpServer.SendPacket(m_pSocket,data,offset,count,m_pRemoteEP,out localEP);
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets UDP server which received packet.
        /// </summary>
        public UDP_Server UdpServer
        {
            get{ return m_pUdpServer; }
        }

        /// <summary>
        /// Gets local end point what recieved packet.
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get{ return (IPEndPoint)m_pSocket.LocalEndPoint; }
        }

        /// <summary>
        /// Gets remote end point what sent data.
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get{ return m_pRemoteEP; }
        }

        /// <summary>
        /// Gets UDP packet data.
        /// </summary>
        public byte[] Data
        {
            get{ return m_pData; }
        }


        /// <summary>
        /// Gets socket which received packet.
        /// </summary>
        internal Socket Socket
        {
            get{ return m_pSocket; }
        }

        #endregion

    }
}
