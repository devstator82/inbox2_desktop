using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interop
{
	public class ExplorerSelectedFile : SelectedItem
	{
		public string Filename { get; set; }

		public string Location { get; set; }

		public Point Coordinates { get; set; }

		public FileInfo GetFileInfo()
		{
			if (Path.HasExtension(Filename))
				return new FileInfo(Path.Combine(Location, Filename));

			// Get file-extension for selected item as this is not returned
			string[] files = Directory.GetFiles(Location, Filename + ".*", SearchOption.TopDirectoryOnly);

			// Probably a folder
			if (files.Length == 0)
				return null;

			if (files.Length > 1)
			{
				// ToDo: match on filesize? (Multiple files with same filename but different extensions on desktop are not supported yet)
				return null;
			}

			return new FileInfo(Path.Combine(Location, files[0]));
		}
	}
}
