using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	public class ChannelConfig
	{
		[PrimaryKey] public long? ChannelConfigId { get; set; }

		[Persist] public string ChannelKey { get; set; }

		[Persist] public ChannelConnection ChannelConnection { get; set; }

		[Persist] public string DisplayName { get; set; }

		[Persist] public string Hostname { get; set; }

		[Persist] public int? Port { get; set; }

        [Persist] public bool SSL { get; set; }

		[Persist] public string Username { get; set; }

		[Persist] public string Password { get; set; }

        [Persist] public string OutgoingHostname { get; set; }

        [Persist] public int? OutgoingPort { get; set; }

        [Persist] public bool OutgoingSSL { get; set; }

        [Persist] public string OutgoingUsername { get; set; }

        [Persist] public string OutgoingPassword { get; set; }

        [Persist] public string Type { get; set; }

		[Persist] public bool IsVisible { get; set; }

        [Persist] public bool IsDefault { get; set; }

        [Persist] public bool IsManuallyCustomized { get; set; }

		[Persist] public DateTime DateCreated { get; set; }

		[Persist] public DateTime? DateModified { get; set; }
	}
}
