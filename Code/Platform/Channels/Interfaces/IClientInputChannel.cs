using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IClientInputChannel : IClientChannel
	{
		string SourceAdress { get; }

		string AuthMessage { get; }

		/// <summary>
		/// Connects this instance.
		/// </summary>
		/// <returns></returns>
		ConnectResult Connect();

		/// <summary>
		/// Gets all the folders.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ChannelFolder> GetFolders();

		/// <summary>
		/// Selects the folder.
		/// </summary>
		/// <param name="folder">The folder to select.</param>
		void SelectFolder(ChannelFolder folder);

		/// <summary>
		/// Gets the headers.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ChannelMessageHeader> GetHeaders();

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <param name="header">The header.</param>
		/// <returns></returns>
		IEnumerable<ChannelMessage> GetMessage(ChannelMessageHeader header);

		/// <summary>
		/// Disconnects this instance.
		/// </summary>
		/// <returns></returns>
		bool Disconnect();

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns></returns>
		IClientInputChannel Clone();
	}
}