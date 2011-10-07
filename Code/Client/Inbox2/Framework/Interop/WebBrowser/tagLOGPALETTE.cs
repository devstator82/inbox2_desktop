using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[StructLayout( LayoutKind.Sequential )]
	public sealed class tagLOGPALETTE
	{
		[MarshalAs( UnmanagedType.U2 )]
		public short palVersion;
		[MarshalAs( UnmanagedType.U2 )]
		public short palNumEntries;
		public tagLOGPALETTE()
		{
		}
	}
}