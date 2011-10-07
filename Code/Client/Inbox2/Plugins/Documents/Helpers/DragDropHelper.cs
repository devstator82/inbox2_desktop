using System;
using System.IO;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Documents.Helpers
{
	internal static class DragDropHelper
	{
		public static void UploadFile(string filename)
		{
			var document = new Document
			{
				Filename = Path.GetFileName(filename),
				ContentType = ContentType.Attachment,
				DateCreated = DateTime.Now,
				DateSent = DateTime.Now,
				DocumentFolder = Folders.SentItems
			};

			document.StreamName = Guid.NewGuid().GetHash(12) + "_" + Path.GetExtension(document.Filename);
			document.ContentStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

			EventBroker.Publish(AppEvents.DocumentReceived, document);
		}
	}
}
