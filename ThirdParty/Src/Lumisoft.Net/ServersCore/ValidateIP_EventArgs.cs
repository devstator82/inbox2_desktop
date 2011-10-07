using System;
using System.Net;

namespace LumiSoft.Net
{
	/// <summary>
	/// Provides data for the ValidateIPAddress event for servers.
	/// </summary>
	public class ValidateIP_EventArgs
	{
		private IPEndPoint m_pLocalEndPoint  = null;
		private IPEndPoint m_pRemoteEndPoint = null;
		private bool       m_Validated       = true;
		private object     m_SessionTag      = null;
		private string     m_ErrorText       = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="localEndPoint">Server IP.</param>
		/// <param name="remoteEndPoint">Connected client IP.</param>
		public ValidateIP_EventArgs(IPEndPoint localEndPoint,IPEndPoint remoteEndPoint)
		{
			m_pLocalEndPoint  = localEndPoint;
			m_pRemoteEndPoint = remoteEndPoint;
		}

		#region Properties Implementation

		/// <summary>
		/// IP address of computer, which is sending mail to here.
		/// </summary>
		public string ConnectedIP
		{
			get{ return m_pRemoteEndPoint.Address.ToString(); }
		}

		/// <summary>
		/// Gets local endpoint.
		/// </summary>
		public IPEndPoint LocalEndPoint
		{
			get{ return m_pLocalEndPoint; }
		}

		/// <summary>
		/// Gets remote endpoint.
		/// </summary>
		public IPEndPoint RemoteEndPoint
		{
			get{ return m_pRemoteEndPoint; }
		}


		/// <summary>
		/// Gets or sets if IP is allowed access.
		/// </summary>
		public bool Validated
		{
			get{ return m_Validated; }

			set{ m_Validated = value; }
		}

		/// <summary>
		/// Gets or sets user data what is stored to session.Tag property.
		/// </summary>
		public object SessionTag
		{
			get{ return m_SessionTag; }

			set{ m_SessionTag = value; }
		}

		/// <summary>
		/// Gets or sets error text what is sent to connected socket. NOTE: This is only used if Validated = false.
		/// </summary>
		public string ErrorText
		{
			get{ return m_ErrorText; }

			set{ m_ErrorText = value; }
		}

		#endregion

	}
}
