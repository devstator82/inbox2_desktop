using System;
using System.IO;
using Inbox2.Framework;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2ClientWorker.Core.Threading.Handlers
{
	internal static class DocumentsHandler
	{
		internal static void DocumentReceived(Document source)
		{
			try
			{
				var document = source;

				document.Crc32 = document.ContentStream.CalculateCrc32();
				document.Size = document.ContentStream.Length;

				var message = document.Message;
				var root = ClientState.Current.DataService.SelectBy<Document>(new { Crc32 = document.Crc32 });

				if (root == null)
				{
					document.StreamName = Guid.NewGuid().GetHash(12) + "_" + Path.GetExtension(document.Filename);

					// Save document to storage engine
					ClientState.Current.Storage.Write(
						".",
						document.StreamName,
						document.ContentStream);

					// New document, save stream and add to search/mailbox
					ClientState.Current.DataService.Save(document);

					ClientState.Current.Search.Store(document);
				}
				else
				{
					// Replace document reference with root to create new version for
					document = root;
				}

				// Create new version
				var version = new DocumentVersion
				{
					DocumentId = document.DocumentId.Value,
					SourceChannelId = document.SourceChannelId,
					TargetChannelId = document.TargetChannelId,
					StreamName = document.StreamName,
					Filename = document.Filename,
					DateReceived = document.DateReceived,
					DateSent = document.DateSent
				};

				if (message != null)
				{
					version.From = message.From;
					version.To = message.To;
					version.MessageId = message.MessageId;
				}

				ClientState.Current.DataService.Save(version);
			}
			finally
			{
				source.ContentStream.Dispose();
				source.ContentStream = null;
			}
		}
	}
}
