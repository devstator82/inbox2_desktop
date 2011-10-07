
using System.Windows;

namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.ComponentModel;


    #region Native Values

    /// <summary>
    /// Non-client hit test values, HT*
    /// </summary>
    internal enum HT
    {
        ERROR = -2,
        TRANSPARENT = -1,
        NOWHERE = 0,
        CLIENT = 1,
        CAPTION = 2,
        SYSMENU = 3,
        GROWBOX = 4,
        MENU = 5,
        HSCROLL = 6,
        VSCROLL = 7,
        MINBUTTON = 8,
        MAXBUTTON = 9,
        LEFT = 10,
        RIGHT = 11,
        TOP = 12,
        TOPLEFT = 13,
        TOPRIGHT = 14,
        BOTTOM = 15,
        BOTTOMLEFT = 16,
        BOTTOMRIGHT = 17,
        BORDER = 18,
        OBJECT = 19,
        CLOSE = 20,
        HELP = 21
    }

    /// <summary>
    /// GetWindowLongPtr values, GWL_*
    /// </summary>
    internal enum GWL
    {
         WNDPROC =    (-4),
         HINSTANCE =  (-6),
         HWNDPARENT = (-8),
         STYLE =      (-16),
         EXSTYLE =    (-20),
         USERDATA =   (-21),
         ID =         (-12)
    }

    /// <summary>
    /// SystemParameterInfo values, SPI_*
    /// </summary>
    internal enum SPI
    {
        GETNONCLIENTMETRICS = 41,
    }

    /// <summary>
    /// WindowStyle values, WS_*
    /// </summary>
    [Flags]
    internal enum WS : uint
    {
        OVERLAPPED = 0x00000000,
        POPUP = 0x80000000,
        CHILD = 0x40000000,
        MINIMIZE = 0x20000000,
        VISIBLE = 0x10000000,
        DISABLED = 0x08000000,
        CLIPSIBLINGS = 0x04000000,
        CLIPCHILDREN = 0x02000000,
        MAXIMIZE = 0x01000000,
        BORDER = 0x00800000,
        DLGFRAME = 0x00400000,
        VSCROLL = 0x00200000,
        HSCROLL = 0x00100000,
        SYSMENU = 0x00080000,
        THICKFRAME = 0x00040000,
        GROUP = 0x00020000,
        TABSTOP = 0x00010000,

        MINIMIZEBOX = 0x00020000,
        MAXIMIZEBOX = 0x00010000,

        CAPTION = BORDER | DLGFRAME,
        TILED = OVERLAPPED,
        ICONIC = MINIMIZE,
        SIZEBOX = THICKFRAME,
        TILEDWINDOW = OVERLAPPEDWINDOW,

        OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
        POPUPWINDOW = POPUP | BORDER | SYSMENU,
        CHILDWINDOW = CHILD,
    }

    /// <summary>
    /// Window message values, WM_*
    /// </summary>
    internal enum WM
    {
        NULL = 0x0000,
        CREATE = 0x0001,
        DESTROY = 0x0002,
        MOVE = 0x0003,
        SIZE = 0x0005,
        ACTIVATE = 0x0006,
        SETFOCUS = 0x0007,
        KILLFOCUS = 0x0008,
        ENABLE = 0x000A,
        SETREDRAW = 0x000B,
        SETTEXT = 0x000C,
        GETTEXT = 0x000D,
        GETTEXTLENGTH = 0x000E,
        PAINT = 0x000F,
        CLOSE = 0x0010,
        QUERYENDSESSION = 0x0011,
        QUIT = 0x0012,
        QUERYOPEN = 0x0013,
        ERASEBKGND = 0x0014,
        SYSCOLORCHANGE = 0x0015,

        WINDOWPOSCHANGING = 0x0046,
        WINDOWPOSCHANGED = 0x0047,

        SETICON = 0x0080,
        NCCREATE = 0x0081,
        NCDESTROY = 0x0082,
        NCCALCSIZE = 0x0083,
        NCHITTEST = 0x0084,
        NCPAINT = 0x0085,
        NCACTIVATE = 0x0086,
        GETDLGCODE = 0x0087,
        SYNCPAINT = 0x0088,
        NCMOUSEMOVE = 0x00A0,
        NCLBUTTONDOWN = 0x00A1,
        NCLBUTTONUP = 0x00A2,
        NCLBUTTONDBLCLK = 0x00A3,
        NCRBUTTONDOWN = 0x00A4,
        NCRBUTTONUP = 0x00A5,
        NCRBUTTONDBLCLK = 0x00A6,
        NCMBUTTONDOWN = 0x00A7,
        NCMBUTTONUP = 0x00A8,
        NCMBUTTONDBLCLK = 0x00A9,

        SYSKEYDOWN = 0x0104,
        SYSKEYUP = 0x0105,
        SYSCHAR = 0x0106,
        SYSDEADCHAR = 0x0107,
        SYSCOMMAND = 0x0112,

        DWMCOMPOSITIONCHANGED = 0x031E,
        USER = 0x0400,
        APP = 0x8000,
    }

    /// <summary>
    /// Window style extended values, WS_EX_*
    /// </summary>
    [Flags]
    internal enum WS_EX : uint
    {
        None = 0,
        DLGMODALFRAME     = 0x00000001,
        NOPARENTNOTIFY    = 0x00000004,
        TOPMOST           = 0x00000008,
        ACCEPTFILES       = 0x00000010,
        TRANSPARENT       = 0x00000020,
        MDICHILD          = 0x00000040,
        TOOLWINDOW        = 0x00000080,
        WINDOWEDGE        = 0x00000100,
        CLIENTEDGE        = 0x00000200,
        CONTEXTHELP       = 0x00000400,
        RIGHT             = 0x00001000,
        LEFT              = 0x00000000,
        RTLREADING        = 0x00002000,
        LTRREADING        = 0x00000000,
        LEFTSCROLLBAR     = 0x00004000,
        RIGHTSCROLLBAR    = 0x00000000,
        CONTROLPARENT     = 0x00010000,
        STATICEDGE        = 0x00020000,
        APPWINDOW         = 0x00040000,
        LAYERED           = 0x00080000,
        NOINHERITLAYOUT   = 0x00100000, // Disable inheritence of mirroring by children
        LAYOUTRTL         = 0x00400000, // Right to left mirroring
        COMPOSITED        = 0x02000000,
        NOACTIVATE        = 0x08000000,
        OVERLAPPEDWINDOW  = (WINDOWEDGE | CLIENTEDGE),
        PALETTEWINDOW     = (WINDOWEDGE | TOOLWINDOW | TOPMOST),
    }

    /// <summary>
    /// GetDeviceCaps nIndex values.
    /// </summary>
    internal enum DeviceCap
    {
        /// <summary>
        /// Logical pixels inch in X
        /// </summary>
        LOGPIXELSX = 88,
        /// <summary>
        /// Logical pixels inch in Y
        /// </summary>
        LOGPIXELSY = 90,
    }

    /// <summary>
    /// EnableMenuItem uEnable values, MF_*
    /// </summary>
    [Flags]
    internal enum MF : uint
    {
        /// <summary>
        /// Possible return value for EnableMenuItem
        /// </summary>
        DOES_NOT_EXIST = unchecked((uint)-1),
        ENABLED = 0,
        BYCOMMAND = 0,
        GRAYED = 1,
        DISABLED = 2,
    }

    /// <summary>Specifies the type of visual style attribute to set on a window.</summary>
    internal enum WINDOWTHEMEATTRIBUTETYPE : uint
    {
        /// <summary>Non-client area window attributes will be set.</summary>
        WTA_NONCLIENT = 1,
    }

    /// <summary>
    /// WindowThemeNonClientAttributes
    /// </summary>
    [Flags]
    internal enum WTNCA : uint
    {
        /// <summary>Prevents the window caption from being drawn.</summary>
        NODRAWCAPTION = 0x00000001,
        /// <summary>Prevents the system icon from being drawn.</summary>
        NODRAWICON = 0x00000002,
        /// <summary>Prevents the system icon menu from appearing.</summary>
        NOSYSMENU = 0x00000004,
        /// <summary>Prevents mirroring of the question mark, even in right-to-left (RTL) layout.</summary>
        NOMIRRORHELP = 0x00000008,
        /// <summary> A mask that contains all the valid bits.</summary>
        VALIDBITS = NODRAWCAPTION | NODRAWICON | NOMIRRORHELP | NOSYSMENU,
    }

    /// <summary>
    /// SetWindowPos options
    /// </summary>
    [Flags]
    internal enum SWP
    {
        ASYNCWINDOWPOS = 0x4000,
        DEFERERASE = 0x2000,
        DRAWFRAME = 0x0020,
        FRAMECHANGED = 0x0020,
        HIDEWINDOW = 0x0080,
        NOACTIVATE = 0x0010,
        NOCOPYBITS = 0x0100,
        NOMOVE = 0x0002,
        NOOWNERZORDER = 0x0200,
        NOREDRAW = 0x0008,
        NOREPOSITION = 0x0200,
        NOSENDCHANGING = 0x0400,
        NOSIZE = 0x0001,
        NOZORDER = 0x0004,
        SHOWWINDOW = 0x0040,
    }

    /// <summary>
    /// ShowWindow options
    /// </summary>
    internal enum SW
    {
        HIDE         = 0,
        SHOWNORMAL     = 1,
        NORMAL        = 1,
        SHOWMINIMIZED    = 2,
        SHOWMAXIMIZED    = 3,
        MAXIMIZE    = 3,
        SHOWNOACTIVATE    = 4,
        SHOW        = 5,
        MINIMIZE    = 6,
        SHOWMINNOACTIVE    = 7,
        SHOWNA        = 8,
        RESTORE    = 9,
        SHOWDEFAULT    = 10,
        FORCEMINIMIZE    = 11,
    }

    internal enum SC
    {
        SIZE = 0xF000,
        MOVE = 0xF010,
        MINIMIZE = 0xF020,
        MAXIMIZE = 0xF030,
        NEXTWINDOW = 0xF040,
        PREVWINDOW = 0xF050,
        CLOSE = 0xF060,
        VSCROLL = 0xF070,
        HSCROLL = 0xF080,
        MOUSEMENU = 0xF090,
        KEYMENU = 0xF100,
        ARRANGE = 0xF110,
        RESTORE = 0xF120,
        TASKLIST = 0xF130,
        SCREENSAVE = 0xF140,
        HOTKEY = 0xF150,
        DEFAULT = 0xF160,
        MONITORPOWER = 0xF170,
        CONTEXTHELP = 0xF180,
        SEPARATOR = 0xF00F,
        /// <summary>
        /// SCF_ISSECURE
        /// </summary>
        F_ISSECURE = 0x00000001,
        ICON = MINIMIZE,
        ZOOM = MAXIMIZE,
    }

    #endregion

    #region Native Types

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct LOGFONT
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NONCLIENTMETRICS
    {
        public int cbSize;
        public int iBorderWidth;
        public int iScrollWidth;
        public int iScrollHeight;
        public int iCaptionWidth;
        public int iCaptionHeight;
        public LOGFONT lfCaptionFont;
        public int iSmCaptionWidth;
        public int iSmCaptionHeight;
        public LOGFONT lfSmCaptionFont;
        public int iMenuWidth;
        public int iMenuHeight;
        public LOGFONT lfMenuFont;
        public LOGFONT lfStatusFont;
        public LOGFONT lfMessageFont;
        // Vista only
        public int iPaddedBorderWidth;

        public static NONCLIENTMETRICS VistaMetricsStruct
        {
            get
            {
                var ncm = new NONCLIENTMETRICS();
                ncm.cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS));
                return ncm;
            }
        }

        public static NONCLIENTMETRICS XPMetricsStruct
        {
            get
            {
                var ncm = new NONCLIENTMETRICS();
                // Account for the missing iPaddedBorderWidth
                ncm.cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS)) - sizeof(int);
                return ncm;
            }
        }
    }

    /// <summary>Defines options that are used to set window visual style attributes.</summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct WTA_OPTIONS
    {
        // public static readonly uint Size = (uint)Marshal.SizeOf(typeof(WTA_OPTIONS));
        public const uint Size = 8;

        /// <summary>
        /// A combination of flags that modify window visual style attributes.
        /// Can be a combination of the WTNCA constants.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used by native code.")]
        [FieldOffset(0)]
        public WTNCA dwFlags;

        /// <summary>
        /// A bitmask that describes how the values specified in dwFlags should be applied.
        /// If the bit corresponding to a value in dwFlags is 0, that flag will be removed.
        /// If the bit is 1, the flag will be added.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used by native code.")]
        [FieldOffset(4)]
        public WTNCA dwMask;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MARGINS
    {
        /// <summary>Width of left border that retains its size.</summary>
        public int cxLeftWidth;
        /// <summary>Width of right border that retains its size.</summary>
        public int cxRightWidth;
        /// <summary>Height of top border that retains its size.</summary>
        public int cyTopHeight;
        /// <summary>Height of bottom border that retains its size.</summary>
        public int cyBottomHeight;

		public MARGINS(Thickness t)
		{
			cxLeftWidth = (int)t.Left;
			cxRightWidth = (int)t.Right;
			cyTopHeight = (int)t.Top;
			cyBottomHeight = (int)t.Bottom;
		}
    };

    [StructLayout(LayoutKind.Sequential)]
    internal class MONITORINFO
    {
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        public RECT rcMonitor;
        public RECT rcWork;
        public int dwFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public void Offset(int dx, int dy)
        {
            left += dx;
            top += dy;
            right += dx;
            bottom += dy;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class WINDOWPLACEMENT
    {
        public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
        public int flags;
        public SW showCmd;
        public POINT ptMinPosition;
        public POINT ptMaxPosition;
        public RECT rcNormalPosition;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int flags;
    }

    #endregion

    // Some native methods are shimmed through public versions that handle converting failures into thrown exceptions.
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        /// <summary>
        /// Delegate declaration that matches WndProc signatures.
        /// </summary>
        public delegate IntPtr MessageHandler(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled);

        [DllImport("gdi32.dll", EntryPoint="CreateRoundRectRgn", SetLastError=true)]
        private static extern IntPtr _CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public static IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse)
        {
            IntPtr ret = _CreateRoundRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect, nWidthEllipse, nHeightEllipse);
            if (IntPtr.Zero == ret)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return ret;
        }

        [DllImport("gdi32.dll", EntryPoint="CreateRectRgn", SetLastError=true)]
        private static extern IntPtr _CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        public static IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
        {
            IntPtr ret = _CreateRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect);
            if (IntPtr.Zero == ret)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return ret;
        }

        [DllImport("gdi32.dll", EntryPoint="CreateRectRgnIndirect", SetLastError = true)]
        private static extern IntPtr _CreateRectRgnIndirect([In] ref RECT lprc);

        public static IntPtr CreateRectRgnIndirect(RECT lprc)
        {
            IntPtr ret = _CreateRectRgnIndirect(ref lprc);
            if (IntPtr.Zero == ret)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return ret;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll", EntryPoint="DwmIsCompositionEnabled", PreserveSig = false)]
        private static extern void _DwmIsCompositionEnabled([Out, MarshalAs(UnmanagedType.Bool)] out bool pfEnabled);

        public static bool DwmIsCompositionEnabled()
        {
            bool composited;
            _DwmIsCompositionEnabled(out composited);
            return composited;
        }

        [DllImport("dwmapi.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DwmDefWindowProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);

        [DllImport("user32.dll", EntryPoint="EnableMenuItem")]
        private static extern int _EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable);

        public static MF EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable)
        {
            // Returns the previous state of the menu item, or -1 if the menu item does not exist.
            int iRet = _EnableMenuItem(hMenu, uIDEnableItem, uEnable);
            return (MF)iRet;
        }

        [DllImport("user32.dll", EntryPoint="GetDC", SetLastError=true)]
        private static extern IntPtr _GetDC(IntPtr hwnd);

        public static IntPtr GetDC(IntPtr hwnd)
        {
            IntPtr hdc = _GetDC(hwnd);
            if (IntPtr.Zero == hdc)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return hdc;
        }

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, DeviceCap nIndex);

        [DllImport("user32.dll", EntryPoint="GetMonitorInfo", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool _GetMonitorInfo(IntPtr hMonitor, [In, Out] MONITORINFO lpmi);

        public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
        {
            var mi = new MONITORINFO();
            if (!_GetMonitorInfo(hMonitor, mi))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return mi;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

        // This is aliased as a macro in 32bit Windows.
        public static IntPtr GetWindowLongPtr(IntPtr hwnd, GWL nIndex)
        {
            IntPtr ret = IntPtr.Zero;
            if (8 == IntPtr.Size)
            {
                ret = GetWindowLongPtr64(hwnd, nIndex);
            }
            else
            {
                ret = GetWindowLongPtr32(hwnd, nIndex);
            }
            if (IntPtr.Zero == ret)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return ret;
        }

        [
            SuppressMessage(
                "Microsoft.Interoperability",
                "CA1400:PInvokeEntryPointsShouldExist"),
            SuppressMessage(
                "Microsoft.Portability",
                "CA1901:PInvokeDeclarationsShouldBePortable",
                MessageId = "return"),
            DllImport("user32.dll", EntryPoint="GetWindowLong", SetLastError=true)
        ]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, GWL nIndex);

        [
            SuppressMessage(
                "Microsoft.Portability",
                "CA1901:PInvokeDeclarationsShouldBePortable",
                MessageId = "return"),
            SuppressMessage(
                "Microsoft.Interoperability",
                "CA1400:PInvokeEntryPointsShouldExist"),
            DllImport("user32.dll", EntryPoint="GetWindowLongPtr", SetLastError=true)
        ]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hwnd, WINDOWPLACEMENT lpwndpl);

        public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
        {
            WINDOWPLACEMENT wndpl = new WINDOWPLACEMENT();
            if (GetWindowPlacement(hwnd, wndpl))
            {
                return wndpl;
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", EntryPoint="PostMessage", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool _PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        public static void PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam)
        {
            if (!_PostMessage(hWnd, Msg, wParam, lParam))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // This is aliased as a macro in 32bit Windows.
        public static IntPtr SetWindowLongPtr(IntPtr hwnd, GWL nIndex, IntPtr dwNewLong)
        {
            if (8 == IntPtr.Size)
            {
                return SetWindowLongPtr64(hwnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr32(hwnd, nIndex, dwNewLong);
        }

        [
            SuppressMessage(
                "Microsoft.Portability",
                "CA1901:PInvokeDeclarationsShouldBePortable",
                MessageId = "2"),
            SuppressMessage(
                "Microsoft.Interoperability",
                "CA1400:PInvokeEntryPointsShouldExist"),
            SuppressMessage(
                "Microsoft.Portability",
                "CA1901:PInvokeDeclarationsShouldBePortable",
                MessageId = "return"),
            DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)
        ]
        private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

        [
            SuppressMessage(
                "Microsoft.Portability",
                "CA1901:PInvokeDeclarationsShouldBePortable",
                MessageId = "return"),
            SuppressMessage(
                "Microsoft.Interoperability",
                "CA1400:PInvokeEntryPointsShouldExist"),
            DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)
        ]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint="SetWindowRgn", SetLastError=true)]
        private static extern int _SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);

        public static void SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw)
        {
            int err =_SetWindowRgn(hWnd, hRgn, bRedraw);
            if (0 == err)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        [DllImport("user32.dll", EntryPoint="SetWindowPos", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

        public static void SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags)
        {
            if (!_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // This function is strange in that it returns a BOOL if TPM_RETURNCMD isn't specified, but otherwise the command Id.
        // Currently it's only used with TPM_RETURNCMD, so making the signature match that.
        [DllImport("user32.dll")]
        public static extern uint TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);
    }
}
