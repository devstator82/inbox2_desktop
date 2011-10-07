using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using LumiSoft.Net.Mail;

namespace Inbox2.Channels.Smtp
{
	public static class SourceAddressExtensions
	{
		public static Mail_t_MailboxList ToMailBoxList(this SourceAddress address)
		{
			return new Mail_t_MailboxList { address.ToMailBox() };
		}

		public static Mail_t_AddressList ToAddressList(this SourceAddress address)
		{
			return new Mail_t_AddressList { address.ToMailBox() };
		}

		public static Mail_t_Mailbox ToMailBox(this SourceAddress address)
		{
			return new Mail_t_Mailbox(address.DisplayName, address.Address);
		}
	}
}
