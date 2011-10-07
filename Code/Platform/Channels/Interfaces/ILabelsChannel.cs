using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface ILabelsChannel
	{
		LabelsSupport LabelsSupport { get; }

		void AddLabel(ChannelMessageHeader message, string labelname);

		void RemoveLabel(string messagenumber, string labelname);
	}
}
