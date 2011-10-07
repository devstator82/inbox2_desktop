using System;
using System.IO;
using System.Linq;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Core.Threading.Handlers
{
	public static class DocumentsHandler
	{
		public static void Init()
		{
			EventBroker.Subscribe<Document>(AppEvents.DocumentReceived, DocumentReceived);			
		}

		public static void DocumentReceived(Document source)
		{
			try
			{
				var mailbox = VirtualMailBox.Current;
				var document = source;

				document.Crc32 = document.ContentStream.CalculateCrc32();
				document.Size = document.ContentStream.Length;

				var message = document.Message;

				bool isNewVersion;

				using (mailbox.Documents.ReaderLock)
				{
					Document document1 = document;

					isNewVersion = mailbox.Documents.Any(d => d.Crc32 == document1.Crc32);
				}

				if (isNewVersion == false)
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

					mailbox.Documents.Add(document);
				}
				else
				{
					// Replace document reference with instance from mailbox
					Document document1 = document;

					document = mailbox.Documents.First(d => d.Crc32 == document1.Crc32);
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

				mailbox.DocumentVersions.Add(version);

				document.Versions.Add(version);

				ClientState.Current.DataService.Save(version);

				if (message != null)
				{
					version.From = message.From;
					version.To = message.To;
					version.MessageId = message.MessageId;

					Thread.CurrentThread.ExecuteOnUIThread(() => message.Add(document));
				}								
			}
			finally
			{
				source.ContentStream.Dispose();
				source.ContentStream = null;
			}
		}
	}
}
