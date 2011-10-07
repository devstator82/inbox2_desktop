using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inbox2.Framework.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct LVITEM
	{
		public int mask;
		public int iItem;
		public int iSubItem;
		public int state;
		public int stateMask;
		public IntPtr pszText; // string
		public int cchTextMax;
		public int iImage;
		public IntPtr lParam;
		public int iIndent;
		public int iGroupId;
		public int cColumns;
		public IntPtr puColumns;
	}
}
