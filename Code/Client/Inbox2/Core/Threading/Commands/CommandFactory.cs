using System;
using System.Linq;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.CloudApi.Enumerations;

namespace Inbox2.Core.Threading.Commands
{
	public static class CommandFactory
	{
		public static CommandBase CreateCommand(QueuedCommand command)
		{
			switch (command.CommandType)
			{
				case AppCommands.SendMessage:
					return new SendMessageCommand(GetMessage(command));
				
				case AppCommands.SendStatusUpdate:
					return new SendStatusUpdateCommand(GetUserStatus(command));

				case AppCommands.SyncMessage:
					return new SyncEntityCommand(Entities.Messages, GetMessage(command).MessageKey, command);

				case AppCommands.SyncPerson:
					return new SyncEntityCommand(Entities.Persons, GetPerson(command).PersonKey, command);

				case AppCommands.SyncStatusUpdate:
					return new SyncEntityCommand(Entities.StatusUpdates, GetUserStatus(command).StatusKey, command);
			}

			throw new ArgumentException(String.Format("Unexpected command type {0}", command.CommandType));
		}

		static Message GetMessage(QueuedCommand command)
		{
			var messageid = Int64.Parse(command.Target.Substring(1));
			Message message;

			using (VirtualMailBox.Current.Messages.ReaderLock)
				message = VirtualMailBox.Current.Messages.FirstOrDefault(m => m.MessageId == messageid);

			if (message == null)
				throw new ApplicationException(String.Format("The given source entity with key {0} was not found", command.Target));

			return message;
		}

		static UserStatus GetUserStatus(QueuedCommand command)
		{
			var statusId = Int64.Parse(command.Target.Substring(1));
			UserStatus status;

			using (VirtualMailBox.Current.StatusUpdates.ReaderLock)
				status = VirtualMailBox.Current.StatusUpdates.FirstOrDefault(s => s.StatusId == statusId);

			if (status == null) 
				throw new ApplicationException(String.Format("The given source entity with key {0} was not found", command.Target));

			return status;
		}

		static Person GetPerson(QueuedCommand command)
		{
			var personId = Int64.Parse(command.Target.Substring(1));
			Person person;

			using (VirtualMailBox.Current.Persons.ReaderLock)
				person = VirtualMailBox.Current.Persons.FirstOrDefault(s => s.PersonId == personId);

			if (person == null)
				throw new ApplicationException(String.Format("The given source entity with key {0} was not found", command.Target));

			return person;
		}
	}
}
