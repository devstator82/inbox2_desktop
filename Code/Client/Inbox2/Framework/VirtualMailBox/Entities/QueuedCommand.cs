using System;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass(TableName = "CommandQueue")]
	public class QueuedCommand
	{
		[PrimaryKey]
		public long? CommandId { get; set; }

		[Persist]
		public AppCommands CommandType { get; set; }

		[Persist]
		public ExecutionTrigger TriggerType { get; set; }

		[Persist]
		public ExecutionStatus Status { get; set; }

		[Persist]
		public string Target { get; set; }		

		[Persist]
		public string Value { get; set; }

		[Persist]
		public ModifyAction ModifyAction { get; set; }

		[Persist]
		public string LastResult { get; set; }

		[Persist]
		public int MaxRetries { get; set; }

		[Persist]
		public int ActualRetries { get; set; }

		[Persist]
		public DateTime DateCreated { get; set; }

		[Persist]
		public DateTime? DateScheduled { get; set; }

		[Persist]
		public DateTime? DateStarted { get; set; }

		[Persist]
		public DateTime? DateUpdated { get; set; }

		public override string ToString()
		{
			return String.Format("[{0} {1} {2}]", CommandId, CommandType, TriggerType);
		}
	}
}