using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[ComImport, InterfaceType( ComInterfaceType.InterfaceIsIUnknown ), Guid( "0000011B-0000-0000-C000-000000000046" )]
	public interface IOleContainer
	{
		[PreserveSig]
		int ParseDisplayName( [In, MarshalAs( UnmanagedType.Interface )] object pbc, [In, MarshalAs( UnmanagedType.BStr )] string pszDisplayName, [Out, MarshalAs( UnmanagedType.LPArray )] int[] pchEaten, [Out, MarshalAs( UnmanagedType.LPArray )] object[] ppmkOut );
		[PreserveSig]
		int EnumObjects( [In, MarshalAs( UnmanagedType.U4 )] int grfFlags, out IEnumUnknown ppenum );
		[PreserveSig]
		int LockContainer( bool fLock );
	}
}