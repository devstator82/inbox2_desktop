using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[StructLayout( LayoutKind.Sequential )]
	public sealed class tagOLEVERB
	{
		public int lVerb;
		[MarshalAs( UnmanagedType.LPWStr )]
		public string lpszVerbName;
		[MarshalAs( UnmanagedType.U4 )]
		public int fuFlags;
		[MarshalAs( UnmanagedType.U4 )]
		public int grfAttribs;
		public tagOLEVERB()
		{
		}
	}
}