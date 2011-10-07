using System;
using System.Collections.Generic;
using System.Text;

using LumiSoft.Net.SIP.Stack;

namespace LumiSoft.Net.SIP.Proxy
{
    /// <summary>
    /// This class represents SIP gateway to other system.
    /// </summary>
    public class SIP_Gateway
    {
        private string m_Transport = SIP_Transport.UDP;
        private string m_Host      = "";
        private int    m_Port      = 5060;
        private string m_Realm     = "";
        private string m_UserName  = "";
        private string m_Password  = "";

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="transport">Transport to use.</param>
        /// <param name="host">Remote gateway host name or IP address.</param>
        /// <param name="port">Remote gateway port.</param>
        public SIP_Gateway(string transport,string host,int port) : this(transport,host,port,"","","")
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="transport">Transport to use.</param>
        /// <param name="host">Remote gateway host name or IP address.</param>
        /// <param name="port">Remote gateway port.</param>
        /// <param name="realm">Remote gateway realm.</param>
        /// <param name="userName">Remote gateway user name.</param>
        /// <param name="password">Remote gateway password.</param>
        public SIP_Gateway(string transport,string host,int port,string realm,string userName,string password)
        {
            this.Transport = transport;
            this.Host      = host;
            this.Port      = port;
            this.Realm     = realm;
            this.UserName  = userName;
            this.Password  = password;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets or sets transport.
        /// </summary>
        /// <exception cref="ArgumentException">Is raised when invalid value passed.</exception>
        public string Transport
        {
            get{ return m_Transport; }

            set{
                if(string.IsNullOrEmpty(value)){
                    throw new ArgumentException("Value cant be null or empty !");
                }

                m_Transport = value;
            }
        }

        /// <summary>
        /// Gets or sets remote gateway host name or IP address.
        /// </summary>
        /// <exception cref="ArgumentException">Is raised when invalid value passed.</exception>
        public string Host
        {
            get{ return m_Host; }

            set{
                if(string.IsNullOrEmpty(value)){
                    throw new ArgumentException("Value cant be null or empty !");
                }

                m_Host = value;
            }
        }

        /// <summary>
        /// Gets or sets remote gateway port.
        /// </summary>
        /// <exception cref="ArgumentException">Is raised when invalid value passed.</exception>
        public int Port
        {
            get{ return m_Port; }
            
            set{
                if(value < 1){
                    throw new ArgumentException("Value must be >= 1 !");
                }

                m_Port = value;
            }
        }

        /// <summary>
        /// Gets or sets remote gateway realm(domain).
        /// </summary>
        public string Realm
        {
            get{ return m_Realm; }

            set{
                if(value == null){
                    m_Realm = "";
                }

                m_Realm = value;
            }
        }

        /// <summary>
        /// Gets or sets remote gateway user name.
        /// </summary>
        public string UserName
        {
            get{ return m_UserName; }

            set{
                if(value == null){
                    m_UserName = "";
                }

                m_UserName = value;
            }
        }

        /// <summary>
        /// Gets or sets remote gateway password.
        /// </summary>
        public string Password
        {
            get{ return m_Password; }

            set{
                if(value == null){
                    m_Password = "";
                }

                m_Password = value;
            }
        }

        #endregion

    }
}
