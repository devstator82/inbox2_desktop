using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Network.Utils
{
	public static class BufferUtils
	{
		public static void CreateSegments(byte[] data, List<ArraySegment<byte>> segments)
		{
			for (int i = 0; i < Math.Ceiling((double)data.Length / BufferManager.ChunkSize); i++)
			{
				var segment = BufferManager.Current.CheckOut();

				Buffer.BlockCopy(data, i * BufferManager.ChunkSize, segment.Array, 0,
					data.Length >= ((i + 1) * BufferManager.ChunkSize) ?
						BufferManager.ChunkSize :
						data.Length - (i * BufferManager.ChunkSize));

				segments.Add(segment);
			}
		}

		public static byte[] CreateBytes(List<ArraySegment<byte>> segments, int numberOfBytes)
		{
			var buffer = new byte[numberOfBytes];

			for (int i = 0; i < Math.Ceiling((double)numberOfBytes / BufferManager.ChunkSize); i++)
			{
				int bytesToRead;

				if (numberOfBytes <= BufferManager.ChunkSize)
					bytesToRead = numberOfBytes;
				else
					bytesToRead = (i * BufferManager.ChunkSize > numberOfBytes) ?
						numberOfBytes - (i * BufferManager.ChunkSize) :
						BufferManager.ChunkSize;

				Buffer.BlockCopy(segments[i].Array, 0, buffer, i * BufferManager.ChunkSize, bytesToRead);
			}

			return buffer;
		}

		/// <summary>
		/// Find the occurance of needle inside the haystack byte[] array. 
		/// Starts reading backwards from the maxBytes index.
		/// </summary>
		/// <param name="haystack"></param>
		/// <param name="needle"></param>
		/// <param name="maxBytes"></param>
		/// <returns></returns>
		public static int IndexOf(byte[] haystack, byte[] needle, int maxBytes)
		{
			if (haystack.Length < needle.Length)
				return -1;

			for (int i = (maxBytes - needle.Length); i >= 0; i--)
			{
				if (IsIndexOfMatch(i, haystack, needle))
					return i;
			}

			return -1;
		}

		static bool IsIndexOfMatch(int i, byte[] haystack, byte[] needle)
		{
			int match = 0;

			for (int j = 0; j < needle.Length; j++)
				if (haystack[i + j] == needle[j]) match++;

			return match == needle.Length;
		}
	}
}
