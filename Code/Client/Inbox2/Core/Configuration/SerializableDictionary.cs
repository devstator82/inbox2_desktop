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
using Inbox2.Framework.Utils.Text;

namespace Inbox2.Core.Configuration
{
	public class SerializableDictionary : Dictionary<string, object>, IXmlSerializable
	{
		public static string EncryptionKey { get; set; }

		protected bool encrypt;		

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.Read();
						
			encrypt = bool.Parse(reader.GetAttribute("encrypt"));
			reader.ReadStartElement("dictionary");
			
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				string key = reader.ReadElementString("key");

				string value = encrypt ? Crypto.Decrypt(reader.ReadElementString("value"), EncryptionKey) 
				               	: reader.ReadElementString("value");

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
			writer.WriteAttributeString("encrypt", encrypt.ToString());

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

					writer.WriteElementString("value", encrypt ? Crypto.Encrypt(sr.ReadToEnd(), EncryptionKey) : sr.ReadToEnd());
				}

				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
	}
}