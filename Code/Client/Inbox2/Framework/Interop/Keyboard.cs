using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace Inbox2.Framework.Interop
{
	public enum Win32Message
	{
		WM_KEYDOWN = 0x100,
		WM_KEYUP = 0x101,
		WM_CHAR = 0x102,
		WM_SYSKEYDOWN = 0x104,
		WM_SYSKEYUP = 0x105,
		WM_SYSCHAR = 0x106,
		WM_SYSDEADCHAR = 0x107,
		WM_MOUSEDOWN = 0x201
	}

	[Flags]
	public enum ControlKeyStates
	{
		CapsLockOn = 0x80,
		EnhancedKey = 0x100,
		LeftAltPressed = 2,
		LeftCtrlPressed = 8,
		NumLockOn = 0x20,
		RightAltPressed = 1,
		RightCtrlPressed = 4,
		ScrollLockOn = 0x40,
		ShiftPressed = 0x10
	}

	public class NativeMethods
	{
		public enum MapType : uint
		{
			MAPVK_VK_TO_CHAR = 2,
			MAPVK_VK_TO_VSC = 0,
			MAPVK_VK_TO_VSC_EX = 4,
			MAPVK_VSC_TO_VK = 1,
			MAPVK_VSC_TO_VK_EX = 3
		}

		[DllImport("user32.dll")]
		public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

		[DllImport("user32.dll")]
		public static extern bool GetKeyboardState(byte[] lpKeyState);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct KeyInfo
	{
		private int virtualKeyCode;
		private char character;
		private ControlKeyStates controlKeyState;
		private bool keyDown;
		public int VirtualKeyCode
		{
			get
			{
				return this.virtualKeyCode;
			}
			set
			{
				this.virtualKeyCode = value;
			}
		}
		public char Character
		{
			get
			{
				return this.character;
			}
			set
			{
				this.character = value;
			}
		}
		public ControlKeyStates ControlKeyState
		{
			get
			{
				return this.controlKeyState;
			}
			set
			{
				this.controlKeyState = value;
			}
		}
		public bool KeyDown
		{
			get
			{
				return this.keyDown;
			}
			set
			{
				this.keyDown = value;
			}
		}
		public KeyInfo(int virtualKeyCode, char ch, ControlKeyStates controlKeyState, bool keyDown)
		{
			this.virtualKeyCode = virtualKeyCode;
			this.character = ch;
			this.controlKeyState = controlKeyState;
			this.keyDown = keyDown;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", new object[] { this.VirtualKeyCode, this.Character, this.ControlKeyState, this.KeyDown });
		}

		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is KeyInfo)
			{
				flag = this == ((KeyInfo)obj);
			}
			return flag;
		}

		public override int GetHashCode()
		{
			uint num = this.KeyDown ? 0x10000000u : 0;
			num |= ((uint)this.ControlKeyState) << 0x10;
			num |= (uint)this.VirtualKeyCode;
			return num.GetHashCode();
		}

		public static bool operator ==(KeyInfo first, KeyInfo second)
		{
			return ((((first.Character == second.Character) && (first.ControlKeyState == second.ControlKeyState)) && (first.KeyDown == second.KeyDown)) && (first.VirtualKeyCode == second.VirtualKeyCode));
		}

		public static bool operator !=(KeyInfo first, KeyInfo second)
		{
			return !(first == second);
		}
	}

	public static class KeyboardExtensions
	{
		/// <summary>The set of valid MapTypes used in MapVirtualKey
		/// </summary>
		/// <remarks></remarks>
		public enum MapType : uint
		{
			/// <summary>uCode is a virtual-key code and is translated into a scan code.
			/// If it is a virtual-key code that does not distinguish between left- and
			/// right-hand keys, the left-hand scan code is returned.
			/// If there is no translation, the function returns 0.
			/// </summary>
			/// <remarks></remarks>
			MAPVK_VK_TO_VSC = 0x0,

			/// <summary>uCode is a scan code and is translated into a virtual-key code that
			/// does not distinguish between left- and right-hand keys. If there is no
			/// translation, the function returns 0.
			/// </summary>
			/// <remarks></remarks>
			MAPVK_VSC_TO_VK = 0x1,

			/// <summary>uCode is a virtual-key code and is translated into an unshifted
			/// character value in the low-order word of the return value. Dead keys (diacritics)
			/// are indicated by setting the top bit of the return value. If there is no
			/// translation, the function returns 0.
			/// </summary>
			/// <remarks></remarks>
			MAPVK_VK_TO_CHAR = 0x2,

			/// <summary>Windows NT/2000/XP: uCode is a scan code and is translated into a
			/// virtual-key code that distinguishes between left- and right-hand keys. If
			/// there is no translation, the function returns 0.
			/// </summary>
			/// <remarks></remarks>
			MAPVK_VSC_TO_VK_EX = 0x3,

			/// <summary>Not currently documented
			/// </summary>
			/// <remarks></remarks>
			MAPVK_VK_TO_VSC_EX = 0x4,
		}

		/// <summary>
		/// The ToUnicode function translates the specified virtual-key code and keyboard state 
		/// to the corresponding Unicode character or characters. To specify a handle to the keyboard layout 
		/// to use to translate the specified code, use the ToUnicodeEx function.
		/// </summary>
		/// <param name="wVirtKey">Specifies the virtual-key code to be translated.</param>
		/// <param name="wScanCode">Specifies the hardware scan code of the key to be translated. 
		/// The high-order bit of this value is set if the key is up.</param>
		/// <param name="lpKeyState">Pointer to a 256-byte array that contains the current keyboard state.
		///     Each element (byte) in the array contains the state of one key. 
		///     If the high-order bit of a byte is set, the key is down.</param>
		/// <param name="pwszBuff">Pointer to the buffer that receives the translated Unicode character 
		///     or characters. However, this buffer may be returned without being null-terminated 
		///     even though the variable name suggests that it is null-terminated.
		/// </param>
		/// <param name="cchBuff">Specifies the size, in wide characters, of the buffer pointed to by the pwszBuff parameter.</param>
		/// <param name="wFlags">Specifies the behavior of the function. If bit 0 is set, a menu is active. Bits 1 through 31 are reserved.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern int ToUnicode(
			uint wVirtKey,
			uint wScanCode,
			byte[] lpKeyState,
			[Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] 
            StringBuilder pwszBuff,
			int cchBuff,
			uint wFlags);

		[DllImport("user32.dll")]
		public static extern bool GetKeyboardState(byte[] lpKeyState);

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint uCode, MapType uMapType);


		/// <summary>
		/// Create a KeyInfo from a <see cref="KeyEventArgs"/>
		/// </summary>
		/// <param name="e">The KeyEventArgs</param>
		/// <returns>A KeyInfo struct</returns>
		public static KeyInfo ToKeyInfo(this KeyEventArgs e)
		{
			int vk = KeyInterop.VirtualKeyFromKey(e.Key);
			char c = GetChar(vk);
			return new KeyInfo(vk, c, e.KeyboardDevice.GetControlKeyStates(), e.IsDown);
		}

		/// <summary>
		/// Get the char from a VirtualKey -- MUST be called within the keyboard event handler.
		/// </summary>
		/// <param name="vk">VirtualKey code</param>
		/// <returns>the character represented by the VirtualKey</returns>
		private static char GetChar(int vk)
		{
			var ks = new byte[256];
			GetKeyboardState(ks);

			var sc = MapVirtualKey((uint)vk, MapType.MAPVK_VK_TO_VSC);
			var sb = new StringBuilder(2);
			var ch = (char)0;

			switch (ToUnicode((uint)vk, sc, ks, sb, sb.Capacity, 0))
			{
				case -1: break;
				case 0: break;
				case 1:
					{
						ch = sb[0];
						break;
					}
				default:
					{
						ch = sb[0];
						break;
					}
			}
			return ch;
		}

		private static ControlKeyStates GetControlKeyStates(this KeyboardDevice kb)
		{
			ControlKeyStates controlStates = default(ControlKeyStates);

			if (kb.IsKeyDown(Key.LeftCtrl))
			{
				controlStates |= ControlKeyStates.LeftCtrlPressed;
			}
			if (kb.IsKeyDown(Key.LeftAlt))
			{
				controlStates |= ControlKeyStates.LeftAltPressed;
			}
			if (kb.IsKeyDown(Key.RightAlt))
			{
				controlStates |= ControlKeyStates.RightAltPressed;
			}
			if (kb.IsKeyDown(Key.RightCtrl))
			{
				controlStates |= ControlKeyStates.RightCtrlPressed;
			}
			if (kb.IsKeyToggled(Key.Scroll))
			{
				controlStates |= ControlKeyStates.ScrollLockOn;
			}
			if (kb.IsKeyToggled(Key.CapsLock))
			{
				controlStates |= ControlKeyStates.CapsLockOn;
			}
			if (kb.IsKeyToggled(Key.NumLock))
			{
				controlStates |= ControlKeyStates.NumLockOn;
			}
			if (kb.IsKeyDown(Key.LeftShift) || kb.IsKeyDown(Key.RightShift))
			{
				controlStates |= ControlKeyStates.ShiftPressed;
			}
			return controlStates;
		}


		//// we didn't need these, but I'm saving them anyway
		//private static int VK_CAPITAL = 0x14;
		//private static byte TOGGLED = 0x01;
		//private static byte PRESSED = 0x80;
		// System.Diagnostics.Trace.WriteLine(string.Format("CapsLock: {0}",((ks[VK_CAPITAL] & TOGGLED)>0)));


		//// These overloads are only PARTIALLY correct
		//public static KeyInfo ToKeyInfo(this Key k)
		//{
		//   var xConverter = new KeyConverter();
		//   char xChar = xConverter.ConvertToString(k)[0];
		//   return new KeyInfo(KeyInterop.VirtualKeyFromKey(k), xChar, ControlKeyStates.NumLockOn, false);
		//}

		//public static KeyInfo ToKeyInfo(this char c)
		//{
		//   var kConverter = new KeyConverter();
		//   Key xKey = (Key)kConverter.ConvertFromString(new string(c, 1));
		//   return new KeyInfo(KeyInterop.VirtualKeyFromKey(xKey), c, ControlKeyStates.NumLockOn, false);
		//}

		///// <summary>
		///// Create a KeyInfo from a character, specifying whether it's up or down
		///// </summary>
		///// <param name="c">Alphanumeric characters only...</param>
		///// <param name="keyDown">True for key down</param>
		///// <returns>A KeyInfo object</returns>
		//public static KeyInfo ToKeyInfo(this char c, bool keyDown)
		//{
		//   var kConverter = new KeyConverter();
		//   Key xKey = (Key)kConverter.ConvertFromString(new string(c, 1));
		//   return new KeyInfo(KeyInterop.VirtualKeyFromKey(xKey), c, ControlKeyStates.NumLockOn, keyDown);
		//}
		[DllImport("user32.dll")]
		public static extern int ToAsciiEx(uint uVirtKey, uint uScanCode, byte[] lpKeyState, [Out] StringBuilder lpChar, uint uFlags, IntPtr hkl);

		/// <summary>
		/// The ToAscii function translates the specified virtual-key code and keyboard state 
		/// to the corresponding character or characters. The function translates the code using 
		/// the input language and physical keyboard layout identified by the keyboard layout handle.
		/// </summary>
		/// <param name="uVirtKey">Specifies the virtual-key code to be translated.</param>
		/// <param name="uScanCode">Specifies the hardware scan code of the key to be translated. 
		/// The high-order bit of this value is set if the key is up (not pressed). </param>
		/// <param name="lpKeyState">Pointer to a 256-byte array that contains the current keyboard state. 
		/// Each element (byte) in the array contains the state of one key. If the high-order bit of a byte is set, 
		/// the key is down (pressed). The low bit, if set, indicates that the key is toggled on. In this function, 
		/// only the toggle bit of the CAPS LOCK key is relevant. The toggle state of the NUM LOCK and SCROLL LOCK keys is ignored.</param>
		/// <param name="lpChar">Pointer to the buffer that receives the translated character or characters.</param>
		/// <param name="uFlags">Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, [Out] StringBuilder lpChar, uint uFlags);
	}
}
