using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces
{
	public interface IFileStorage : IDisposable
	{
		/// <summary>
		/// Reads the specified file from the given namespace and returns a stream. The caller is responsible for disposing of the stream.
		/// </summary>
		/// <param name="ns">The ns.</param>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		Stream Read(string ns, string filename);

		/// <summary>
		/// Writes the specified dataStream in the given filename and namespace.
		/// </summary>
		/// <param name="ns">The ns.</param>
		/// <param name="filename">The filename.</param>
		/// <param name="dataStream">The data stream.</param>
		/// <returns></returns>
		bool Write(string ns, string filename, Stream dataStream);

		/// <summary>
		/// Deletes the given file from the given namespace.
		/// </summary>
		/// <param name="ns">The ns.</param>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		bool Delete(string ns, string filename);

		/// <summary>
		/// Resolves the physical filename.
		/// </summary>
		/// <param name="ns">The ns.</param>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		string ResolvePhysicalFilename(string ns, string filename);		
	}
}
