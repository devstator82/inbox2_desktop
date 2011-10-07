using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox.Mappers
{
	public class MessageContentMapper : IContentMapper
	{
		private readonly Message message;

		public MessageContentMapper(Message message)
		{
			this.message = message;
		}

		public string PropertyName
		{
			get { return "BodyPreview"; }
		}

		public string GetContent()
		{
			return new ClientMessageAccess(message)
				.GetBestBodyMatch(TextConversion.ToText);
		}
	}
}
