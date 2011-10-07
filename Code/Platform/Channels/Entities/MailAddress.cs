using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	public class MailAddress : System.Net.Mail.MailAddress, IComparable, IComparable<MailAddress>
	{
		public MailAddress(System.Net.Mail.MailAddress mailaddress) : base(mailaddress.Address, mailaddress.DisplayName)
		{
			
		}

		public MailAddress(string address) : base(address)
		{
		}

		public MailAddress(string address, string displayName) : base(address, displayName)
		{
		}

		public MailAddress(string address, string displayName, Encoding displayNameEncoding) : base(address, displayName, displayNameEncoding)
		{
		}

		public int CompareTo(object obj)
		{
			return CompareTo((MailAddress)obj);
		}

		public int CompareTo(MailAddress other)
		{
			return Address.CompareTo(other.Address);
		}
	}
}