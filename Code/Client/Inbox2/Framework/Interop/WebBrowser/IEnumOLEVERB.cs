using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[ComImport, Guid( "00000104-0000-0000-C000-000000000046" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	public interface IEnumOLEVERB
	{
		[PreserveSig]
		int Next( [MarshalAs( UnmanagedType.U4 )] int celt, [Out] tagOLEVERB rgelt, [Out, MarshalAs( UnmanagedType.LPArray )] int[] pceltFetched );
		[PreserveSig]
		int Skip( [In, MarshalAs( UnmanagedType.U4 )] int celt );
		void Reset();
		void Clone( out IEnumOLEVERB ppenum );
	}
}