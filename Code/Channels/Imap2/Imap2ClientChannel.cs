using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Inbox2.Platform.Channels.Connections;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Exceptions;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Parsing;
using LumiSoft.Net.IMAP;
using LumiSoft.Net.IMAP.Client;
using LumiSoft.Net.MIME;

namespace Inbox2.Channels.Imap2
{
	public class Imap2ClientChannel : PoolableConnectionBase<Imap2Connection>, IClientInputChannel, IReversePagableChannel, IReadStateChannel, IProgressChannel, ILabelsChannel, IPushChannel
	{
		#region Properties

		public long StartIndex { get; set; }

		public long EndIndex { get; set; }

		public long PageSize { get; set; }		

		public ChannelProgressDelegate BytesRead { get; set; }

		public ChannelProgressDelegate BytesWritten { get; set; }

		public override string Protocol
		{
			get { return "IMAP"; }
		}

		public string SourceAdress
		{
			get
			{
				string username = CredentialsProvider.GetCredentials().Claim;

				return username.Contains("@") ? username : String.Format("{0}@{1}", username, Hostname);
			}
		}

		public string AuthMessage { get; private set; }

		public LabelsSupport LabelsSupport { get { return LabelsSupport.Folders; } }

		#endregion

		public ConnectResult Connect()
		{
			var credentials = CredentialsProvider.GetCredentials();

			Imap2Connection instance;

			// Get the connection for the current server and mailbox (username)
			bool isFree = ConnectionPool<Imap2Connection>.AcquireLock(out instance, Hostname, Port, IsSecured, credentials.Claim, credentials.Evidence);

			if (isFree == false)
				return ConnectResult.Delay;

			connection = instance;

			if (instance == null)
				throw new ChannelFunctionalException("Connection was null in Connect");

			try
			{
				connection.Open();
				connection.Authenticate();

				return ConnectResult.Success;
			}
			catch (ChannelAuthenticationException ex)
			{
				AuthMessage = ex.Message;

				return ConnectResult.AuthFailure;
			}
		}

		public IEnumerable<ChannelFolder> GetFolders()
		{
			if (connection == null)
				throw new ChannelFunctionalException("Connection was null in GetFolders");

			var folders = connection.Client.GetFolders();

			if (folders != null && folders.Length > 0)
			{
				foreach (var folder in folders)
					yield return new ChannelFolder(folder, folder, FolderHelper.GetImapFolderType(folder));
			}
		}

		public long GetNumberOfItems()
		{
			return connection.Client.UIDNext -1;
		}

		public void SelectFolder(ChannelFolder folder)
		{
			connection.Client.SelectFolder(folder.FolderId);
		}

		public IEnumerable<ChannelMessageHeader> GetHeaders()
		{
			var set = new IMAP_SequenceSet(String.Format("{0}:{1}", StartIndex <= 1 ? 1 : StartIndex, EndIndex < 0 ? "*" : EndIndex.ToString()));

			return FetchImapSet(set);
		}

		public IEnumerable<ChannelMessage> GetMessage(ChannelMessageHeader header)
		{
			connection.Client.SelectFolder(header.SourceFolder);

			using (var ms = new MemoryStream())
			{
				connection.Client.FetchMessage(Int32.Parse(header.MessageNumber), ms);

				yield return ChannelMessageParser.From(ms, header);
			}
		}

		public bool Disconnect()
		{
			if (connection != null)
				ConnectionPool<Imap2Connection>.ReleaseLock(connection);

			return true;
		}

		public IEnumerable<ChannelMessageHeader> BeginWaitForPush()
		{
			var commands = connection.Client.Idle();

			foreach (var command in commands)
			{
				if (command.ToUpper().Contains("EXISTS"))
				{
					var parts = command.Split(new[] { " " }, StringSplitOptions.None);

					foreach (var header in FetchImapSet(new IMAP_SequenceSet(String.Format("{0}:*", parts[0]))))
						yield return header;
				}
			}

			yield break;
		}

		public void EndWaitForPush()
		{
			connection.Client.IdleDone();					
		}

		IEnumerable<ChannelMessageHeader> FetchImapSet(IMAP_SequenceSet set)
		{
			var result = connection.Client.FetchMessages(set,
				IMAP_FetchItem_Flags.UID | IMAP_FetchItem_Flags.MessageFlags |
				IMAP_FetchItem_Flags.InternalDate | IMAP_FetchItem_Flags.Size |
				IMAP_FetchItem_Flags.Header | IMAP_FetchItem_Flags.Envelope, false, true);

			if (result != null && result.Length > 0)
			{
				foreach (var item in result)
				{
					var header = new ChannelMessageHeader();
					header.MessageNumber = item.UID.ToString();
					header.SourceFolder = connection.Client.SelectedFolder;
					header.Size = item.Size;
					header.Context = item.Envelope.Subject;
					header.DateReceived = item.Envelope.Date;
					header.IsRead = !item.IsNewMessage;

					// Message is starred
					if ((item.MessageFlags & IMAP_MessageFlags.Flagged) == IMAP_MessageFlags.Flagged)
						header.IsStarred = true;

					header.MessageIdentifier = item.Envelope.MessageID;
					header.InReplyTo = item.Envelope.InReplyTo;
					
					var provider = new MIME_h_Provider();					
					var headers = new MIME_h_Collection(provider);
					headers.Parse(item.HeaderData);

					var i2mpMessageId = headers.GetFirst("x-i2mp-messageid");
					if (i2mpMessageId != null)
					{
						var unstructured = (MIME_h_Unstructured) i2mpMessageId;
						header.Metadata.i2mpMessageId = unstructured.Value;
					}

					yield return header;
				}
			}
		}

		public IClientInputChannel Clone()
		{
			return new Imap2ClientChannel
         		{
         			Hostname = Hostname,
         			Port = Port,
         			IsSecured = IsSecured,
         			MaxConcurrentConnections = MaxConcurrentConnections,
         			CredentialsProvider = CredentialsProvider
         		};
		}

		public virtual void MarkRead(ChannelMessageHeader message)
		{
			if (String.IsNullOrEmpty(message.SourceFolder)) return;

			connection.Client.SelectFolder(message.SourceFolder);
			connection.Client.StoreMessageFlags(new IMAP_SequenceSet(message.MessageNumber), IMAP_MessageFlags.Seen, true);
		}

		public virtual void MarkUnread(ChannelMessageHeader message)
		{
			if (String.IsNullOrEmpty(message.SourceFolder)) return;

			connection.Client.SelectFolder(message.SourceFolder);
			connection.Client.StoreMessageFlags(new IMAP_SequenceSet(message.MessageNumber), IMAP_MessageFlags.None, true);
		}

		public virtual void MarkDeleted(ChannelMessageHeader message)
		{
			if (String.IsNullOrEmpty(message.SourceFolder)) return;

			connection.Client.SelectFolder(message.SourceFolder);
			connection.Client.StoreMessageFlags(new IMAP_SequenceSet(message.MessageNumber), IMAP_MessageFlags.Deleted, true);
		}

		public virtual void SetStarred(ChannelMessageHeader message, bool starred)
		{
			if (String.IsNullOrEmpty(message.SourceFolder)) return;

			IMAP_MessageFlags flags = message.IsRead ? IMAP_MessageFlags.Seen : IMAP_MessageFlags.None;

			if (starred) flags = (flags | IMAP_MessageFlags.Flagged);

			connection.Client.SelectFolder(message.SourceFolder);
			connection.Client.StoreMessageFlags(new IMAP_SequenceSet(message.MessageNumber), flags, true);
		}

		public virtual void Purge(ChannelMessageHeader message)
		{
			// todo this is not working correctly with gmail because it moves stuff to the All Mail folder?
			//if (String.IsNullOrEmpty(message.SourceFolder)) return;

			//connection.Client.SelectFolder(message.SourceFolder);
			//connection.Client.DeleteMessages(new IMAP_SequenceSet(message.MessageNumber), true);
		}

		public ChannelFolder CreateFolder(string folderName)
		{
			connection.Client.CreateFolder(folderName);

			return new ChannelFolder(folderName, folderName, FolderHelper.GetImapFolderType(folderName));
		}

		public virtual void MoveToFolder(ChannelMessageHeader message, ChannelFolder folder)
		{
			if (String.IsNullOrEmpty(message.SourceFolder)) return;

			connection.Client.SelectFolder(message.SourceFolder);
			connection.Client.MoveMessages(new IMAP_SequenceSet(message.MessageNumber), folder.FolderId, true);
		}

		public void CopyToFolder(ChannelMessageHeader message, ChannelFolder folder)
		{
			if (String.IsNullOrEmpty(message.SourceFolder)) return;

			connection.Client.SelectFolder(message.SourceFolder);
			connection.Client.CopyMessages(new IMAP_SequenceSet(message.MessageNumber), folder.FolderId, true);
		}

		public void RemoveFromFolder(ChannelMessageHeader message, ChannelFolder folder)
		{
			if (String.IsNullOrEmpty(message.SourceFolder)) return;

			connection.Client.SelectFolder(folder.FolderId);
			connection.Client.DeleteMessages(new IMAP_SequenceSet(message.MessageNumber), true);			
		}

		public void AddLabel(ChannelMessageHeader message, string labelname)
		{
			connection.Client.SelectFolder(message.SourceFolder);
			connection.Client.CopyMessages(new IMAP_SequenceSet(message.MessageNumber), labelname, true);
		}

		public void RemoveLabel(string messagenumber, string labelname)
		{
			connection.Client.SelectFolder(labelname);
			connection.Client.DeleteMessages(new IMAP_SequenceSet(messagenumber), true);	
		}

		public override void Dispose()
		{
			if (connection != null)
			{
				Disconnect();
				
				connection.Close();
			}
		}
	}
}
