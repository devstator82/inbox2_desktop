using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IReadStateChannel
	{
		/// <summary>
		/// Marks the message as read.
		/// </summary>
		/// <param name="message">The message.</param>
		void MarkRead(ChannelMessageHeader message);

		/// <summary>
		/// Marks the message as unread.
		/// </summary>
		/// <param name="message">The message.</param>
		void MarkUnread(ChannelMessageHeader message);

		/// <summary>
		/// Marks the message as deleted.
		/// </summary>
		/// <param name="message">The message.</param>
		void MarkDeleted(ChannelMessageHeader message);

		/// <summary>
		/// Marks the message as starred.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="starred"></param>
		void SetStarred(ChannelMessageHeader message, bool starred);

		/// <summary>
		/// Purges the message from trash.
		/// </summary>
		void Purge(ChannelMessageHeader message);

		/// <summary>
		/// Creates a folder with the given name.
		/// </summary>
		/// <param name="folderName"></param>
		/// <returns></returns>
		ChannelFolder CreateFolder(string folderName);

		/// <summary>
		/// Moves the message to the given folder.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="folder"></param>
		void MoveToFolder(ChannelMessageHeader message, ChannelFolder folder);

		/// <summary>
		/// Copies the message to the given folder.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="folder"></param>
		void CopyToFolder(ChannelMessageHeader message, ChannelFolder folder);

		/// <summary>
		/// Removes the message from the given folder.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="folder"></param>
		void RemoveFromFolder(ChannelMessageHeader message, ChannelFolder folder);
	}
}
