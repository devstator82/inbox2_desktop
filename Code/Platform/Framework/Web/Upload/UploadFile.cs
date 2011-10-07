// http://aspnetupload.com
// Copyright © 2009 Krystalware, Inc.
//
// This work is licensed under a Creative Commons Attribution-Share Alike 3.0 United States License
// http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Inbox2.Platform.Framework.Web.Upload
{
    public class UploadFile
    {
		public Stream Data { get; set; }

		public string FieldName { get; set; }

		public string FileName { get; set; }

		public string ContentType { get; set; }

		public UploadFile(Stream data, string fileName, string contentType)
			: this(data, null, fileName, contentType)
		{
			
		}

    	public UploadFile(Stream data, string fieldName, string fileName, string contentType)
        {
            Data = data;
            FieldName = fieldName;
            FileName = fileName;
            ContentType = contentType;
        }
    }
}
