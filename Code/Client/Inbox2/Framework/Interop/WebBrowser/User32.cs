/*
 * This sample is released as public domain.  It is distributed in the hope that 
 * it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 * 
 * 2007-08-01:
 * Initial release.
 * 
 */


using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Inbox2.Framework.Interop.WebBrowser
{
	public enum GW : uint
	{
		HWNDFIRST        = 0,
		HWNDLAST         = 1,
		HWNDNEXT         = 2,
		HWNDPREV         = 3,
		OWNER            = 4,
		CHILD            = 5,
		ENABLEDPOPUP     = 6,
	}

	public class ICON
	{
		public const UInt32 SMALL          = 0;
		public const UInt32 BIG            = 1;
		public const UInt32 SMALL2         = 2; // XP+
	}

	public enum MB : uint
	{
		SimpleBeep      = 0xFFFFFFFF,
		IconAsterisk    = 0x00000040,
		IconWarning     = 0x00000030,
		IconError       = 0x00000010,
		IconQuestion    = 0x00000020,
		OK              = 0x00000000
	}

	[ Flags ]
	public enum RDW : uint
	{
		INVALIDATE      = 0x0001,
		INTERNALPAINT   = 0x0002,
		ERASE           = 0x0004,
		VALIDATE        = 0x0008,
		NOINTERNALPAINT = 0x0010,
		NOERASE         = 0x0020,
		NOCHILDREN      = 0x0040,
		ALLCHILDREN     = 0x0080,
		UPDATENOW       = 0x0100,
		ERASENOW        = 0x0200,
		FRAME           = 0x0400,
		NOFRAME         = 0x0800,
	}

	public class SW
	{
		public const int HIDE               = 0;
		public const int SHOWNORMAL         = 1;
		public const int NORMAL             = 1;
		public const int SHOWMINIMIZED      = 2;
		public const int SHOWMAXIMIZED      = 3;
		public const int MAXIMIZE           = 3;
		public const int SHOWNOACTIVATE     = 4;
		public const int SHOW               = 5;
		public const int MINIMIZE           = 6;
		public const int SHOWMINNOACTIVE    = 7;
		public const int SHOWNA             = 8;
		public const int RESTORE            = 9;
		public const int SHOWDEFAULT        = 10;
		public const int FORCEMINIMIZE      = 11;
		public const int MAX                = 11;
	}

	public class TB
	{
		public const uint GETBUTTON       = WM.USER + 23 ;
		public const uint BUTTONCOUNT     = WM.USER + 24 ;
		public const uint CUSTOMIZE       = WM.USER + 27 ;
		public const uint GETBUTTONTEXTA  = WM.USER + 45 ;
		public const uint GETBUTTONTEXTW  = WM.USER + 75 ;
	}

	public class TBSTATE
	{
		public const uint CHECKED        =  0x01 ;
		public const uint PRESSED        =  0x02 ;
		public const uint ENABLED        =  0x04 ;
		public const uint HIDDEN         =  0x08 ;
		public const uint INDETERMINATE  =  0x10 ;
		public const uint WRAP           =  0x20 ;
		public const uint ELLIPSES       =  0x40 ;
		public const uint MARKED         =  0x80 ;
	}

	public class WM
	{
		public const uint CLOSE   = 0x0010;
		public const uint GETICON = 0x007F;
		public const uint KEYDOWN = 0x0100;
		public const uint COMMAND = 0x0111;
		public const uint USER    = 0x0400; // 0x0400 - 0x7FFF
		public const uint APP     = 0x8000; // 0x8000 - 0xBFFF
	}

	public class GCL
	{
		public const int MENUNAME       = - 8;
		public const int HBRBACKGROUND  = -10;
		public const int HCURSOR        = -12;
		public const int HICON          = -14;
		public const int HMODULE        = -16;
		public const int CBWNDEXTRA     = -18;
		public const int CBCLSEXTRA     = -20;
		public const int WNDPROC        = -24;
		public const int STYLE          = -26;
		public const int ATOM           = -32;
		public const int HICONSM        = -34;

		// GetClassLongPtr ( 64-bit )
		private const int GCW_ATOM           = -32;
		private const int GCL_CBCLSEXTRA     = -20;
		private const int GCL_CBWNDEXTRA     = -18;
		private const int GCLP_MENUNAME      = - 8;
		private const int GCLP_HBRBACKGROUND = -10;
		private const int GCLP_HCURSOR       = -12;
		private const int GCLP_HICON         = -14;
		private const int GCLP_HMODULE       = -16;
		private const int GCLP_WNDPROC       = -24;
		private const int GCLP_HICONSM       = -34;
		private const int GCL_STYLE          = -26;

	}

	public class INPUT
	{
		public const int MOUSE     = 0;
		public const int KEYBOARD  = 1;
		public const int HARDWARE  = 2;
	}

	public class MOUSEEVENTF
	{
		public const int MOVE        = 0x0001 ; /* mouse move */
		public const int LEFTDOWN    = 0x0002 ; /* left button down */
		public const int LEFTUP      = 0x0004 ; /* left button up */
		public const int RIGHTDOWN   = 0x0008 ; /* right button down */
		public const int RIGHTUP     = 0x0010 ; /* right button up */
		public const int MIDDLEDOWN  = 0x0020 ; /* middle button down */
		public const int MIDDLEUP    = 0x0040 ; /* middle button up */
		public const int XDOWN       = 0x0080 ; /* x button down */
		public const int XUP         = 0x0100 ; /* x button down */
		public const int WHEEL       = 0x0800 ; /* wheel button rolled */
		public const int VIRTUALDESK = 0x4000 ; /* map to entire virtual desktop */
		public const int ABSOLUTE    = 0x8000 ; /* absolute move */
	}

	[ StructLayout( LayoutKind.Sequential ) ]
	public struct POINT 
	{
		public Int32 X;
		public Int32 Y;
	}

	[ StructLayout( LayoutKind.Sequential ) ]
	public struct RECT 
	{
		public Int32 Left;
		public Int32 Top;
		public Int32 Right;
		public Int32 Bottom;
	}

	[ StructLayout( LayoutKind.Sequential ) ]
	public struct TBBUTTON 
	{
		public Int32 iBitmap;
		public Int32 idCommand;
		public byte fsState;
		public byte fsStyle;
//		[ MarshalAs( UnmanagedType.ByValArray, SizeConst=2 ) ]
//		public byte[] bReserved;
		public byte bReserved1;
		public byte bReserved2;
		public UInt32 dwData;
		public IntPtr iString;
	};

	[ StructLayout( LayoutKind.Sequential ) ]
	public struct WINDOWPLACEMENT
	{
		public int Length;
		public int Flags;
		public int ShowCmd;
		public POINT MinPosition;
		public POINT MaxPosition;
		public RECT NormalPosition;
	}

	[ StructLayout( LayoutKind.Sequential ) ]
	public struct MOUSEINPUT 
	{
		public int dx;
		public int dy;
		public int mouseData;
		public int dwFlags;
		public int time;
		public IntPtr dwExtraInfo;
	}

	[ StructLayout( LayoutKind.Sequential ) ]
	public struct KEYBDINPUT 
	{
		public short wVk;
		public short wScan;
		public int dwFlags;
		public int time;
		public IntPtr dwExtraInfo;
	}

	[ StructLayout( LayoutKind.Sequential ) ]
	public struct HARDWAREINPUT
	{
		public int uMsg;
		public short wParamL;
		public short wParamH;
	}


	[ StructLayout( LayoutKind.Explicit ) ]
	public struct Input 
	{
		[ FieldOffset( 0 ) ] public int type;
		[ FieldOffset( 4 ) ] public MOUSEINPUT mi;
		[ FieldOffset( 4 ) ] public KEYBDINPUT ki;
		[ FieldOffset( 4 ) ] public HARDWAREINPUT hi;
	}

	public class User32
	{
		private User32() {}

//		public const UInt32 WM_USER = 0x0400;

//		public const UInt32 WM_KEYDOWN = 0x0100;
		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(
			IntPtr hWnd,
			UInt32 msg,
			IntPtr wParam,
			IntPtr lParam );

		[DllImport("user32.dll")]
		public static extern UInt32 SendMessage(
			IntPtr hWnd,
			UInt32 msg,
			UInt32 wParam,
			UInt32 lParam );

		[ DllImport( "User32.dll" ) ]
		public static extern bool PostMessage
			(
			IntPtr hWnd,
			UInt32 Msg,
			IntPtr wParam,
			IntPtr lParam
			);

		[ DllImport( "User32.dll" ) ]
		public static extern bool PostMessage
			(
			IntPtr hWnd,
			UInt32 Msg,
			UInt32 wParam,
			UInt32 lParam
			);

		[ DllImport( "User32.dll" ) ]
		public static extern bool MessageBeep
			(
			MB BeepType
			);

		[DllImport( "user32.dll" )]
		public static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X,
		                                        int Y, int cx, int cy, uint uFlags );

		[DllImport( "user32.dll" )]
		public static extern IntPtr SetActiveWindow( IntPtr hWnd );

		[DllImport("user32.dll")]
		public static extern bool ShowWindow
			(
			IntPtr hWnd,
			int nCmdShow
			);

		[ DllImport( "User32.dll" ) ]
		public static extern bool SetForegroundWindow
			(
			IntPtr hWnd
			);


		[ DllImport( "User32.dll" ) ]
		public static extern IntPtr GetDesktopWindow
			(
			);

		[ DllImport( "User32.dll" ) ]
		public static extern IntPtr GetTaskmanWindow
			(
			);

		[ DllImport( "user32.dll", CharSet = CharSet.Unicode ) ]
		public static extern IntPtr FindWindowEx(
			IntPtr hwndParent,
			IntPtr hwndChildAfter,
			string lpszClass,
			string lpszWindow);
		
		[ DllImport( "User32.dll" ) ]
		public static extern IntPtr GetWindow
			(
			HandleRef hWnd,
			GW     uCmd
			);

		[ DllImport( "User32.dll" ) ]
		public static extern Int32 GetWindowTextLength
			(
			HandleRef hWnd
			);

		[ DllImport( "User32.dll", SetLastError = true, CharSet = CharSet.Auto ) ]
		public static extern Int32 GetWindowText
			(
			HandleRef hWnd,
			StringBuilder lpString,
			Int32 nMaxCount
			);

		[DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetCapture();

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern Int32 GetClassName
			(
			HandleRef hWnd,
			out StringBuilder lpClassName,
			Int32 nMaxCount
			);

//		[ DllImport( "user32.dll", EntryPoint = "GetClassLongPtrW" ) ]
		[ DllImport( "user32.dll" ) ]
		public static extern UInt32 GetClassLong
			(
			HandleRef hWnd,
			int nIndex
			);

		[DllImport("user32.dll")]
		public static extern uint SetClassLong
			(
			HandleRef hWnd,
			int nIndex,
			uint dwNewLong
			);
		
		[ DllImport( "User32.dll", CharSet=CharSet.Auto ) ]
		public static extern UInt32 GetWindowThreadProcessId
			(
			IntPtr hWnd,
//			[ MarshalAs( UnmanagedType.
			out UInt32 lpdwProcessId
			);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool RedrawWindow
			(
			HandleRef hWnd,
//			[In] ref RECT lprcUpdate,
			IntPtr lprcUpdate,
			IntPtr hrgnUpdate,
			uint flags
			);

		public static bool RedrawWindow(HandleRef hWnd)
		{
			return RedrawWindow( hWnd, IntPtr.Zero, IntPtr.Zero,
			                     ( uint ) ( RDW.ERASE | RDW.INVALIDATE | RDW.UPDATENOW ) );
		}

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool PrintWindow
			(
			IntPtr hwnd,
			IntPtr hdcBlt,
			uint nFlags
			);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern UInt32 SendInput
			(
			UInt32 nInputs,
			Input[] pInputs,
			Int32 cbSize
			);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool GetWindowPlacement
			(
			IntPtr hWnd,
			ref WINDOWPLACEMENT lpwndpl
			);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern IntPtr WindowFromPoint(
			POINT point
			);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern IntPtr ChildWindowFromPoint(
			POINT point
			);

		[DllImport("User32.dll", SetLastError=true)]
		public static extern IntPtr GetParent
			(
			HandleRef hWnd
			);

		[DllImport("User32.dll", SetLastError = true)]
		private static extern IntPtr GetParent
			(
			IntPtr hWnd
			);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool ScreenToClient
			(
			HandleRef hWnd,
			ref POINT lpPoint
			);

		[DllImport("User32", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int ClientToScreen(HandleRef hWnd, [In, Out] POINT pt);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool GetClientRect
			(
			HandleRef hWnd,
			out RECT lpRect
			);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool PtInRect
			(
			ref RECT lprc,
			POINT pt
			);

		[DllImport("User32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);
		public static POINT GetCursorPos()
		{
			POINT p;
			GetCursorPos(out p);
			return p;
		}

		[DllImport( "user32.dll" )]
		static extern bool GetUpdateRect( HandleRef hWnd, out RECT rect, bool bErase );
		public static RECT GetUpdateRect( HandleRef hWnd, bool bErase )
		{
			RECT rect;
			GetUpdateRect( hWnd, out rect, bErase );
			return rect;
		}

		[DllImport( "user32.dll" )]
		public static extern bool ValidateRect( IntPtr hWnd, ref RECT lpRect );

		public static uint MAKEDWORD(ushort loWord, ushort hiWord)
		{
			return (uint)(loWord + (hiWord << 16));
		}

		public static uint MAKEDWORD(short loWord, short hiWord)
		{
			return (uint)(loWord + (hiWord << 16));
		}

		public static Point DWordToPoint(UInt32 dWord)
		{
			return new Point(
				(int)(dWord & 0xFFFF),
				(int)(dWord >> 16)
				);
		}

		[DllImport("user32.dll")]
		public static extern void mouse_event
			(
			UInt32 dwFlags, // motion and click options
			UInt32 dx, // horizontal position or change
			UInt32 dy, // vertical position or change
			UInt32 dwData, // wheel movement
			IntPtr dwExtraInfo // application-defined information
			);

		public static bool IsParentWindow(IntPtr hWnd, HandleRef parent)
		{
			while (hWnd != IntPtr.Zero)
			{
				if (hWnd == parent.Handle)
					return true;

				hWnd = GetParent(hWnd);
			}

			return false;
		}

		public const int WS_EX_TRANSPARENT = 0x00000020;
		public const int GWL_EXSTYLE = ( -20 );

		[DllImport( "user32.dll" )]
		public static extern int GetWindowLong( IntPtr hwnd, int index );

		[DllImport( "user32.dll" )]
		public static extern int SetWindowLong( IntPtr hwnd, int index, int newStyle );

		public const int WM_SETCURSOR = 0x20;
		public const int WM_TIMER = 0x113;

		public const int WM_MOUSEMOVE = 0x200;
		public const int WM_MBUTTONDOWN = 0x0207;
		public const int WM_MBUTTONUP = 0x0208;
		public const int WM_MBUTTONDBLCLK = 0x0209;

		public const int WM_LBUTTONDOWN = 0x0201;
		public const int WM_LBUTTONUP = 0x0202;
		public const int WM_LBUTTONDBLCLK = 0x0203;

		public const int WM_RBUTTONDOWN = 0x0204;
		public const int WM_RBUTTONUP = 0x0205;
		public const int WM_RBUTTONDBLCLK = 0x0206;

		public const int WM_ACTIVATE = 0x006;
		public const int WM_ACTIVATEAPP = 0x01C;
		public const int WM_NCACTIVATE = 0x086;
		public const int WM_CLOSE = 0x010;

		public const int WM_PAINT = 0x000F;
		public const int WM_ERASEBKGND = 0x0014;
	}
}


