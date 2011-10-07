using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[ComImport, Guid( "00000114-0000-0000-C000-000000000046" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	public interface IOleWindow
	{
		[PreserveSig]
		int GetWindow( out IntPtr hwnd );
		void ContextSensitiveHelp( int fEnterMode );
	}
}