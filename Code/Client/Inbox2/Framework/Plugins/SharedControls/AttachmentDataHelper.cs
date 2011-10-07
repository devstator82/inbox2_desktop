using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Framework.Plugins.SharedControls
{
	[DataContract]
	public class AttachmentDataHelper
	{
		[DataMember]
		public long? DocumentId { get; set; }

		[DataMember]
		public string Filename { get; set; }

		[DataMember]
		public string Streamname { get; set; }

		private readonly FileInfo fileInfo;

		public FileInfo FileInfo
		{
			get { return fileInfo; }
		}

		public AttachmentDataHelper()
		{
			fileInfo = new FileInfo(Streamname);
		}

		public AttachmentDataHelper(string filename, string streamname)
		{
			Filename = filename;
			Streamname = streamname;
			fileInfo = new FileInfo(Streamname);
		}

		public AttachmentDataHelper(Document document)
		{
			DocumentId = document.DocumentId;
			Filename = document.Filename;
			Streamname = ClientState.Current.Storage.ResolvePhysicalFilename(".", document.StreamName);
			fileInfo = new FileInfo(Streamname);
		}
	}
}