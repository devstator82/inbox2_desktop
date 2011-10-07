using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Net;

using LumiSoft.Net;
using LumiSoft.Net.RTP;

namespace LumiSoft.Net.RTP.Debug
{
    /// <summary>
    /// This class implements RTP multimedia session debugger/monitoring UI.
    /// </summary>
    public class wfrm_RTP_Debug : Form
    {
        #region method ComboBoxItem

        /// <summary>
        /// This class implements ComboBaox item.
        /// </summary>
        private class ComboBoxItem
        {
            private string m_Text = "";
            private object m_pTag = null;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="text">Text.</param>
            /// <param name="tag">User data.</param>
            public ComboBoxItem(string text,object tag)
            {
                m_Text = text;
                m_pTag = tag;
            }


            #region method ToString

            /// <summary>
            /// Returns ComboBox text.
            /// </summary>
            /// <returns>eturns ComboBox text.</returns>
            public override string ToString()
            {
                return m_Text;
            }

            #endregion


            #region Properties implementation

            /// <summary>
            /// Gets text.
            /// </summary>
            public string Text
            {
                get{ return m_Text; }
            }

            /// <summary>
            /// Gets user data.
            /// </summary>
            public object Tag
            {
                get{ return m_pTag; }
            }

            #endregion
        }

        #endregion

        #region class RTP_SessionStatistics

        /// <summary>
        /// This class provides data for RTP global statistic property grid.
        /// </summary>
        private class RTP_SessionStatistics
        {
            private RTP_Session m_pSession = null;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="session">RTP session.</param>
            /// <exception cref="ArgumentNullException">Is raised when <b>session</b></exception>
            public RTP_SessionStatistics(RTP_Session session)
            {
                if(session == null){
                    throw new ArgumentNullException("session");
                }

                m_pSession = session;
            }


            #region Properties implementation

            /// <summary>
            /// Gets total members count.
            /// </summary>
            public long Members
            {
                get{ return m_pSession.Members.Length; }
            }

            /// <summary>
            /// Gets total members who send RPT data.
            /// </summary>
            public long Senders
            {
                get{ return m_pSession.Senders.Length; }
            }

            /// <summary>
            /// Gets total of RTP packets sent by this session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtpPacketsSent
            {
                get{
                    return m_pSession.RtpPacketsSent; 
                }
            }

            /// <summary>
            /// Gets total of RTP bytes(RTP headers included) sent by this session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtpBytesSent
            {
                get{
                    return m_pSession.RtpBytesSent; 
                }
            }

            /// <summary>
            /// Gets total of RTP packets received by this session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtpPacketsReceived
            {
                get{
                    return m_pSession.RtpPacketsReceived; 
                }
            }

            /// <summary>
            /// Gets total of RTP bytes(RTP headers included) received by this session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtpBytesReceived
            {
                get{
                    return m_pSession.RtpBytesReceived; 
                }
            }

            /// <summary>
            /// Gets number of times RTP packet sending has failed.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtpFailedTransmissions
            {
                get{
                    return m_pSession.RtpFailedTransmissions; 
                }
            }

            /// <summary>
            /// Gets total of RTCP packets sent by this session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtcpPacketsSent
            {
                get{
                    return m_pSession.RtcpPacketsSent; 
                }
            }

            /// <summary>
            /// Gets total of RTCP bytes(RTCP headers included) sent by this session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtcpBytesSent
            {
                get{
                    return m_pSession.RtcpBytesSent; 
                }
            }

            /// <summary>
            /// Gets total of RTCP packets received by this session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtcpPacketsReceived
            {
                get{
                    return m_pSession.RtcpPacketsReceived; 
                }
            }

            /// <summary>
            /// Gets total of RTCP bytes(RTCP headers included) received by this session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtcpBytesReceived
            {
                get{
                    return m_pSession.RtcpBytesReceived; 
                }
            }

            /// <summary>
            /// Gets number of times RTCP packet sending has failed.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtcpFailedTransmissions
            {
                get{
                    return m_pSession.RtcpFailedTransmissions; 
                }
            }

            /// <summary>
            /// Current RTCP reporting interval in seconds.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int RtcpInterval
            {
                get{ 
                    return m_pSession.RtcpInterval; 
                }
            }

            /// <summary>
            /// Gets time when last RTCP report was sent.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public string RtcpLastTransmission
            {
                get{
                    return m_pSession.RtcpLastTransmission.ToString("HH:mm:ss"); 
                }
            }

            /// <summary>
            /// Gets number of times local SSRC collision dedected.
            /// </summary>
            public long LocalCollisions
            {
                get{
                    return m_pSession.LocalCollisions; 
                }
            }

            /// <summary>
            /// Gets number of times remote SSRC collision dedected.
            /// </summary>
            public long RemoteCollisions
            {
                get{
                    return m_pSession.RemoteCollisions; 
                }
            }

            /// <summary>
            /// Gets number of times local packets loop dedected.
            /// </summary>
            public long LocalPacketsLooped
            {
                get{
                    return m_pSession.LocalPacketsLooped; 
                }
            }

            /// <summary>
            /// Gets number of times remote packets loop dedected.
            /// </summary>
            public long RemotePacketsLooped
            {
                get{
                    return m_pSession.RemotePacketsLooped; 
                }
            }

            #endregion

        }

        #endregion

        #region class RTP_ParticipantInfo

        /// <summary>
        /// This class provides data for RTP participant property grid.
        /// </summary>
        private class RTP_ParticipantInfo
        {
            private RTP_Participant m_pParticipant = null;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="participant">RTP local participant.</param>
            /// <exception cref="ArgumentNullException">Is raised when <b>participant</b> null reference.</exception>
            public RTP_ParticipantInfo(RTP_Participant participant)
            {
                if(participant == null){
                    throw new ArgumentNullException("participant");
                }

                m_pParticipant = participant;
            }


            #region Properties implementation

            /// <summary>
            /// Gets or sets the real name, eg. "John Doe". Value null means not specified.
            /// </summary>
            public string Name
            {
                get{
                    if(m_pParticipant is RTP_Participant_Local){
                        return ((RTP_Participant_Local)m_pParticipant).Name;
                    }
                    else{
                        return ((RTP_Participant_Remote)m_pParticipant).Name;
                    }
                }
            }

            /// <summary>
            /// Gets or sets email address. For example "John.Doe@example.com". Value null means not specified.
            /// </summary>
            public string Email
            {
                get{
                    if(m_pParticipant is RTP_Participant_Local){
                        return ((RTP_Participant_Local)m_pParticipant).Email;
                    }
                    else{
                        return ((RTP_Participant_Remote)m_pParticipant).Email;
                    }
                }
            }

            /// <summary>
            /// Gets or sets phone number. For example "+1 908 555 1212". Value null means not specified.
            /// </summary>
            public string Phone
            {
                get{
                    if(m_pParticipant is RTP_Participant_Local){
                        return ((RTP_Participant_Local)m_pParticipant).Phone;
                    }
                    else{
                        return ((RTP_Participant_Remote)m_pParticipant).Phone;
                    }
                }
            }

            /// <summary>
            /// Gets  or sets location string. It may be geographic address or for example chat room name.
            /// Value null means not specified.
            /// </summary>
            public string Location
            {
                get{
                    if(m_pParticipant is RTP_Participant_Local){
                        return ((RTP_Participant_Local)m_pParticipant).Location;
                    }
                    else{
                        return ((RTP_Participant_Remote)m_pParticipant).Location;
                    }
                }
            }

            /// <summary>
            /// Gets or sets streaming application name/version.
            /// Value null means not specified.
            /// </summary>
            public string Tool
            {
                get{
                    if(m_pParticipant is RTP_Participant_Local){
                        return ((RTP_Participant_Local)m_pParticipant).Tool;
                    }
                    else{
                        return ((RTP_Participant_Remote)m_pParticipant).Tool;
                    }
                }
            }

            /// <summary>
            /// Gets or sets note text. The NOTE item is intended for transient messages describing the current state
            /// of the source, e.g., "on the phone, can't talk". Value null means not specified.
            /// </summary>
            public string Note
            {
                get{
                    if(m_pParticipant is RTP_Participant_Local){
                        return ((RTP_Participant_Local)m_pParticipant).Note;
                    }
                    else{
                        return ((RTP_Participant_Remote)m_pParticipant).Note;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region class RTP_SourceInfo

        /// <summary>
        /// This class provides data for RTP "source" property grid.
        /// </summary>
        private class RTP_SourceInfo
        {
            private RTP_Source m_pSource = null;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="source">RTP source.</param>
            /// <exception cref="ArgumentNullException">Is raised when <b>source</b> is null reference.</exception>
            public RTP_SourceInfo(RTP_Source source)
            {
                if(source == null){
                    throw new ArgumentNullException("source");
                }

                m_pSource = source;
            }


            #region Properties implementation

            /// <summary>
            /// Gets source state.
            /// </summary>
            public RTP_SourceState State
            {
                get{ return m_pSource.State; }
            }

            /// <summary>
            /// Gets owner RTP session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int Session
            {
                get{ return m_pSource.Session.GetHashCode(); }
            }

            /// <summary>
            /// Gets synchronization source ID.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public uint SSRC
            {
                get{ return m_pSource.SSRC; }
            }

            /// <summary>
            /// Gets source RTCP end point. Value null means source haven't sent any RTCP packet.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public IPEndPoint RtcpEP
            {
                get{ return m_pSource.RtcpEP; }
            }

            /// <summary>
            /// Gets source RTP end point. Value null means source haven't sent any RTCP packet.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public IPEndPoint RtpEP
            {
                get{ return m_pSource.RtpEP; }
            }

            /// <summary>
            /// Gets last time when source sent RTP or RCTP packet.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public string LastActivity
            {
                get{ return m_pSource.LastActivity.ToString("HH:mm:ss"); }
            }

            /// <summary>
            /// Gets last time when source sent RTCP packet.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public string LastRtcpPacket
            {
                get{ return m_pSource.LastRtcpPacket.ToString("HH:mm:ss"); }
            }

            /// <summary>
            /// Gets last time when source sent RTP packet.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public string LastRtpPacket
            {
                get{ return m_pSource.LastRtpPacket.ToString("HH:mm:ss"); }
            }

            #endregion
        }

        #endregion

        #region class RTP_ReceiveStreamInfo

        /// <summary>
        /// This class provides data for RTP "receive stream" property grid.
        /// </summary>
        private class RTP_ReceiveStreamInfo
        {
            private RTP_ReceiveStream m_pStream = null;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="stream">RTP receive stream.</param>
            public RTP_ReceiveStreamInfo(RTP_ReceiveStream stream)
            {
                if(stream == null){
                    throw new ArgumentNullException("stream");
                }

                m_pStream = stream;
            }


            #region Properties implementation

            /// <summary>
            /// Gets stream owner RTP session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int Session
            {
                get{ return m_pStream.Session.GetHashCode(); }
            }

            /// <summary>
            /// Gets number of times <b>SeqNo</b> has wrapped around.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int SeqNoWrapCount
            {
                get{ return m_pStream.SeqNoWrapCount; }
            }

            /// <summary>
            /// Gets first sequence number what this stream got.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int FirstSeqNo
            {
                get{ return m_pStream.FirstSeqNo; }
            }

            /// <summary>
            /// Gets maximum sequnce number that stream has got.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int MaxSeqNo
            {
                get{ return m_pStream.MaxSeqNo; }
            }

            /// <summary>
            /// Gets how many RTP packets has received by this stream.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long PacketsReceived
            {
                get{ return m_pStream.PacketsReceived; }
            }

            /// <summary>
            /// Gets how many RTP misorder packets has received by this stream.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long PacketsMisorder
            {
                get{ return m_pStream.PacketsMisorder; }
            }

            /// <summary>
            /// Gets how many RTP data has received by this stream.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long BytesReceived
            {
                get{ return m_pStream.BytesReceived; }
            }

            /// <summary>
            /// Gets how many RTP packets has lost during transmission.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long PacketsLost
            {
                get{ return m_pStream.PacketsLost; }
            }

            /// <summary>
            /// Gets inter arrival jitter.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public double Jitter
            {
                get{ return m_pStream.Jitter; }
            }

            /// <summary>
            /// Gets time when last SR(sender report) was received. Returns <b>DateTime.MinValue</b> if no SR received.
            /// </summary>
            public string LastSRTime
            {
                get{ return m_pStream.LastSRTime.ToString("HH:mm:ss"); }
            }

            /// <summary>
            /// Gets delay between las SR(sender report) and now in seconds.
            /// </summary>
            public int DelaySinceLastSR
            {
                get{ return m_pStream.DelaySinceLastSR / 1000; }
            }

            #endregion

        }

        #endregion

        #region method RTP_SendStreamInfo

        /// <summary>
        /// This class provides data for RTP "send stream" property grid.
        /// </summary>
        private class RTP_SendStreamInfo
        {
            private RTP_SendStream m_pStream = null;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="stream">RTP send stream.</param>
            /// <exception cref="ArgumentNullException">Is raised when <b>stream</b> is null reference.</exception>
            public RTP_SendStreamInfo(RTP_SendStream stream)
            {
                if(stream == null){
                    throw new ArgumentNullException("stream");
                }

                m_pStream = stream;
            }


            #region Properties implementation

            /// <summary>
            /// Gets stream owner RTP session.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int Session
            {
                get{ return m_pStream.Session.GetHashCode(); }
            }

            /// <summary>
            /// Gets number of times <b>SeqNo</b> has wrapped around.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int SeqNoWrapCount
            {
                get{ return m_pStream.SeqNoWrapCount; }
            }

            /// <summary>
            /// Gets next packet sequence number.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public int SeqNo
            {
                get{ return m_pStream.SeqNo; }
            }

            /// <summary>
            /// Gets last packet send time.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public string LastPacketTime
            {
                get{ return m_pStream.LastPacketTime.ToString("HH:mm:ss"); }
            }

            /// <summary>
            /// Gets last sent RTP packet RTP timestamp header value.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public uint LastPacketRtpTimestamp
            {
                get{ return m_pStream.LastPacketRtpTimestamp; }
            }

            /// <summary>
            /// Gets how many RTP packets has sent by this stream.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtpPacketsSent
            {
                get{ return m_pStream.RtpPacketsSent; }
            }

            /// <summary>
            /// Gets how many RTP bytes has sent by this stream.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtpBytesSent
            {
                get{ return m_pStream.RtpBytesSent; }
            }

            /// <summary>
            /// Gets how many RTP data(no RTP header included) bytes has sent by this stream.
            /// </summary>
            /// <exception cref="ObjectDisposedException">Is raised when this class is Disposed and this property is accessed.</exception>
            public long RtpDataBytesSent
            {
                get{ return m_pStream.RtpDataBytesSent; }
            }

            #endregion
        }

        #endregion

        private TabControl     m_pTab                  = null;
        private SplitContainer m_pParticipantsSplitter = null;
        private TreeView       m_pParticipants         = null;
        private PropertyGrid   m_pParticipantData      = null;
        private ComboBox       m_pSessions             = null;
        private PropertyGrid   m_pGlobalSessionInfo    = null;
        private ListView       m_pErrors               = null;

        private bool                   m_IsDisposed = false;
        private RTP_MultimediaSession  m_pSession   = null;
        private Timer                  m_pTimer     = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="session">RTP multimedia session.</param>
        public wfrm_RTP_Debug(RTP_MultimediaSession session)
        {
            if(session == null){
                throw new ArgumentNullException("session");
            }

            m_pSession = session;

            InitUI();

            m_pSession.Error += new EventHandler<LumiSoft.Net.ExceptionEventArgs>(m_pSession_Error);
            m_pSession.SessionCreated += new EventHandler<LumiSoft.Net.EventArgs<RTP_Session>>(m_pSession_SessionCreated);
            m_pSession.NewParticipant += new EventHandler<RTP_ParticipantEventArgs>(m_pSession_NewParticipant);
            m_pSession.LocalParticipant.SourceAdded += new EventHandler<RTP_SourceEventArgs>(Participant_SourceAdded);
            m_pSession.LocalParticipant.SourceRemoved += new EventHandler<RTP_SourceEventArgs>(Participant_SourceRemoved);

            m_pTimer = new Timer();
            m_pTimer.Interval = 1000;
            m_pTimer.Tick += new EventHandler(m_pTimer_Tick);
            m_pTimer.Enabled = true;

            TreeNode nodeParticipant = new TreeNode(session.LocalParticipant.CNAME);
            nodeParticipant.Tag = new RTP_ParticipantInfo(session.LocalParticipant);
            nodeParticipant.Nodes.Add("Sources");
            m_pParticipants.Nodes.Add(nodeParticipant);
        }

                                                
        #region method InitUI

        /// <summary>
        /// Creates and initializes UI.
        /// </summary>
        private void InitUI()
        {
            this.ClientSize = new Size(400,450);
            this.Text = "RTP debug";
            //this.Icon = ; TODO:
            this.FormClosing += new FormClosingEventHandler(wfrm_RTP_Debug_FormClosing);

            m_pTab = new TabControl();
            m_pTab.Dock = DockStyle.Fill;

            m_pTab.TabPages.Add("participants","Participants");

            m_pParticipantsSplitter = new SplitContainer();
            m_pParticipantsSplitter.Dock = DockStyle.Fill;
            m_pParticipantsSplitter.Orientation = Orientation.Vertical;
            m_pParticipantsSplitter.SplitterDistance = 60;
            m_pTab.TabPages["participants"].Controls.Add(m_pParticipantsSplitter);
            
            m_pParticipants = new TreeView();
            m_pParticipants.Dock = DockStyle.Fill;
            m_pParticipants.BorderStyle = BorderStyle.None;
            m_pParticipants.FullRowSelect = true;
            m_pParticipants.HideSelection = false;
            m_pParticipants.AfterSelect += new TreeViewEventHandler(m_pParticipants_AfterSelect);
            m_pParticipantsSplitter.Panel1.Controls.Add(m_pParticipants);

            m_pParticipantData = new PropertyGrid();
            m_pParticipantData.Dock = DockStyle.Fill;
            m_pParticipantsSplitter.Panel2.Controls.Add(m_pParticipantData);

            m_pTab.TabPages.Add("global_statistics","Global statistics");

            m_pGlobalSessionInfo = new PropertyGrid();
            m_pGlobalSessionInfo.Dock = DockStyle.Fill;
            m_pTab.TabPages["global_statistics"].Controls.Add(m_pGlobalSessionInfo);

            m_pSessions = new ComboBox();
            m_pSessions.Size = new Size(200,20);
            m_pSessions.Location = new Point(100,2);
            m_pSessions.DropDownStyle = ComboBoxStyle.DropDownList;
            m_pSessions.SelectedIndexChanged += new EventHandler(m_pSessions_SelectedIndexChanged);
            m_pTab.TabPages["global_statistics"].Controls.Add(m_pSessions);
            m_pSessions.BringToFront();

            m_pTab.TabPages.Add("errors","Errors");

            m_pErrors = new ListView();
            m_pErrors.Dock = DockStyle.Fill;
            m_pErrors.View = View.Details;
            m_pErrors.FullRowSelect = true;
            m_pErrors.HideSelection = false;
            m_pErrors.Columns.Add("Message",300);
            m_pErrors.DoubleClick += new EventHandler(m_pErrors_DoubleClick);
            m_pTab.TabPages["errors"].Controls.Add(m_pErrors);

            this.Controls.Add(m_pTab);
        }
                                                                
        #endregion


        #region Events handling

        #region method m_pParticipants_AfterSelect
                
        private void m_pParticipants_AfterSelect(object sender,TreeViewEventArgs e)
        {
            m_pParticipantData.SelectedObject = e.Node.Tag;
        }

        #endregion

        #region method m_pSessions_SelectedIndexChanged

        private void m_pSessions_SelectedIndexChanged(object sender,EventArgs e)
        {
            m_pGlobalSessionInfo.SelectedObject = ((ComboBoxItem)m_pSessions.SelectedItem).Tag;
        }

        #endregion

        #region method m_pErrors_DoubleClick

        private void m_pErrors_DoubleClick(object sender,EventArgs e)
        {
            if(m_pErrors.SelectedItems.Count > 0){
                MessageBox.Show(this,"Error: " + ((Exception)m_pErrors.SelectedItems[0].Tag).ToString(),"Error:",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        #endregion


        #region method m_pSession_Error

        /// <summary>
        /// Is called when RTP session gets unhandled error.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void m_pSession_Error(object sender,LumiSoft.Net.ExceptionEventArgs e)
        {
            if(m_IsDisposed){
                return;
            }

            // Move processing to UI thread.
            this.BeginInvoke(new MethodInvoker(delegate(){
                ListViewItem item = new ListViewItem(e.Exception.Message);
                item.Tag = e.Exception;
                m_pErrors.Items.Add(item);
            }));
        }

        #endregion

        #region method m_pSession_SessionCreated

        /// <summary>
        /// Is called when RTP multimedia session creates new session.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void m_pSession_SessionCreated(object sender,EventArgs<RTP_Session> e)
        {
            if(m_IsDisposed){
                return;
            }

            // Move processing to UI thread.
            this.BeginInvoke(new MethodInvoker(delegate(){
                ComboBoxItem item = new ComboBoxItem("Session: " + e.Value.GetHashCode(),new RTP_SessionStatistics(e.Value));
                m_pSessions.Items.Add(item);

                if(m_pSessions.SelectedIndex == -1){
                    m_pSessions.SelectedIndex = 0;
                }
            }));
        }

        #endregion

        #region method m_pSession_NewParticipant

        /// <summary>
        /// This method is called when RTP session sees new remote participant.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void m_pSession_NewParticipant(object sender,RTP_ParticipantEventArgs e)
        {
            if(m_IsDisposed){
                return;
            }

            e.Participant.Removed += new EventHandler(Participant_Removed);
            e.Participant.SourceAdded += new EventHandler<RTP_SourceEventArgs>(Participant_SourceAdded);
            e.Participant.SourceRemoved += new EventHandler<RTP_SourceEventArgs>(Participant_SourceRemoved);
            
            // Move processing to UI thread.
            this.BeginInvoke(new MethodInvoker(delegate(){
                TreeNode nodeParticipant = new TreeNode(e.Participant.CNAME);
                nodeParticipant.Tag = new RTP_ParticipantInfo(e.Participant);
                nodeParticipant.Nodes.Add("Sources");
                m_pParticipants.Nodes.Add(nodeParticipant);
            }));
        }
                                                
        #endregion

        #region method Participant_Removed

        /// <summary>
        /// This method is called when RTP remote participant has disjoined the multimedia session.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Participant_Removed(object sender,EventArgs e)
        {
            if(m_IsDisposed){
                return;
            }

            // Move processing to UI thread.
            this.BeginInvoke(new MethodInvoker(delegate(){
                TreeNode nodeParticipant = FindParticipantNode((RTP_Participant)sender);
                if(nodeParticipant != null){
                    nodeParticipant.Remove();
                }
            }));
        }

        #endregion

        #region method Participant_SourceAdded

        /// <summary>
        /// This method is called when participant creates new source. 
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Participant_SourceAdded(object sender,RTP_SourceEventArgs e)
        {
            if(m_IsDisposed){
                return;
            }

            e.Source.StateChanged += new EventHandler(Source_StateChanged);

            // Move processing to UI thread.
            this.BeginInvoke(new MethodInvoker(delegate(){
                TreeNode nodeParticipant = null;
                if(e.Source is RTP_Source_Remote){
                    nodeParticipant = FindParticipantNode(((RTP_Source_Remote)e.Source).Participant);
                }
                else{
                    nodeParticipant = FindParticipantNode(((RTP_Source_Local)e.Source).Participant);
                }
                TreeNode nodeSource = nodeParticipant.Nodes[0].Nodes.Add(e.Source.SSRC.ToString());
                nodeSource.Tag = new RTP_SourceInfo(e.Source);

                if(e.Source.State == RTP_SourceState.Active){
                    TreeNode nodeSourceStream = nodeSource.Nodes.Add("RTP Stream");
                    if(e.Source is RTP_Source_Local){
                        nodeSourceStream.Tag = new RTP_SendStreamInfo(((RTP_Source_Local)e.Source).Stream);
                    }
                    else{
                        nodeSourceStream.Tag = new RTP_ReceiveStreamInfo(((RTP_Source_Remote)e.Source).Stream);
                    }
                }
            }));
        }
                                
        #endregion

        #region method Source_StateChanged

        /// <summary>
        /// This method is called when participant source state changes.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Source_StateChanged(object sender,EventArgs e)
        {
            if(m_IsDisposed){
                return;
            }

            RTP_Source source = (RTP_Source)sender;
            if(source.State == RTP_SourceState.Disposed){
                return;
            }

            // Move processing to UI thread.
            this.BeginInvoke(new MethodInvoker(delegate(){
                TreeNode nodeParticipant = null;
                if(source is RTP_Source_Remote){
                    nodeParticipant = FindParticipantNode(((RTP_Source_Remote)source).Participant);
                }
                else{
                    nodeParticipant = FindParticipantNode(((RTP_Source_Local)source).Participant);
                }
                if(nodeParticipant != null){
                    foreach(TreeNode nodeSource in nodeParticipant.Nodes[0].Nodes){
                        if(nodeSource.Text == source.SSRC.ToString()){                            
                            if(source.State == RTP_SourceState.Active){
                                TreeNode nodeSourceStream = nodeSource.Nodes.Add("RTP Stream");
                                if(source is RTP_Source_Local){
                                    nodeSourceStream.Tag = new RTP_SendStreamInfo(((RTP_Source_Local)source).Stream);
                                }
                                else{
                                    nodeSourceStream.Tag = new RTP_ReceiveStreamInfo(((RTP_Source_Remote)source).Stream);
                                }
                            }

                            break;
                        }
                    }
                }
            }));
        }

        #endregion

        #region method Participant_SourceRemoved

        /// <summary>
        /// This method is called when participant closes source.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Participant_SourceRemoved(object sender,RTP_SourceEventArgs e)
        {
            if(m_IsDisposed){
                return;
            }

            // Get SSRC here, BeginInvoke is anynchronous and source may dispose at same time.
            uint ssrc = e.Source.SSRC;

            // Move processing to UI thread.
            this.BeginInvoke(new MethodInvoker(delegate(){
                TreeNode nodeParticipant = FindParticipantNode((RTP_Participant)sender);
                if(nodeParticipant != null){
                    foreach(TreeNode nodeSource in nodeParticipant.Nodes[0].Nodes){
                        if(nodeSource.Text == ssrc.ToString()){
                            nodeSource.Remove();
                            break;
                        }
                    }
                }
            }));
        }

        #endregion

        #region method m_pTimer_Tick

        private void m_pTimer_Tick(object sender,EventArgs e)
        {
            m_pParticipantData.Refresh();
            m_pGlobalSessionInfo.Refresh();
        }

        #endregion

        #region method wfrm_RTP_Debug_FormClosing

        private void wfrm_RTP_Debug_FormClosing(object sender,FormClosingEventArgs e)
        {
            m_IsDisposed = true;

            m_pSession.Error -= new EventHandler<LumiSoft.Net.ExceptionEventArgs>(m_pSession_Error);
            m_pSession.SessionCreated -= new EventHandler<LumiSoft.Net.EventArgs<RTP_Session>>(m_pSession_SessionCreated);
            m_pSession.NewParticipant -= new EventHandler<RTP_ParticipantEventArgs>(m_pSession_NewParticipant);
            m_pSession.LocalParticipant.SourceAdded -= new EventHandler<RTP_SourceEventArgs>(Participant_SourceAdded);
            m_pSession.LocalParticipant.SourceRemoved -= new EventHandler<RTP_SourceEventArgs>(Participant_SourceRemoved);
        }

        #endregion

        #endregion


        #region FindParticipantNode

        /// <summary>
        /// Searches specified participant tree node.
        /// </summary>
        /// <param name="participant">RTP participant.</param>
        /// <returns>Returns specified participant tree node or null if no matching node.</returns>
        private TreeNode FindParticipantNode(RTP_Participant participant)
        {
            if(participant == null){
                throw new ArgumentNullException("participant");
            }

            foreach(TreeNode node in m_pParticipants.Nodes){
                if(node.Text == participant.CNAME){
                    return node;
                }
            }

            return null;
        }

        #endregion


        #region Properties implementation

        /// <summary>
        /// Gets RTP session what UI debugs.
        /// </summary>
        public RTP_MultimediaSession Session
        {
            get{ return m_pSession; }
        }

        #endregion

    }
}
