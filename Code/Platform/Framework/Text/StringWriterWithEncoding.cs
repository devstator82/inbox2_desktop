using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Text
{
	public class StringWriterWithEncoding : StringWriter
	{
		private readonly Encoding encoding;

		public StringWriterWithEncoding(Encoding encoding)
		{
			this.encoding = encoding;
		}

		public override Encoding Encoding
		{
			get { return encoding; }
		}
	}

}
