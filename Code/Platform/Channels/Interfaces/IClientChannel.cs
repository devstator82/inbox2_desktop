using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IClientChannel : IDisposable
	{
		/// <summary>
		/// Gets or sets the hostname.
		/// </summary>
		/// <value>The hostname.</value>
		string Hostname { get; set; }

		/// <summary>
		/// Gets or sets the port.
		/// </summary>
		/// <value>The port.</value>
		int Port { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is secured.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is secured; otherwise, <c>false</c>.
		/// </value>
		bool IsSecured { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
		/// </value>
		bool IsEnabled { get; set; }

		/// <summary>
		/// Gets or sets the max concurrent connections.
		/// </summary>
		/// <value>The max concurrent connections.</value>
		int MaxConcurrentConnections { get; set; }

		/// <summary>
		/// Gets or sets the credentials provider.
		/// </summary>
		/// <value>The credentials provider.</value>
		IChannelCredentialsProvider CredentialsProvider { get; set; }

		/// <summary>
		/// Gets the communication protocol for this channel.
		/// </summary>
		/// <value>The protocol.</value>
		string Protocol { get; }
	}
}