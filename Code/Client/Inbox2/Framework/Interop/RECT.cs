using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inbox2.Framework.Interop
{
    /// <summary> Win32 </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RECT
    {
        /// <summary> Win32 </summary>
        public int left;
        /// <summary> Win32 </summary>
        public int top;
        /// <summary> Win32 </summary>
        public int right;
        /// <summary> Win32 </summary>
        public int bottom;

        /// <summary> Win32 </summary>
        public static readonly RECT Empty = new RECT();

        /// <summary> Win32 </summary>
        public int Width
        {
            get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
        }
        /// <summary> Win32 </summary>
        public int Height
        {
            get { return bottom - top; }
        }

        /// <summary> Win32 </summary>
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }


        /// <summary> Win32 </summary>
        public RECT(RECT rcSrc)
        {
            this.left = rcSrc.left;
            this.top = rcSrc.top;
            this.right = rcSrc.right;
            this.bottom = rcSrc.bottom;
        }

        /// <summary> Win32 </summary>
        public bool IsEmpty
        {
            get
            {
                // BUGBUG : On Bidi OS (hebrew arabic) left > right
                return left >= right || top >= bottom;
            }
        }
        /// <summary> Return a user friendly representation of this struct </summary>
        public override string ToString()
        {
            if (this == RECT.Empty) { return "RECT {Empty}"; }
            return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
        }

        /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is RECT)) { return false; }
            return (this == (RECT)obj);
        }

        /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
        public override int GetHashCode()
        {
            return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
        }


        /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
        public static bool operator ==(RECT rect1, RECT rect2)
        {
            return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
        }

        /// <summary> Determine if 2 RECT are different(deep compare)</summary>
        public static bool operator !=(RECT rect1, RECT rect2)
        {
            return !(rect1 == rect2);
        }


    }
}
