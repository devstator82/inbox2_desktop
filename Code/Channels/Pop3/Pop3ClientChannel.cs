using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inbox2.Platform.Channels.Connections;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Exceptions;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Parsing;
using LumiSoft.Net.POP3.Client;

namespace Inbox2.Channels.Pop3
{
	public class Pop3ClientChannel : PoolableConnectionBase<Pop3Connection>, IClientInputChannel, IDisposable, IPagableChannel
	{
		private POP3_ClientMessageCollection messages;

		public long StartIndex { get; set; }

		public long EndIndex { get; set; }

		public long PageSize { get; set; }

		public override string Protocol
		{
			get { return "POP3";  }
		}

		public string SourceAdress
		{
			get
			{
				string username = CredentialsProvider.GetCredentials().Claim;

				return username.Contains("@") ? username : String.Format("{0}@{1}", username, Hostname);
			}
		}

		public string AuthMessage
		{
			get; private set;
		}

		public ConnectResult Connect()
		{
			Pop3Connection instance;

			var credentials = CredentialsProvider.GetCredentials();

			// Get the connection for the current server and user
			bool isFree = ConnectionPool<Pop3Connection>.AcquireLock(out instance, Hostname, Port, IsSecured, credentials.Claim, credentials.Evidence);

			if (isFree == false)
				return ConnectResult.Delay;

			connection = instance;

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
			yield return new ChannelFolder("inbox", "inbox", ChannelFolderType.Inbox);
		}

		public void SelectFolder(ChannelFolder folder)
		{			
		}

		public IEnumerable<ChannelMessageHeader> GetHeaders()
		{
			BuildMessages();			

			foreach (var item in messages.Skip((int)StartIndex).Take((int)PageSize))
			{
				yield return new ChannelMessageHeader { MessageNumber = item.SequenceNumber.ToString(), Size = item.Size };
			}
		}

		public IEnumerable<ChannelMessage> GetMessage(ChannelMessageHeader header)
		{
			using (var ms = new MemoryStream())
			{
				connection.Client.GetMessage(Int32.Parse(header.MessageNumber), ms);

				yield return ChannelMessageParser.From(ms, header);
			}
		}

		public long GetNumberOfItems()
		{
			BuildMessages();			

			return messages.Count;
		}

		public bool Disconnect()
		{
			if (connection != null)
				ConnectionPool<Pop3Connection>.ReleaseLock(connection);

			return true;
		}

		public IClientInputChannel Clone()
		{
			Pop3ClientChannel channel = new Pop3ClientChannel();
			channel.Hostname = Hostname;
			channel.Port = Port;
			channel.IsSecured = IsSecured;
			channel.MaxConcurrentConnections = MaxConcurrentConnections;
			channel.CredentialsProvider = CredentialsProvider;

			return channel;
		}

		protected void BuildMessages()
		{
			// Only fetch messages when list is null because otherwise we would end up
			// downloading the mailbox headers twice, which can be quite annoying with large mailboxes
			if (messages == null)
				messages = connection.Client.List();			
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
