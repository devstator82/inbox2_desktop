using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Network
{
	public static class WireFormat
	{
		public const byte SuccessControlChar = 0x000;
		public const byte ErrorControlChar = 0x0ff;
		public static byte[] Eof = Encoding.UTF8.GetBytes("<EOF>");

		public static byte[] GetBytes(byte method, MemoryStream ms)
		{
			var buffer = GetBytesInternal(ms.GetBuffer(), method);

			return buffer;
		}

		public static byte[] GetBytes(MemoryStream ms)
		{
			return GetBytesInternal(ms.GetBuffer(), SuccessControlChar);
		}

		public static byte[] GetBytes(Exception ex)
		{
			return GetBytesInternal(Encoding.UTF8.GetBytes(ex.ToString()), ErrorControlChar);
		}

		public static byte[] GetBytes(string error)
		{
			return GetBytesInternal(Encoding.UTF8.GetBytes(error), ErrorControlChar);
		}

		static byte[] GetBytesInternal(byte[] payload, byte controlByte)
		{
			var responseData = new byte[payload.Length + 1 + Eof.Length];

			// Copy controlByte
			responseData[0] = controlByte;

			Buffer.BlockCopy(payload, 0, responseData, 1, payload.Length);
			Buffer.BlockCopy(Eof, 0, responseData, payload.Length + 1, Eof.Length);

			return responseData;
		}
	}
}
