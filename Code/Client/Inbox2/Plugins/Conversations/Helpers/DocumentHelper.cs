using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Plugins.Conversations.Helpers
{
	internal static class DocumentHelper
	{
		public static void Open(Document document)
		{
			string filename = ClientState.Current.Storage.ResolvePhysicalFilename(".", document.StreamName);

			new Process { StartInfo = new ProcessStartInfo(filename) }.Start();
		}

		public static void Save(Document document)
		{
			var dialog = new System.Windows.Forms.SaveFileDialog();
			dialog.FileName = document.Filename;
			dialog.Filter = String.Format("{0} files (*.{0})|*.{0}|All files (*.*)|*.*",
				Path.GetExtension(document.Filename));

			var result = dialog.ShowDialog();

			if (result == System.Windows.Forms.DialogResult.OK)
			{
				string filename = ClientState.Current.Storage.ResolvePhysicalFilename(".", document.StreamName);
				File.Copy(filename, dialog.FileName);
			}
		}
	}
}
