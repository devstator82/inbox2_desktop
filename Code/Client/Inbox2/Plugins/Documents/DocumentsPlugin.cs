using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Plugins.Documents.Helpers;

namespace Inbox2.Plugins.Documents
{
	[Export(typeof(PluginPackage))]
	public class DocumentsPlugin : PluginPackage
	{
		private readonly DocumentsState state;

		public override string Name
		{
			get { return "Documents"; }
		}

		public override IStatePlugin State
		{
			get { return state; }
		}

		public override IColumnPlugin Colomn
		{
			get { return new PluginHelper(state); }
		}

		public override IEnumerable<IOverviewPlugin> Overviews
		{
			get { return new IOverviewPlugin[] { new DocumentsViewHelper(state), new ImagesViewHelper(state) }; }
		}
		
		public DocumentsPlugin()
		{
			state = new DocumentsState();

			EventBroker.Subscribe(AppEvents.View, delegate(Document document)
				{
					string sourceFilename = ClientState.Current.Storage.ResolvePhysicalFilename(".", document.StreamName);
					string targetFileName = document.Filename;

					int i = 0;

					while (true)
					{
						if (i > 0)
						{
							targetFileName = String.Format("{0} ({1}){2}", 
								Path.GetFileNameWithoutExtension(document.Filename), i,
								Path.GetExtension(document.Filename));
						}

						if (File.Exists(Path.Combine(Path.GetTempPath(), targetFileName)))
						{
							i++;
							continue;
						}

						break;
					}

					var targetFilename = Path.Combine(Path.GetTempPath(), targetFileName);

					File.Copy(sourceFilename, targetFilename);

					new Process { StartInfo = new ProcessStartInfo(targetFilename) }.Start();
				});

			EventBroker.Subscribe(AppEvents.Save, delegate(Document document)
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
			});
		}
	}
}
