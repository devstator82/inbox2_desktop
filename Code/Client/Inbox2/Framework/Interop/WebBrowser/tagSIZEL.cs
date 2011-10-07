using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[StructLayout( LayoutKind.Sequential )]
	public sealed class tagSIZEL
	{
		public int cx;
		public int cy;
		public tagSIZEL()
		{
		}
	}
}