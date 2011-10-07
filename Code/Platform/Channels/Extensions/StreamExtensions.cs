using System;
using System.IO;
using System.Text;

namespace Inbox2.Platform.Channels.Extensions
{
	public static class StreamExtensions
	{
		public static MemoryStream ToStream(this StringBuilder s)
		{
			return s.ToString().ToStream();
		}

		public static MemoryStream ToStream(this string s)
		{
			if (String.IsNullOrEmpty(s))
				return new MemoryStream();

			byte[] bytes = Encoding.UTF8.GetBytes(s);

			MemoryStream ms = new MemoryStream();
			ms.Write(bytes, 0, bytes.Length);
			ms.Seek(0, SeekOrigin.Begin);

			return ms;
		}

		/// <summary>
		/// Copies this stream to the output stream.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="output">The output.</param>
		public static void CopyTo(this Stream input, Stream output)
		{
			if (input == null)
				throw new ArgumentNullException("input");

			if (output == null)
				throw new ArgumentNullException("output");

			if (input.CanSeek)
				input.Seek(0, SeekOrigin.Begin);

			const int size = 4096;
			byte[] bytes = new byte[4096];
			int numBytes;

			while ((numBytes = input.Read(bytes, 0, size)) > 0)
			{
				output.Write(bytes, 0, numBytes);
			}
		}

		public static string ReadString(this Stream s)
		{
			if (s == null)
				return String.Empty;

			if (s.CanSeek)
				s.Seek(0, SeekOrigin.Begin);

			StreamReader sr = new StreamReader(s, Encoding.UTF8);

			return sr.ReadToEnd();
		}
	}
}