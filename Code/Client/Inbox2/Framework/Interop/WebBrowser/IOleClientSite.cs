using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[ComImport, InterfaceType( ComInterfaceType.InterfaceIsIUnknown ), Guid( "00000118-0000-0000-C000-000000000046" )]
	public interface IOleClientSite
	{
		[PreserveSig]
		int SaveObject();
		[PreserveSig]
		int GetMoniker( [In, MarshalAs( UnmanagedType.U4 )] int dwAssign, [In, MarshalAs( UnmanagedType.U4 )] int dwWhichMoniker, [MarshalAs( UnmanagedType.Interface )] out object moniker );
		[PreserveSig]
		int GetContainer( out IOleContainer container );
		[PreserveSig]
		int ShowObject();
		[PreserveSig]
		int OnShowWindow( int fShow );
		[PreserveSig]
		int RequestNewObjectLayout();
	}
}