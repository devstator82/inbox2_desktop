using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Inbox2.Platform.Framework.ServiceModel.Extensions
{
	public class CloudMessageInspector : IDispatchMessageInspector
	{
		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			return null;
		}

		public void BeforeSendReply(ref Message reply, object correlationState)
		{
			MessageBuffer messageBuffer = reply.CreateBufferedCopy(Int32.MaxValue);

			// Create string builders
			StringBuilder source = GetMessageBufferAsStringBuilder(messageBuffer);

			// In case of an error, do not modify the message
			if (source.ToString().StartsWith("<s:Fault"))
			{
				reply = messageBuffer.CreateMessage();
				return;
			}

			StringBuilder target = new StringBuilder();

			// Read source xml
			StringReader sr = new StringReader(source.ToString());
			XmlTextReader reader = new XmlTextReader(sr);

			// Write target xml
			StringWriter sw = new StringWriter(target);
			XmlManglingWriter writer = new XmlManglingWriter(sw);
			writer.WriteNode(reader, true);

			reader.Close();
			writer.Flush();
			writer.Close();

			// Set target message
			reply = GetStringBuilderAsNewMessage(reply, target);

			// Log message
			//MessageLogger.LogReply(ref reply, log);
		}

		/// <summary>
		/// Returns a StringBuilder containing the contents of the given message buffer.
		/// </summary>
		/// <param name="messageBuffer"></param>
		/// <returns></returns>
		protected StringBuilder GetMessageBufferAsStringBuilder(MessageBuffer messageBuffer)
		{
			XPathNavigator nav = messageBuffer.CreateNavigator();
			XmlDocument navDoc = new XmlDocument();
			navDoc.LoadXml(nav.OuterXml);

			StringBuilder sb = new StringBuilder();
			sb.Append(navDoc.ChildNodes[0].ChildNodes[1].InnerXml);

			return sb;
		}

		/// <summary>
		/// Creates a new Message from the given StringBuilder using the provided source message.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="sb"></param>
		/// <returns></returns>
		protected Message GetStringBuilderAsNewMessage(Message source, StringBuilder sb)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(sb.ToString());

			XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
			Message newMessage = Message.CreateMessage(source.Version, null, reader);

			//Copy headers
			newMessage.Headers.CopyHeadersFrom(source);

			foreach (string propertyKey in source.Properties.Keys)
				newMessage.Properties.Add(propertyKey, source.Properties[propertyKey]);

			// Copy newMessage to request
			return newMessage;
		}
	}
}
