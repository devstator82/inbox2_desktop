using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LumiSoft.Net.UDP
{
    #region Delegates Declarations

    /// <summary>
    /// Represents the method that will handle the <b>UdpServer.PacketReceived</b> event.
    /// </summary>
    /// <param name="e">Event data.</param>
    public delegate void PacketReceivedHandler(UDP_PacketEventArgs e);

    #endregion

    /// <summary>
    /// This class implements generic UDP server.
    /// </summary>
    public class UDP_Server
    {
        #region class UdpPacket

        /// <summary>
        /// This class represents UDP packet.
        /// </summary>
        private class UdpPacket
        {
            private Socket     m_pSocket   = null;
            private IPEndPoint m_pRemoteEP = null;
            private byte[]     m_pData     = null;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="socket">Socket which received packet.</param>
            /// <param name="remoteEP">Remote end point from where packet was received.</param>
            /// <param name="data">UDP packet data.</param>
            public UdpPacket(Socket socket,IPEndPoint remoteEP,byte[] data)
            {
                m_pSocket   = socket;
                m_pRemoteEP = remoteEP;
                m_pData     = data;
            }


            #region Properties Implementation

            /// <summary>
            /// Gets socket which received packet.
            /// </summary>
            public Socket Socket
            {
                get{ return m_pSocket; }
            }

            /// <summary>
            /// Gets remote end point from where packet was received.
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

            #endregion
        }

        #endregion
      
        private UDP_ProcessMode          m_ProcessMode      = UDP_ProcessMode.Sequential;
        private int                      m_MTU              = 1400;
        private int                      m_MaxQueueSize     = 200;
        private IPEndPoint[]             m_pBindings        = null;
        private DateTime                 m_StartTime;
        private List<Socket>             m_pSockets         = null;
        private CircleCollection<Socket> m_pSendSocketsIPv4 = null;
        private CircleCollection<Socket> m_pSendSocketsIPv6 = null;
        private Queue<UdpPacket>         m_pQueuedPackets   = null;
        private long                     m_BytesReceived    = 0;
        private long                     m_PacketsReceived  = 0;
        private long                     m_BytesSent        = 0;
        private long                     m_PacketsSent      = 0;
        private bool                     m_IsRunning        = false;
        private bool                     m_IsDisposed       = false;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UDP_Server()
        {        
        }

        #region method Dispose

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        public void Dispose()
        {
            if(m_IsDisposed){
                return;
            }
            m_IsDisposed = false;
            Stop();
            // Release all events.
            this.Error = null;
            this.PacketReceived = null;
        }

        #endregion


        #region method Start

        /// <summary>
        /// Starts UDP server.
        /// </summary>
        public void Start()
        {
            if(m_IsRunning){
                return;
            }
            m_IsRunning = true;

            m_StartTime = DateTime.Now;
            m_pQueuedPackets = new Queue<UdpPacket>();

            // Run only if we have some listening point.
            if(m_pBindings != null){
                // We must replace IPAddress.Any to all available IPs, otherwise it's impossible to send 
                // reply back to UDP packet sender on same local EP where packet received. This is very 
                // important when clients are behind NAT.
                List<IPEndPoint> listeningEPs = new List<IPEndPoint>();
                foreach(IPEndPoint ep in m_pBindings){                    
                    if(ep.Address.Equals(IPAddress.Any)){
                        // Add localhost.
                        IPEndPoint epLocalhost = new IPEndPoint(IPAddress.Loopback,ep.Port);
                        if(!listeningEPs.Contains(epLocalhost)){
                            listeningEPs.Add(epLocalhost);
                        }
                        // Add all host IPs.
                        foreach(IPAddress ip in System.Net.Dns.GetHostAddresses("")){
                            IPEndPoint epNew = new IPEndPoint(ip,ep.Port);
                            if(!listeningEPs.Contains(epNew)){
                                listeningEPs.Add(epNew);
                            }
                        }
                    }
                    else{
                        if(!listeningEPs.Contains(ep)){
                            listeningEPs.Add(ep);
                        }
                    }
                }

                // Create sockets.
                m_pSockets = new List<Socket>();
                foreach(IPEndPoint ep in listeningEPs){
                    try{
                        m_pSockets.Add(Core.CreateSocket(ep,ProtocolType.Udp));
                    }
                    catch(Exception x){
                        OnError(x);
                    }
                }
           
                // Create round-robin send sockets. NOTE: We must skip localhost, it can't be used 
                // for sending out of server.
                m_pSendSocketsIPv4 = new CircleCollection<Socket>();
                m_pSendSocketsIPv6 = new CircleCollection<Socket>();
                foreach(Socket socket in m_pSockets){
                    if(((IPEndPoint)socket.LocalEndPoint).AddressFamily == AddressFamily.InterNetwork){
                        if(!((IPEndPoint)socket.LocalEndPoint).Address.Equals(IPAddress.Loopback)){
                            m_pSendSocketsIPv4.Add(socket);
                        }
                    }
                    else if(((IPEndPoint)socket.LocalEndPoint).AddressFamily == AddressFamily.InterNetworkV6){
                        m_pSendSocketsIPv6.Add(socket);
                    }                    
                }
            
                Thread tr = new Thread(new ThreadStart(this.ProcessIncomingUdp));
                tr.Start();
                Thread tr2 = new Thread(new ThreadStart(this.ProcessQueuedPackets));
                tr2.Start();
            }
        }

        #endregion

        #region method Stop

        /// <summary>
        /// Stops UDP server.
        /// </summary>
        public void Stop()
        {
            if(!m_IsRunning){
                return;
            }
            m_IsRunning = false;
                        
            m_pQueuedPackets = null;
            // Close sockets.
            foreach(Socket socket in m_pSockets){
                socket.Close();
            }
            m_pSockets = null;
            m_pSendSocketsIPv4 = null;
            m_pSendSocketsIPv6 = null;
        }

        #endregion

        #region method Restart

        /// <summary>
        /// Restarts running server. If server is not running, this methods has no efffect.
        /// </summary>
        public void Restart()
        {
            if(m_IsRunning){
                Stop();
                Start();
            }
        }

        #endregion


        #region method SendPacket
                
        /// <summary>
        /// Sends specified UDP packet to the specified remote end point.
        /// </summary>
        /// <param name="packet">UDP packet to send.</param>
        /// <param name="offset">Offset in the buffer.</param>
        /// <param name="count">Number of bytes to send.</param>
        /// <param name="remoteEP">Remote end point.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when any of the arumnets is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public void SendPacket(byte[] packet,int offset,int count,IPEndPoint remoteEP)
        {
            IPEndPoint localEP = null;
            SendPacket(packet,offset,count,remoteEP,out localEP);
        }

        /// <summary>
        /// Sends specified UDP packet to the specified remote end point.
        /// </summary>
        /// <param name="packet">UDP packet to send.</param>
        /// <param name="offset">Offset in the buffer.</param>
        /// <param name="count">Number of bytes to send.</param>
        /// <param name="remoteEP">Remote end point.</param>
        /// <param name="localEP">Returns local IP end point which was used to send UDP packet.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when any of the arumnets is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public void SendPacket(byte[] packet,int offset,int count,IPEndPoint remoteEP,out IPEndPoint localEP)
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException("UdpServer");
            }
            if(!m_IsRunning){
                throw new InvalidOperationException("UDP server is not running.");
            }
            if(packet == null){
                throw new ArgumentNullException("packet");
            }
            if(remoteEP == null){
                throw new ArgumentNullException("remoteEP");
            }

            localEP = null;
            SendPacket(null,packet,offset,count,remoteEP,out localEP);
        }

        /// <summary>
        /// Sends specified UDP packet to the specified remote end point.
        /// </summary>
        /// <param name="localEP">Local end point to use for sending.</param>
        /// <param name="packet">UDP packet to send.</param>
        /// <param name="offset">Offset in the buffer.</param>
        /// <param name="count">Number of bytes to send.</param>
        /// <param name="remoteEP">Remote end point.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when any of the arumnets is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public void SendPacket(IPEndPoint localEP,byte[] packet,int offset,int count,IPEndPoint remoteEP)
        {
            if(m_IsDisposed){
                throw new ObjectDisposedException("UdpServer");
            }
            if(!m_IsRunning){
                throw new InvalidOperationException("UDP server is not running.");
            }
            if(packet == null){
                throw new ArgumentNullException("packet");
            }
            if(localEP == null){
                throw new ArgumentNullException("localEP");
            }        
            if(remoteEP == null){
                throw new ArgumentNullException("remoteEP");
            }
            if(localEP.AddressFamily != remoteEP.AddressFamily){
                throw new ArgumentException("Argumnet localEP and remoteEP AddressFamily won't match.");
            }

            // Search specified local end point socket.
            Socket socket = null;
            if(localEP.AddressFamily == AddressFamily.InterNetwork){
                foreach(Socket s in m_pSendSocketsIPv4.ToArray()){
                    if(localEP.Equals((IPEndPoint)s.LocalEndPoint)){
                        socket = s;
                        break;
                    }
                }
            }
            else if(localEP.AddressFamily == AddressFamily.InterNetworkV6){
                foreach(Socket s in m_pSendSocketsIPv6.ToArray()){
                    if(localEP.Equals((IPEndPoint)s.LocalEndPoint)){
                        socket = s;
                        break;
                    }
                }
            }
            else{
                throw new ArgumentException("Argument 'localEP' has unknown AddressFamily.");
            }

            // We don't have specified local end point.
            if(socket == null){
                throw new ArgumentException("Specified local end point '" + localEP + "' doesn't exist.");
            }
                        
            IPEndPoint lEP = null;
            SendPacket(socket,packet,offset,count,remoteEP,out lEP);
        }

        /// <summary>
        /// Sends specified UDP packet to the specified remote end point.
        /// </summary>
        /// <param name="socket">UDP socket to use for data sending.</param>
        /// <param name="packet">UDP packet to send.</param>
        /// <param name="offset">Offset in the buffer.</param>
        /// <param name="count">Number of bytes to send.</param>
        /// <param name="remoteEP">Remote end point.</param>
        /// <param name="localEP">Returns local IP end point which was used to send UDP packet.</param>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this method is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this method is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when any of the arumnets is null.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        internal void SendPacket(Socket socket,byte[] packet,int offset,int count,IPEndPoint remoteEP,out IPEndPoint localEP)
        {
            // Round-Robin all local end points, if no end point specified.
            if(socket == null){
                // Get right IP address family socket which matches remote end point.
                if(remoteEP.AddressFamily == AddressFamily.InterNetwork){
                    if(m_pSendSocketsIPv4.Count == 0){
                        throw new ArgumentException("There is no suitable IPv4 local end point in this.Bindings.");
                    }

                    socket = m_pSendSocketsIPv4.Next();
                }
                else if(remoteEP.AddressFamily == AddressFamily.InterNetworkV6){
                    if(m_pSendSocketsIPv6.Count == 0){
                        throw new ArgumentException("There is no suitable IPv6 local end point in this.Bindings.");
                    }

                    socket = m_pSendSocketsIPv6.Next();
                }
                else{
                    throw new ArgumentException("Invalid remote end point address family.");
                }
            }

            // Send packet.
            socket.SendTo(packet,0,count,SocketFlags.None,remoteEP);

            localEP = (IPEndPoint)socket.LocalEndPoint;

            m_BytesSent += count;
            m_PacketsSent++;
        }

        #endregion

        #region method GetLocalEndPoint

        /// <summary>
        /// Gets suitable local IP end point for the specified remote endpoint.
        /// If there are multiple sending local end points, they will be load-balanched with round-robin.
        /// </summary>
        /// <param name="remoteEP">Remote end point.</param>
        /// <returns>Returns local IP end point.</returns>
        /// <exception cref="ArgumentNullException">Is raised when argument <b>remoteEP</b> is null.</exception>
        /// <exception cref="ArgumentException">Is raised when argument <b>remoteEP</b> has invalid value.</exception>
        /// <exception cref="InvalidOperationException">Is raised when no suitable IPv4 or IPv6 socket for <b>remoteEP</b>.</exception>
        public IPEndPoint GetLocalEndPoint(IPEndPoint remoteEP)
        {
            if(remoteEP == null){
                throw new ArgumentNullException("remoteEP");
            }

            if(remoteEP.AddressFamily == AddressFamily.InterNetwork){
                // We don't have any IPv4 local end point.
                if(m_pSendSocketsIPv4.Count == 0){
                    throw new InvalidOperationException("There is no suitable IPv4 local end point in this.Bindings.");
                }

                return (IPEndPoint)m_pSendSocketsIPv4.Next().LocalEndPoint;
            }
            else if(remoteEP.AddressFamily == AddressFamily.InterNetworkV6){
                // We don't have any IPv6 local end point.
                if(m_pSendSocketsIPv6.Count == 0){
                    throw new InvalidOperationException("There is no suitable IPv6 local end point in this.Bindings.");
                }

                return (IPEndPoint)m_pSendSocketsIPv6.Next().LocalEndPoint;
            }
            else{
                throw new ArgumentException("Argument 'remoteEP' has unknown AddressFamily.");
            }
        }

        #endregion


        #region method ProcessIncomingUdp

        /// <summary>
        /// Processes incoming UDP data and queues it for processing.
        /// </summary>
        private void ProcessIncomingUdp()
        {
            // Create Round-Robin for listening points.
            CircleCollection<Socket> listeningEPs = new CircleCollection<Socket>();
            foreach(Socket socket in m_pSockets){
                listeningEPs.Add(socket);
            }

            byte[] buffer = new byte[m_MTU];            
            while(m_IsRunning){
                try{
                    // Maximum allowed UDP queued packts exceeded.
                    if(m_pQueuedPackets.Count >= m_MaxQueueSize){
                        Thread.Sleep(1);
                    }
                    else{
                        // Roun-Robin sockets.
                        bool receivedData = false;
                        for(int i=0;i<listeningEPs.Count;i++){
                            Socket socket = listeningEPs.Next();
                            // Read data only when there is some, otherwise we block thread.
                            if(socket.Poll(0,SelectMode.SelectRead)){
                                // Receive data.
                                EndPoint remoteEP = new IPEndPoint(IPAddress.Any,0);
                                int received = socket.ReceiveFrom(buffer,ref remoteEP);
                                m_BytesReceived += received;
                                m_PacketsReceived++;

                                // Queue received packet.
                                byte[] data = new byte[received];
                                Array.Copy(buffer,data,received);
                                lock(m_pQueuedPackets){
                                    m_pQueuedPackets.Enqueue(new UdpPacket(socket,(IPEndPoint)remoteEP,data));
                                }

                                // We received data, so exit round-robin loop.
                                receivedData = true;
                                break;
                            }
                        }
                        // We didn't get any data from any listening point, we must sleep or we use 100% CPU.
                        if(!receivedData){
                            Thread.Sleep(1);
                        }
                    }
                }
                catch(Exception x){
                    OnError(x);
                }
            }
        }

        #endregion

        #region method ProcessQueuedPackets

        /// <summary>
        /// This method processes queued UDP packets.
        /// </summary>
        private void ProcessQueuedPackets()
        {
            while(m_IsRunning){
                try{
                    // There are no packets to process.
                    if(m_pQueuedPackets.Count == 0){
                        Thread.Sleep(1);
                    }
                    else{
                        UdpPacket udpPacket = null;
                        lock(m_pQueuedPackets){
                            udpPacket = m_pQueuedPackets.Dequeue();
                        }

                        // Sequential, call PacketReceived event on same thread.
                        if(m_ProcessMode == UDP_ProcessMode.Sequential){
                            OnUdpPacketReceived(udpPacket);                        
                        }
                        // Parallel, call PacketReceived event on Thread pool thread.
                        else{
                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessPacketOnTrPool),udpPacket);
                        }             
                    }
                }
                catch(Exception x){
                    OnError(x);
                }
            }
        }

        #endregion

        #region method ProcessPacketOnTrPool

        /// <summary>
        /// Processes UDP packet on thread pool thread.
        /// </summary>
        /// <param name="state">User data.</param>
        private void ProcessPacketOnTrPool(object state)
        {
            try{
                OnUdpPacketReceived((UdpPacket)state);
            }
            catch(Exception x){
                OnError(x);
            }
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets if this object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get{ return m_IsDisposed; }
        }

        /// <summary>
        /// Gets if UDP server is running.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public bool IsRunning
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }

                return m_IsRunning;
            }
        }

        /// <summary>
        /// Gets or sets UDP packets process mode.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when server is running and this property value is tried to set.</exception>
        public UDP_ProcessMode ProcessMode
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }

                return m_ProcessMode; 
            }

            set{
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }
                if(m_IsRunning){
                    throw new InvalidOperationException("ProcessMode value can be changed only if UDP server is not running.");
                }

                m_ProcessMode = value;
            }
        }

        /// <summary>
        /// Gets or sets maximum network transmission unit.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised when server is running and this property value is tried to set.</exception>
        public int MTU
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }

                return m_MTU; 
            }

            set{
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }
                if(m_IsRunning){
                    throw new InvalidOperationException("MTU value can be changed only if UDP server is not running.");
                }

                m_MTU = value;
            }
        }

        /// <summary>
        /// Gets maximum UDP packets to queue.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        public int MaxQueueSize
        {
            get{
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }

                return m_MaxQueueSize; 
            }
        }

        /// <summary>
        /// Gets or sets IP end point where UDP server is binded.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="ArgumentNullException">Is raised when null value is passed.</exception>
        public IPEndPoint[] Bindings
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }

                return m_pBindings; 
            }

            set{
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }
                if(value == null){
                    throw new ArgumentNullException();
                }

                // See if changed. Also if server running we must restart it.
                bool changed = false;
                if(m_pBindings == null){
                    changed = true;
                }
                else if(m_pBindings.Length != value.Length){
                    changed = true;
                }
                else{
                    for(int i=0;i<m_pBindings.Length;i++){
                        if(!m_pBindings[i].Equals(value[i])){
                            changed = true;
                            break;
                        }
                    }
                }               
                if(changed){
                    m_pBindings = value;
                    Restart();
                }
            }
        }

        /// <summary>
        /// Gets time when server was started.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this property is accessed.</exception>
        public DateTime StartTime
        {
            get{ 
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }
                if(!m_IsRunning){
                    throw new InvalidOperationException("UDP server is not running.");
                }

                return m_StartTime; 
            }
        }

        /// <summary>
        /// Gets how many bytes this UDP server has received since start.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this property is accessed.</exception>
        public long BytesReceived
        {
            get{               
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }
                if(!m_IsRunning){
                    throw new InvalidOperationException("UDP server is not running.");
                }

                return m_BytesReceived; 
            }
        }

        /// <summary>
        /// Gets how many UDP packets this UDP server has received since start.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this property is accessed.</exception>
        public long PacketsReceived
        {
            get{               
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }
                if(!m_IsRunning){
                    throw new InvalidOperationException("UDP server is not running.");
                }

                return m_PacketsReceived; 
            }
        }

        /// <summary>
        /// Gets how many bytes this UDP server has sent since start.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this property is accessed.</exception>
        public long BytesSent
        {
            get{               
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }
                if(!m_IsRunning){
                    throw new InvalidOperationException("UDP server is not running.");
                }

                return m_BytesSent; 
            }
        }

        /// <summary>
        /// Gets how many UDP packets this UDP server has sent since start.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is raised when this object is disposed and this property is accessed.</exception>
        /// <exception cref="InvalidOperationException">Is raised whan UDP server is not running and this property is accessed.</exception>
        public long PacketsSent
        {
            get{               
                if(m_IsDisposed){
                    throw new ObjectDisposedException("UdpServer");
                }
                if(!m_IsRunning){
                    throw new InvalidOperationException("UDP server is not running.");
                }

                return m_PacketsSent; 
            }
        }

        #endregion

        #region Events Implementation

        /// <summary>
        /// This event is raised when new UDP packet received.
        /// </summary>
        public event PacketReceivedHandler PacketReceived = null;

        /// <summary>
        /// Raises PacketReceived event.
        /// </summary>
        /// <param name="packet">UDP packet.</param>
        private void OnUdpPacketReceived(UdpPacket packet)
        {            
            if(this.PacketReceived != null){
                this.PacketReceived(new UDP_PacketEventArgs(this,packet.Socket,packet.RemoteEndPoint,packet.Data));
            }
        }

        /// <summary>
        /// This event is raised when unexpected error happens.
        /// </summary>
        public event ErrorEventHandler Error = null;

        /// <summary>
        /// Raises Error event.
        /// </summary>
        /// <param name="x">Exception occured.</param>
        private void OnError(Exception x)
        {
            if(this.Error != null){
                this.Error(this,new Error_EventArgs(x,new System.Diagnostics.StackTrace()));
            }
        }

        #endregion

    }
}
