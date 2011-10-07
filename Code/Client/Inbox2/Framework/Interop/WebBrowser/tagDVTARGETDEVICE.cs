using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[StructLayout( LayoutKind.Sequential )]
	public sealed class tagDVTARGETDEVICE
	{
		[MarshalAs( UnmanagedType.U4 )]
		public int tdSize;
		[MarshalAs( UnmanagedType.U2 )]
		public short tdDriverNameOffset;
		[MarshalAs( UnmanagedType.U2 )]
		public short tdDeviceNameOffset;
		[MarshalAs( UnmanagedType.U2 )]
		public short tdPortNameOffset;
		[MarshalAs( UnmanagedType.U2 )]
		public short tdExtDevmodeOffset;
		public tagDVTARGETDEVICE()
		{
		}
	}
}