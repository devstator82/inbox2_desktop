using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.InteropServices.ComTypes;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[ComImport, InterfaceType( ComInterfaceType.InterfaceIsIUnknown ), Guid( "0000010d-0000-0000-C000-000000000046" )]
	public interface IViewObject
	{
		[PreserveSig]
		int Draw( [In, MarshalAs( UnmanagedType.U4 )] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, [In] COMRECT lprcBounds, [In] COMRECT lprcWBounds, IntPtr pfnContinue, [In] int dwContinue );
		[PreserveSig]
		int GetColorSet( [In, MarshalAs( UnmanagedType.U4 )] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, [Out] tagLOGPALETTE ppColorSet );
		[PreserveSig]
		int Freeze( [In, MarshalAs( UnmanagedType.U4 )] int dwDrawAspect, int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze );
		[PreserveSig]
		int Unfreeze( [In, MarshalAs( UnmanagedType.U4 )] int dwFreeze );
		void SetAdvise( [In, MarshalAs( UnmanagedType.U4 )] int aspects, [In, MarshalAs( UnmanagedType.U4 )] int advf, [In, MarshalAs( UnmanagedType.Interface )] IAdviseSink pAdvSink );
		void GetAdvise( [In, Out, MarshalAs( UnmanagedType.LPArray )] int[] paspects, [In, Out, MarshalAs( UnmanagedType.LPArray )] int[] advf, [In, Out, MarshalAs( UnmanagedType.LPArray )] IAdviseSink[] pAdvSink );
	}
}