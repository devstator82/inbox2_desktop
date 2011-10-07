using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.Runtime.InteropServices.ComTypes;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[ComImport, Guid( "00000112-0000-0000-C000-000000000046" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown ), SuppressUnmanagedCodeSecurity]
	public interface IOleObject
	{
		[PreserveSig]
		int SetClientSite( [In, MarshalAs( UnmanagedType.Interface )] IOleClientSite pClientSite );
		IOleClientSite GetClientSite();
		[PreserveSig]
		int SetHostNames( [In, MarshalAs( UnmanagedType.LPWStr )] string szContainerApp, [In, MarshalAs( UnmanagedType.LPWStr )] string szContainerObj );
		[PreserveSig]
		int Close( int dwSaveOption );
		[PreserveSig]
		int SetMoniker( [In, MarshalAs( UnmanagedType.U4 )] int dwWhichMoniker, [In, MarshalAs( UnmanagedType.Interface )] object pmk );
		[PreserveSig]
		int GetMoniker( [In, MarshalAs( UnmanagedType.U4 )] int dwAssign, [In, MarshalAs( UnmanagedType.U4 )] int dwWhichMoniker, [MarshalAs( UnmanagedType.Interface )] out object moniker );
		[PreserveSig]
		int InitFromData( [In, MarshalAs( UnmanagedType.Interface )] IDataObject pDataObject, int fCreation, [In, MarshalAs( UnmanagedType.U4 )] int dwReserved );
		[PreserveSig]
		int GetClipboardData( [In, MarshalAs( UnmanagedType.U4 )] int dwReserved, out IDataObject data );
		[PreserveSig]
		int DoVerb( int iVerb, [In] IntPtr lpmsg, [In, MarshalAs( UnmanagedType.Interface )] IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, [In] COMRECT lprcPosRect );
		[PreserveSig]
		int EnumVerbs( out IEnumOLEVERB e );
		[PreserveSig]
		int OleUpdate();
		[PreserveSig]
		int IsUpToDate();
		[PreserveSig]
		int GetUserClassID( [In, Out] ref Guid pClsid );
		[PreserveSig]
		int GetUserType( [In, MarshalAs( UnmanagedType.U4 )] int dwFormOfType, [MarshalAs( UnmanagedType.LPWStr )] out string userType );
		[PreserveSig]
		int SetExtent( [In, MarshalAs( UnmanagedType.U4 )] int dwDrawAspect, [In] tagSIZEL pSizel );
		[PreserveSig]
		int GetExtent( [In, MarshalAs( UnmanagedType.U4 )] int dwDrawAspect, [Out] tagSIZEL pSizel );
		[PreserveSig]
		int Advise( IAdviseSink pAdvSink, out int cookie );
		[PreserveSig]
		int Unadvise( [In, MarshalAs( UnmanagedType.U4 )] int dwConnection );
		[PreserveSig]
		int EnumAdvise( out IEnumSTATDATA e );
		[PreserveSig]
		int GetMiscStatus( [In, MarshalAs( UnmanagedType.U4 )] int dwAspect, out int misc );
		[PreserveSig]
		int SetColorScheme( [In] tagLOGPALETTE pLogpal );
	}
}