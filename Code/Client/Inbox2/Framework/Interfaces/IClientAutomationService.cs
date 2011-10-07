using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Framework.Interfaces
{
	[ServiceContract]
	public interface IClientAutomationService
	{
		[OperationContract]
		void SendMessage(string context, string message, IEnumerable<SourceAddress> recipients);

		[OperationContract]
		void AddDocuments(string[] filenames);

		[OperationContract]
		void SendDocuments(string[] filenames, IEnumerable<SourceAddress> recipients);

		[OperationContract]
		void AddForLater(string content, int type);

		[OperationContract]
		void SendForLater(string content, int type, IEnumerable<SourceAddress> recipients);

		[OperationContract]
		void Search(string searchQuery);
	}
}