using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inbox2.Framework.Interop
{
    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
        /// <summary>
        /// </summary>            
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

        /// <summary>
        /// </summary>            
        public RECT rcMonitor = new RECT();

        /// <summary>
        /// </summary>            
        public RECT rcWork = new RECT();

        /// <summary>
        /// </summary>            
        public int dwFlags = 0;
    }
}
