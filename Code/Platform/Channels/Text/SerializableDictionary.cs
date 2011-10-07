using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Inbox2.Platform.Channels.Text
{
	[Serializable]
	public class SerializableDictionary : Dictionary<string, object>, IXmlSerializable
	{
		public SerializableDictionary()
		{
		}

		protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.Read();
						
			reader.ReadStartElement("dictionary");
			
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				string key = reader.ReadElementString("key");

				string value = reader.ReadElementString("value");

				object instance = null;

				using (MemoryStream ms = new MemoryStream())
				using (StreamWriter sw = new StreamWriter(ms))
				{
					sw.Write(value);
					sw.Flush();

					ms.Seek(0, SeekOrigin.Begin);

					NetDataContractSerializer ser = new NetDataContractSerializer();
					instance = ser.Deserialize(ms);
				}

				Debug.Assert(instance != null, "Deserialization of item failed");

				reader.ReadEndElement();
				reader.MoveToContent();

				Add(key, instance);
			}
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("dictionary");

			foreach (string key in Keys)
			{
				object value = this[key];
				writer.WriteStartElement("item");
				writer.WriteElementString("key", key);

				using (MemoryStream ms = new MemoryStream())
				using (StreamReader sr = new StreamReader(ms))
				{
					NetDataContractSerializer ser = new NetDataContractSerializer();
					ser.Serialize(ms, value);

					ms.Seek(0, SeekOrigin.Begin);

					writer.WriteElementString("value", sr.ReadToEnd());
				}

				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		public override string ToString()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				XmlTextWriter w = new XmlTextWriter(ms, System.Text.Encoding.UTF8);

				WriteXml(w);

				ms.Seek(0, SeekOrigin.Begin);

				using (StreamReader sr = new StreamReader(ms))
					return sr.ReadToEnd();
			}
		}
	}
}