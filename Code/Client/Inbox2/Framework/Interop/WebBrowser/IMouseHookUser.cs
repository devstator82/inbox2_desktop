/*
 * This sample is released as public domain.  It is distributed in the hope that 
 * it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 * 
 * Leslie Godwn (leslie.godwin@gmail.com):
 * This class was taken from the "Olvio.Windows.Forms.Docking.SafeMouseHookManager" source.
 * A good example of how to use it can be found at (which is where I found it) : 
 * http://www.codeproject.com/useritems/MBtn2DblClick.asp
 * 
 * Leslie Godwin - 2007/07/25:
 * I made some minor changes with documentation and namespacing.
 */


using System.Drawing;

namespace Inbox2.Framework.Interop.WebBrowser
{
	/// <summary>
	/// Callback procedure for the System mouse hook handler.
	/// </summary>
	/// 
	public interface IMouseHookUser
	{
		/// <summary>
		/// Called for all system mouse events before any window receives the message.
		/// </summary>
		/// <param name="code">nCode</param>
		/// <param name="mainParameter">wParam</param>
		/// <param name="additionalParameter">lParam</param>
		/// <param name="point">Mouse screen <see cref="Point"/>.</param>
		/// <param name="extraInfo">dwExtraInfo</param>
		/// <returns>Return <c>false</c> to let the message get processed by the next handler.</returns>
		/// 
		bool SystemMouseHookProc(int code, int mainParameter, int additionalParameter, Point point, int extraInfo); 
	}
}


