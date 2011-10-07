using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Core.Configuration;
using Inbox2.Framework.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Storage
{
	[Export(typeof(IFileStorage))]
	public class OpenFileStorage : IFileStorage
	{
		protected string DataDirectory = Path.Combine(DebugKeys.DefaultDataDirectory, "fs");

		public OpenFileStorage()
		{
			DirectoryInfo dir = new DirectoryInfo(DataDirectory);
			
			if (!dir.Exists)
				dir.Create();
		}

		public Stream Read(string ns, string filename)
		{
			#region Input validation

			if (String.IsNullOrEmpty(ns))
			{
				Logger.Error("Namespace parameter was null or empty.", LogSource.Storage);

				return null;
			}

			if (String.IsNullOrEmpty(filename))
			{
				Logger.Error("Filename parameter was null or empty.", LogSource.Storage);

				return null;
			}

			#endregion

			try
			{
				FileInfo file = new FileInfo(ResolvePhysicalFilename(ns, filename));

				if (file.Exists == false)
				{
					Logger.Error("Requested file was not found. Namespace = {0}, Filename = {1}", LogSource.Storage, ns, filename);

					return null;
				}

				return new FileStream(ResolvePhysicalFilename(ns, filename), FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch (Exception e)
			{
				Logger.Error("An error has occured during a read operation. Namespace = {0}, Filename = {1}, Exception = {2}", LogSource.Storage, ns, filename, e);

				return null;
			}
		}

		public bool Write(string ns, string filename, Stream dataStream)
		{
			#region Input validation

			if (String.IsNullOrEmpty(ns))
			{
				Logger.Error("Namespace parameter was null or empty.", LogSource.Storage);

				return false;
			}

			if (String.IsNullOrEmpty(filename))
			{
				Logger.Error("Filename parameter was null or empty.", LogSource.Storage);

				return false;
			}

			if (dataStream == null)
			{
				Logger.Error("DataStream parameter was null or empty.", LogSource.Storage);

				return false;
			}

			#endregion

			try
			{
				using (FileStream fs = new FileStream(ResolvePhysicalFilename(ns, filename), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					dataStream.CopyTo(fs);
				}
			}
			catch (Exception e)
			{
				Logger.Error("An error has occured during a write operation. Namespace = {0}, Filename = {1}, Exception = {2}", LogSource.Storage, ns, filename, e);

				return false;
			}

			return true;
		}

		public bool Delete(string ns, string filename)
		{
			FileInfo file = new FileInfo(ResolvePhysicalFilename(ns, filename));

			if (file.Exists)
			{
				file.Delete();

				return true;
			}

			return false;
		}

		public string ResolvePhysicalFilename(string ns, string filename)
		{
			var basePath = Path.Combine(DataDirectory, ns);

			if (!Directory.Exists(basePath))
				Directory.CreateDirectory(basePath);

			return Path.Combine(basePath, filename);
		}

		public void Dispose()
		{
		}
	}
}
