using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[ComImport, InterfaceType( ComInterfaceType.InterfaceIsIUnknown ), Guid( "00000100-0000-0000-C000-000000000046" )]
	public interface IEnumUnknown
	{
		[PreserveSig]
		int Next( [In, MarshalAs( UnmanagedType.U4 )] int celt, [Out] IntPtr rgelt, IntPtr pceltFetched );
		[PreserveSig]
		int Skip( [In, MarshalAs( UnmanagedType.U4 )] int celt );
		void Reset();
		void Clone( out IEnumUnknown ppenum );
	}
}