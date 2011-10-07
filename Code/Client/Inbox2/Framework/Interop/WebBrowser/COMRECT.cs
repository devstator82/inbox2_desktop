using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[StructLayout( LayoutKind.Sequential )]
	public class COMRECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
		public COMRECT()
		{
		}

		public COMRECT( Rectangle r )
		{
			this.left = r.X;
			this.top = r.Y;
			this.right = r.Right;
			this.bottom = r.Bottom;
		}

		public COMRECT( int left, int top, int right, int bottom )
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public static COMRECT FromXYWH( int x, int y, int width, int height )
		{
			return new COMRECT( x, y, x + width, y + height );
		}

		public override string ToString()
		{
			return string.Concat( new object[] { "Left = ", this.left, " Top ", this.top, " Right = ", this.right, " Bottom = ", this.bottom } );
		}
	}
}
