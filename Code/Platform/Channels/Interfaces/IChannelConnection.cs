using System;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IChannelConnection : IDisposable
	{
		/// <summary>
		/// Gets or sets the bytes read delegate.
		/// </summary>
		/// <value>The bytes read.</value>
		ChannelProgressDelegate BytesRead { get; set; }

		/// <summary>
		/// Gets or sets the bytes written delegate.
		/// </summary>
		/// <value>The bytes written.</value>
		ChannelProgressDelegate BytesWritten { get; set; }

		/// <summary>
		/// Gets the unique id of this instance.
		/// </summary>
		/// <value>The unique id.</value>
		Guid UniqueId { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is connected.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
		/// </value>
		bool IsConnected { get; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is locked.
		/// </summary>
		/// <value><c>true</c> if this instance is locked; otherwise, <c>false</c>.</value>
		bool IsLocked { get; set; }

		/// <summary>
		/// Gets the hostname.
		/// </summary>
		/// <value>The hostname.</value>
		string Hostname { get; }

		/// <summary>
		/// Gets the port.
		/// </summary>
		/// <value>The port.</value>
		int Port { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is secured.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is secured; otherwise, <c>false</c>.
		/// </value>
		bool IsSecured { get; }

		/// <summary>
		/// Gets the mail box.
		/// </summary>
		/// <value>The mail box.</value>
		string Username { get;  }

		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <value>The password.</value>
		string Password { get; }

		/// <summary>
		/// Gets the state of the channel.
		/// </summary>
		/// <value>The state of the channel.</value>
		ChannelState ChannelState { get; }

		/// <summary>
		/// Opens this instance.
		/// </summary>
		void Open();

		/// <summary>
		/// Closes this instance.
		/// </summary>
		void Close();

		/// <summary>
		/// Authenticates this instance with the given credentials.
		/// </summary>
		void Authenticate();
	}
}