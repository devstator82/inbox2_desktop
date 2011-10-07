using System;
using System.IO;
using System.Text;
using Inbox2.Platform.Channels.Text;

namespace Inbox2.Platform.Framework.Extensions
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

		public static string ReadString(this Stream s)
		{
			if (s == null)
				return String.Empty;

			if (s.CanSeek)
				s.Seek(0, SeekOrigin.Begin);

			StreamReader sr = new StreamReader(s, Encoding.UTF8);

			return sr.ReadToEnd();
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

		/// <summary>
		/// Gets the bytes from the given source stream.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static byte[] GetBytes(this Stream source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (source is MemoryStream)
				return (source as MemoryStream).ToArray();

			using (MemoryStream ms = new MemoryStream())
			{
				source.CopyTo(ms);

				return ms.ToArray();
			}
		}

		public static string CalculateCrc32(this Stream s)
		{
			StringBuilder sb = new StringBuilder();

			Crc32 crc32 = new Crc32();
			foreach (byte b in crc32.ComputeHash(s))
				sb.Append(b.ToString("x2").ToLower());

			return sb.ToString();
		}
	}
}