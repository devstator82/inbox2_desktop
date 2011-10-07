using System;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;

namespace Inbox2.Framework.Interop
{
	public static class WinApi
	{
		[Flags]
		public enum WindowStyles : uint
		{
			WS_OVERLAPPED = 0x00000000,
			WS_POPUP = 0x80000000,
			WS_CHILD = 0x40000000,
			WS_MINIMIZE = 0x20000000,
			WS_VISIBLE = 0x10000000,
			WS_DISABLED = 0x08000000,
			WS_CLIPSIBLINGS = 0x04000000,
			WS_CLIPCHILDREN = 0x02000000,
			WS_MAXIMIZE = 0x01000000,
			WS_BORDER = 0x00800000,
			WS_DLGFRAME = 0x00400000,
			WS_VSCROLL = 0x00200000,
			WS_HSCROLL = 0x00100000,
			WS_SYSMENU = 0x00080000,
			WS_THICKFRAME = 0x00040000,
			WS_GROUP = 0x00020000,
			WS_TABSTOP = 0x00010000,

			WS_MINIMIZEBOX = 0x00020000,
			WS_MAXIMIZEBOX = 0x00010000,

			WS_CAPTION = WS_BORDER | WS_DLGFRAME,
			WS_TILED = WS_OVERLAPPED,
			WS_ICONIC = WS_MINIMIZE,
			WS_SIZEBOX = WS_THICKFRAME,
			WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

			WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
			WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
			WS_CHILDWINDOW = WS_CHILD,
		}

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        } 

		public const uint LVM_FIRST = 0x1000;
		public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
		public const uint LVM_GETITEMW = LVM_FIRST + 75;
		public const uint LVM_GETITEMPOSITION = LVM_FIRST + 16;
		public const uint LVM_GETNEXTITEM = (LVM_FIRST + 12);
		public const uint PROCESS_VM_OPERATION = 0x0008;
		public const uint PROCESS_VM_READ = 0x0010;
		public const uint PROCESS_VM_WRITE = 0x0020;
		public const uint MEM_COMMIT = 0x1000;
		public const uint MEM_RELEASE = 0x8000;
		public const uint MEM_RESERVE = 0x2000;
		public const uint PAGE_READWRITE = 4;

		public const int LVIF_TEXT = 0x0001;
		public const int LVIS_SELECTED = 0x0002;

		public const int GWL_ID = -12;
		public const int GWL_STYLE = -16;
		public const int GWL_EXSTYLE = -20;
		
		public const int WS_EX_LAYERED = 0x00080000;
		public const int WS_EX_TRANSPARENT = 0x00000020;

		public const int HWND_TOPMOST = -1;
		public const int HWND_NOTOPMOST = -2;
		public const int HWND_TOP = 0;
		public const int HWND_BOTTOM = 1;

		public const int SWP_NOSIZE = 0x0001;
		public const int SWP_NOMOVE = 0x0002;
		public const int SWP_NOZORDER = 0x0004;
		public const int SWP_NOREDRAW = 0x0008;
		public const int SWP_NOACTIVATE = 0x0010;
		public const int SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
		public const int SWP_SHOWWINDOW = 0x0040;
		public const int SWP_HIDEWINDOW = 0x0080;
		public const int SWP_NOCOPYBITS = 0x0100;
		public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
		public const int SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

		public const int TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
		public const int TOPMOST_FLAGS_NO_FOCUS = SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE;

		#region WM Commands

		public const uint WM_ACTIVATE = 0x0006;
		public const uint WM_ACTIVATEAPP = 0x001C;
		public const uint WM_AFXFIRST = 0x0360;
		public const uint WM_AFXLAST = 0x037F;
		public const uint WM_APP = 0x8000;
		public const uint WM_ASKCBFORMATNAME = 0x030C;
		public const uint WM_CANCELJOURNAL = 0x004B;
		public const uint WM_CANCELMODE = 0x001F;
		public const uint WM_CAPTURECHANGED = 0x0215;
		public const uint WM_CHANGECBCHAIN = 0x030D;
		public const uint WM_CHANGEUISTATE = 0x0127;
		public const uint WM_CHAR = 0x0102;
		public const uint WM_CHARTOITEM = 0x002F;
		public const uint WM_CHILDACTIVATE = 0x0022;
		public const uint WM_CLEAR = 0x0303;
		public const uint WM_CLOSE = 0x0010;
		public const uint WM_COMMAND = 0x0111;
		public const uint WM_COMPACTING = 0x0041;
		public const uint WM_COMPAREITEM = 0x0039;
		public const uint WM_CONTEXTMENU = 0x007B;
		public const uint WM_COPY = 0x0301;
		public const uint WM_COPYDATA = 0x004A;
		public const uint WM_CREATE = 0x0001;
		public const uint WM_CTLCOLORBTN = 0x0135;
		public const uint WM_CTLCOLORDLG = 0x0136;
		public const uint WM_CTLCOLOREDIT = 0x0133;
		public const uint WM_CTLCOLORLISTBOX = 0x0134;
		public const uint WM_CTLCOLORMSGBOX = 0x0132;
		public const uint WM_CTLCOLORSCROLLBAR = 0x0137;
		public const uint WM_CTLCOLORSTATIC = 0x0138;
		public const uint WM_CUT = 0x0300;
		public const uint WM_DEADCHAR = 0x0103;
		public const uint WM_DELETEITEM = 0x002D;
		public const uint WM_DESTROY = 0x0002;
		public const uint WM_DESTROYCLIPBOARD = 0x0307;
		public const uint WM_DEVICECHANGE = 0x0219;
		public const uint WM_DEVMODECHANGE = 0x001B;
		public const uint WM_DISPLAYCHANGE = 0x007E;
		public const uint WM_DRAWCLIPBOARD = 0x0308;
		public const uint WM_DRAWITEM = 0x002B;
		public const uint WM_DROPFILES = 0x0233;
		public const uint WM_ENABLE = 0x000A;
		public const uint WM_ENDSESSION = 0x0016;
		public const uint WM_ENTERIDLE = 0x0121;
		public const uint WM_ENTERMENULOOP = 0x0211;
		public const uint WM_ENTERSIZEMOVE = 0x0231;
		public const uint WM_ERASEBKGND = 0x0014;
		public const uint WM_EXITMENULOOP = 0x0212;
		public const uint WM_EXITSIZEMOVE = 0x0232;
		public const uint WM_FONTCHANGE = 0x001D;
		public const uint WM_GETDLGCODE = 0x0087;
		public const uint WM_GETFONT = 0x0031;
		public const uint WM_GETHOTKEY = 0x0033;
		public const uint WM_GETICON = 0x007F;
		public const uint WM_GETMINMAXINFO = 0x0024;
		public const uint WM_GETOBJECT = 0x003D;
		public const uint WM_GETTEXT = 0x000D;
		public const uint WM_GETTEXTLENGTH = 0x000E;
		public const uint WM_HANDHELDFIRST = 0x0358;
		public const uint WM_HANDHELDLAST = 0x035F;
		public const uint WM_HELP = 0x0053;
		public const uint WM_HOTKEY = 0x0312;
		public const uint WM_HSCROLL = 0x0114;
		public const uint WM_HSCROLLCLIPBOARD = 0x030E;
		public const uint WM_ICONERASEBKGND = 0x0027;
		public const uint WM_IME_CHAR = 0x0286;
		public const uint WM_IME_COMPOSITION = 0x010F;
		public const uint WM_IME_COMPOSITIONFULL = 0x0284;
		public const uint WM_IME_CONTROL = 0x0283;
		public const uint WM_IME_ENDCOMPOSITION = 0x010E;
		public const uint WM_IME_KEYDOWN = 0x0290;
		public const uint WM_IME_KEYLAST = 0x010F;
		public const uint WM_IME_KEYUP = 0x0291;
		public const uint WM_IME_NOTIFY = 0x0282;
		public const uint WM_IME_REQUEST = 0x0288;
		public const uint WM_IME_SELECT = 0x0285;
		public const uint WM_IME_SETCONTEXT = 0x0281;
		public const uint WM_IME_STARTCOMPOSITION = 0x010D;
		public const uint WM_INITDIALOG = 0x0110;
		public const uint WM_INITMENU = 0x0116;
		public const uint WM_INITMENUPOPUP = 0x0117;
		public const uint WM_INPUTLANGCHANGE = 0x0051;
		public const uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
		public const uint WM_KEYDOWN = 0x0100;
		public const uint WM_KEYFIRST = 0x0100;
		public const uint WM_KEYLAST = 0x0108;
		public const uint WM_KEYUP = 0x0101;
		public const uint WM_KILLFOCUS = 0x0008;
		public const uint WM_LBUTTONDBLCLK = 0x0203;
		public const uint WM_LBUTTONDOWN = 0x0201;
		public const uint WM_LBUTTONUP = 0x0202;
		public const uint WM_MBUTTONDBLCLK = 0x0209;
		public const uint WM_MBUTTONDOWN = 0x0207;
		public const uint WM_MBUTTONUP = 0x0208;
		public const uint WM_MDIACTIVATE = 0x0222;
		public const uint WM_MDICASCADE = 0x0227;
		public const uint WM_MDICREATE = 0x0220;
		public const uint WM_MDIDESTROY = 0x0221;
		public const uint WM_MDIGETACTIVE = 0x0229;
		public const uint WM_MDIICONARRANGE = 0x0228;
		public const uint WM_MDIMAXIMIZE = 0x0225;
		public const uint WM_MDINEXT = 0x0224;
		public const uint WM_MDIREFRESHMENU = 0x0234;
		public const uint WM_MDIRESTORE = 0x0223;
		public const uint WM_MDISETMENU = 0x0230;
		public const uint WM_MDITILE = 0x0226;
		public const uint WM_MEASUREITEM = 0x002C;
		public const uint WM_MENUCHAR = 0x0120;
		public const uint WM_MENUCOMMAND = 0x0126;
		public const uint WM_MENUDRAG = 0x0123;
		public const uint WM_MENUGETOBJECT = 0x0124;
		public const uint WM_MENURBUTTONUP = 0x0122;
		public const uint WM_MENUSELECT = 0x011F;
		public const uint WM_MOUSEACTIVATE = 0x0021;
		public const uint WM_MOUSEFIRST = 0x0200;
		public const uint WM_MOUSEHOVER = 0x02A1;
		public const uint WM_MOUSELAST = 0x020D;
		public const uint WM_MOUSELEAVE = 0x02A3;
		public const uint WM_MOUSEMOVE = 0x0200;
		public const uint WM_MOUSEWHEEL = 0x020A;
		public const uint WM_MOUSEHWHEEL = 0x020E;
		public const uint WM_MOVE = 0x0003;
		public const uint WM_MOVING = 0x0216;
		public const uint WM_NCACTIVATE = 0x0086;
		public const uint WM_NCCALCSIZE = 0x0083;
		public const uint WM_NCCREATE = 0x0081;
		public const uint WM_NCDESTROY = 0x0082;
		public const uint WM_NCHITTEST = 0x0084;
		public const uint WM_NCLBUTTONDBLCLK = 0x00A3;
		public const uint WM_NCLBUTTONDOWN = 0x00A1;
		public const uint WM_NCLBUTTONUP = 0x00A2;
		public const uint WM_NCMBUTTONDBLCLK = 0x00A9;
		public const uint WM_NCMBUTTONDOWN = 0x00A7;
		public const uint WM_NCMBUTTONUP = 0x00A8;
		public const uint WM_NCMOUSEMOVE = 0x00A0;
		public const uint WM_NCPAINT = 0x0085;
		public const uint WM_NCRBUTTONDBLCLK = 0x00A6;
		public const uint WM_NCRBUTTONDOWN = 0x00A4;
		public const uint WM_NCRBUTTONUP = 0x00A5;
		public const uint WM_NEXTDLGCTL = 0x0028;
		public const uint WM_NEXTMENU = 0x0213;
		public const uint WM_NOTIFY = 0x004E;
		public const uint WM_NOTIFYFORMAT = 0x0055;
		public const uint WM_NULL = 0x0000;
		public const uint WM_PAINT = 0x000F;
		public const uint WM_PAINTCLIPBOARD = 0x0309;
		public const uint WM_PAINTICON = 0x0026;
		public const uint WM_PALETTECHANGED = 0x0311;
		public const uint WM_PALETTEISCHANGING = 0x0310;
		public const uint WM_PARENTNOTIFY = 0x0210;
		public const uint WM_PASTE = 0x0302;
		public const uint WM_PENWINFIRST = 0x0380;
		public const uint WM_PENWINLAST = 0x038F;
		public const uint WM_POWER = 0x0048;
		public const uint WM_POWERBROADCAST = 0x0218;
		public const uint WM_PRINT = 0x0317;
		public const uint WM_PRINTCLIENT = 0x0318;
		public const uint WM_QUERYDRAGICON = 0x0037;
		public const uint WM_QUERYENDSESSION = 0x0011;
		public const uint WM_QUERYNEWPALETTE = 0x030F;
		public const uint WM_QUERYOPEN = 0x0013;
		public const uint WM_QUEUESYNC = 0x0023;
		public const uint WM_QUIT = 0x0012;
		public const uint WM_RBUTTONDBLCLK = 0x0206;
		public const uint WM_RBUTTONDOWN = 0x0204;
		public const uint WM_RBUTTONUP = 0x0205;
		public const uint WM_RENDERALLFORMATS = 0x0306;
		public const uint WM_RENDERFORMAT = 0x0305;
		public const uint WM_SETCURSOR = 0x0020;
		public const uint WM_SETFOCUS = 0x0007;
		public const uint WM_SETFONT = 0x0030;
		public const uint WM_SETHOTKEY = 0x0032;
		public const uint WM_SETICON = 0x0080;
		public const uint WM_SETREDRAW = 0x000B;
		public const uint WM_SETTEXT = 0x000C;
		public const uint WM_SETTINGCHANGE = 0x001A;
		public const uint WM_SHOWWINDOW = 0x0018;
		public const uint WM_SIZE = 0x0005;
		public const uint WM_SIZECLIPBOARD = 0x030B;
		public const uint WM_SIZING = 0x0214;
		public const uint WM_SPOOLERSTATUS = 0x002A;
		public const uint WM_STYLECHANGED = 0x007D;
		public const uint WM_STYLECHANGING = 0x007C;
		public const uint WM_SYNCPAINT = 0x0088;
		public const uint WM_SYSCHAR = 0x0106;
		public const uint WM_SYSCOLORCHANGE = 0x0015;
		public const uint WM_SYSCOMMAND = 0x0112;
		public const uint WM_SYSDEADCHAR = 0x0107;
		public const uint WM_SYSKEYDOWN = 0x0104;
		public const uint WM_SYSKEYUP = 0x0105;
		public const uint WM_TCARD = 0x0052;
		public const uint WM_TIMECHANGE = 0x001E;
		public const uint WM_TIMER = 0x0113;
		public const uint WM_UNDO = 0x0304;
		public const uint WM_UNINITMENUPOPUP = 0x0125;
		public const uint WM_USER = 0x0400;
		public const uint WM_USERCHANGED = 0x0054;
		public const uint WM_VKEYTOITEM = 0x002E;
		public const uint WM_VSCROLL = 0x0115;
		public const uint WM_VSCROLLCLIPBOARD = 0x030A;
		public const uint WM_WINDOWPOSCHANGED = 0x0047;
		public const uint WM_WINDOWPOSCHANGING = 0x0046;
		public const uint WM_WININICHANGE = 0x001A;
		public const uint WM_XBUTTONDBLCLK = 0x020D;
		public const uint WM_XBUTTONDOWN = 0x020B;
		public const uint WM_XBUTTONUP = 0x020C;

		#endregion

		#region SC Commands

		public const uint SC_SIZE = 0xF000;
		public const uint SC_MOVE = 0xF010;
		public const uint SC_MINIMIZE = 0xF020;
		public const uint SC_MAXIMIZE = 0xF030;
		public const uint SC_NEXTWINDOW = 0xF040;
		public const uint SC_PREVWINDOW = 0xF050;
		public const uint SC_CLOSE = 0xF060;
		public const uint SC_VSCROLL = 0xF070;
		public const uint SC_HSCROLL = 0xF080;
		public const uint SC_MOUSEMENU = 0xF090;
		public const uint SC_KEYMENU = 0xF100;
		public const uint SC_ARRANGE = 0xF110;
		public const uint SC_RESTORE = 0xF120;
		public const uint SC_TASKLIST = 0xF130;
		public const uint SC_SCREENSAVE = 0xF140;
		public const uint SC_HOTKEY = 0xF150;
		public const uint SC_DEFAULT = 0xF160;
		public const uint SC_MONITORPOWER = 0xF170;
		public const uint SC_CONTEXTHELP = 0xF180;
		public const uint SC_SEPARATOR = 0xF00F;
		public const uint SCF_ISSECURE = 0x00000001;
		public const uint SC_ICON = SC_MINIMIZE;
		public const uint SC_ZOOM = SC_MAXIMIZE;

		#endregion

		[DllImport("user32")]
		public static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

		[DllImport("User32")]
		public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

		[DllImport("kernel32.dll")]
		public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll")]
		public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpszClass, string lpszWindow);

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint dwProcessId);

		[DllImport("User32.dll")]
		public static extern Boolean EnumChildWindows(IntPtr hWndParent, Delegate lpEnumFunc, int lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		public static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);

		[DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(POINT Point);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetCursorPos(out POINT pt);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		public static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
		{
			MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

			// Adjust the maximized size and position to fit the work area of the correct monitor
			int MONITOR_DEFAULTTONEAREST = 0x00000002;
			IntPtr monitor = WinApi.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

			if (monitor != IntPtr.Zero)
			{
				MONITORINFO monitorInfo = new MONITORINFO();
				GetMonitorInfo(monitor, monitorInfo);
				RECT rcWorkArea = monitorInfo.rcWork;
				RECT rcMonitorArea = monitorInfo.rcMonitor;
				mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
				mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
				mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
				mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
				mmi.ptMinTrackSize.x = 240;
				mmi.ptMinTrackSize.y = 320;
			}

			Marshal.StructureToPtr(mmi, lParam, true);
		}

		[DllImport("winmm.dll", SetLastError = true)]
		public static extern bool PlaySound(string pszSound, UIntPtr hmod, uint fdwSound);

		[Flags]
		public enum SoundFlags
		{
			/// <summary>play synchronously (default)</summary>
			SND_SYNC = 0x0000,
			/// <summary>play asynchronously</summary>
			SND_ASYNC = 0x0001,
			/// <summary>silence (!default) if sound not found</summary>
			SND_NODEFAULT = 0x0002,
			/// <summary>pszSound points to a memory file</summary>
			SND_MEMORY = 0x0004,
			/// <summary>loop the sound until next sndPlaySound</summary>
			SND_LOOP = 0x0008,
			/// <summary>don't stop any currently playing sound</summary>
			SND_NOSTOP = 0x0010,
			/// <summary>Stop Playing Wave</summary>
			SND_PURGE = 0x40,
			/// <summary>don't wait if the driver is busy</summary>
			SND_NOWAIT = 0x00002000,
			/// <summary>name is a registry alias</summary>
			SND_ALIAS = 0x00010000,
			/// <summary>alias is a predefined id</summary>
			SND_ALIAS_ID = 0x00110000,
			/// <summary>name is file name</summary>
			SND_FILENAME = 0x00020000,
			/// <summary>name is resource name or atom</summary>
			SND_RESOURCE = 0x00040004,
			/// <summary>play in vista queue</summary>
			SND_SYSTEM = 0x00200000,
		}

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
	}
}