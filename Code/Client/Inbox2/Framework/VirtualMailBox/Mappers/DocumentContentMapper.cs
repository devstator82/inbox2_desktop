using System;
using System.IO;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.Text.iFilter;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework.VirtualMailBox.Mappers
{
	[Serializable]
	public class DocumentContentMapper : IContentMapper
	{
		private readonly Document document;

		public DocumentContentMapper(Document document)
		{
			this.document = document;
		}

		public string PropertyName
		{
			get { return "ContentStream"; }
		}

		public string GetContent()
		{
			try
			{
				var filter = FilterReader.GetFilter(
					ClientState.Current.Storage.ResolvePhysicalFilename(".", document.StreamName), 
					Path.GetExtension(document.Filename));

				// If filter is null that means we have no filter for given extension
				if (filter != null)
				{
					using (FilterReader reader = new FilterReader(filter))
						return reader.ReadToEnd();
				}
				else
				{
					Logger.Debug("Unable to find filter for file {0}", LogSource.Search, document.Filename);

					return String.Empty;
				}
			}
			catch (Exception ex)
			{
				Logger.Debug("An error occured while trying to find filter for file {0}. Exception = {1}", LogSource.Search, document.Filename, ex);

				return String.Empty;
			}		
		}
	}
}