using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[ComImport, Guid( "00000113-0000-0000-C000-000000000046" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown ), SuppressUnmanagedCodeSecurity]
	public interface IOleInPlaceObject
	{
		[PreserveSig]
		int GetWindow( out IntPtr hwnd );
		void ContextSensitiveHelp( int fEnterMode );
		void InPlaceDeactivate();
		[PreserveSig]
		int UIDeactivate();
		void SetObjectRects( [In] COMRECT lprcPosRect, [In] COMRECT lprcClipRect );
		void ReactivateAndUndo();
	}
}