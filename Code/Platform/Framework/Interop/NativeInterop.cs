using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Runtime.InteropServices;

namespace Inbox2.Platform.Framework.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SIZE
	{
		public int cx;
		public int cy;
		public SIZE(int cx, int cy)
		{
			this.cx = cx;
			this.cy = cy;
		}
	}

	public enum SIGDN : uint
	{
		NORMALDISPLAY = 0,
		PARENTRELATIVEPARSING = 0x80018001,
		PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
		DESKTOPABSOLUTEPARSING = 0x80028000,
		PARENTRELATIVEEDITING = 0x80031001,
		DESKTOPABSOLUTEEDITING = 0x8004c000,
		FILESYSPATH = 0x80058000,
		URL = 0x80068000
	}

	[Flags]
	public enum SIIGBF
	{
		SIIGBF_RESIZETOFIT = 0x00,
		SIIGBF_BIGGERSIZEOK = 0x01,
		SIIGBF_MEMORYONLY = 0x02,
		SIIGBF_ICONONLY = 0x04,
		SIIGBF_THUMBNAILONLY = 0x08,
		SIIGBF_INCACHEONLY = 0x10,
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
	public interface IShellItem
	{
		void BindToHandler(IntPtr pbc,
		[MarshalAs(UnmanagedType.LPStruct)]Guid bhid,
		[MarshalAs(UnmanagedType.LPStruct)]Guid riid,
		out IntPtr ppv);
		void GetParent(out IShellItem ppsi);
		void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
		void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
		void Compare(IShellItem psi, uint hint, out int piOrder);
	};

	[ComImportAttribute()]
	[GuidAttribute("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
	[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IShellItemImageFactory
	{
		void GetImage(
			[In, MarshalAs(UnmanagedType.Struct)] SIZE size,
			[In] SIIGBF flags,
			[Out] out IntPtr phbm);
	}

	[SuppressUnmanagedCodeSecurity]
	public static class UnsafeNativeMethods
	{
		[DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
		public static extern void SHCreateItemFromParsingName(
		[In][MarshalAs(UnmanagedType.LPWStr)] string pszPath,
		[In] IntPtr pbc,
		[In][MarshalAs(UnmanagedType.LPStruct)] Guid riid,
		[Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellItem ppv);
	}
}
