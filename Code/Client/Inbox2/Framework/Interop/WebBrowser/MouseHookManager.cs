/*
 * Leslie Godwn: (leslie.godwin@gmail.com)
 * This class was taken from the "Olvio.Windows.Forms.Docking.SafeMouseHookManager" source.
 * A good exacple of how to use it can be found at (which is where I found it) : 
 * http://www.codeproject.com/useritems/MBtn2DblClick.asp
 * 
 * 2007/07/25
 * I made some minor changes and implemented the static class with helper methods.
 */


using System;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;

namespace Inbox2.Framework.Interop.WebBrowser
{
	public static class MouseHooks
	{
		public static MouseHookManager HookMouse(HandleRef handlerRef, IMouseHookUser user, bool enabled)
		{
			MouseHookManager result = new MouseHookManager(handlerRef.Handle, user);
			result.HookMessages = enabled;
			return result;
		}

		public static MouseHookManager HookMouse(IMouseHookUser user, bool enabled)
		{
			MouseHookManager result = new MouseHookManager(IntPtr.Zero, user);
			result.HookMessages = enabled;
			return result;
		}

		public class MouseHookManager : IDisposable
		{
			#region #  Externals  #

			[DllImport("user32")]
			private static extern IntPtr SetWindowsHookEx(int hookid, HookProc pfnhook, IntPtr hinst, int threadid);
			[DllImport("user32")]
			private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, int wparam, int lparam);
			[DllImport("user32")]
			private static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);
			[DllImport("kernel32")]
			private static extern int GetModuleHandle(string lpModuleName);
			[DllImport("user32")]
			public static extern int UnhookWindowsHookEx(IntPtr hHook);
			[DllImport("User32.dll")]
			public static extern int SendMessage(IntPtr hWnd,
			                                     int msg, int wParam, int lParam);

			#endregion

			#region #  Fields  #

			private IMouseHookUser user;
			private IntPtr handle;
			private bool hookDisable;
			private IntPtr MessageHookHandle;
			private GCHandle MessageHookRoot;
			internal int thisProcessID;

			#endregion

			#region #  Constructors  #

			public MouseHookManager(IntPtr handle, IMouseHookUser user)
			{
				this.thisProcessID = 0;
				this.hookDisable = false;
				this.handle = handle;
				this.user = user;
			}

			~MouseHookManager()
			{
				this.Dispose();
			}


			#endregion

			#region #  Methods  #

			private void HookMessage()
			{
				HookProc proc1;
				MessageHookObject obj1;
				MouseHookManager manager1;
				GC.KeepAlive(this);
				manager1 = this;
				Monitor.Enter(this);
				try
				{
					if (this.MessageHookHandle == IntPtr.Zero)
					{
						if (this.handle != IntPtr.Zero)
						{
							if (this.thisProcessID == 0)
							{
								GetWindowThreadProcessId(this.handle, ref this.thisProcessID);
							}
						}
						obj1 = new MessageHookObject(this);
						proc1 = new HookProc(obj1.Callback);
						this.MessageHookRoot = GCHandle.Alloc(proc1);
						this.MessageHookHandle = SetWindowsHookEx(14, proc1, new IntPtr(GetModuleHandle(null)), 0);
					}
				}
				finally
				{
					Monitor.Exit(manager1);
				}
			}

			public const int WM_LBUTTONDOWN = 0x0201;
			public const int WM_LBUTTONUP = 0x0202;
			public const int WM_LBUTTONDBLCLK = 0x0203;

			public const int WM_MBUTTONDOWN = 0x0207;
			public const int WM_MBUTTONUP = 0x0208;
			public const int WM_MBUTTONDBLCLK = 0x0209;

			private IntPtr MessageHookProc(int nCode, int wParam, int lParam)
			{
				if (nCode >= 0)
				{
					MSLLHOOKSTRUCT mousehookstruct_ll1 =
						((MSLLHOOKSTRUCT)Marshal.PtrToStructure(((IntPtr)lParam), typeof(MSLLHOOKSTRUCT)));
					if (mousehookstruct_ll1 != null)
					{
						if ((user != null) && user.SystemMouseHookProc(nCode, wParam, lParam, new Point(mousehookstruct_ll1.pt_x, mousehookstruct_ll1.pt_y), (int)mousehookstruct_ll1.dwExtraInfo))
						{
							return ((IntPtr)1);
						}
					}
				}
				GC.KeepAlive(this);
				return CallNextHookEx(this.MessageHookHandle, nCode, wParam, lParam);
			}

			private void UnHookMessage()
			{
				MouseHookManager manager1;
				GC.KeepAlive(this);
				manager1 = this;
				Monitor.Enter(this);
				try
				{
					if (this.MessageHookHandle != IntPtr.Zero)
					{
						UnhookWindowsHookEx(this.MessageHookHandle);
						this.MessageHookRoot.Free();
						this.MessageHookHandle = IntPtr.Zero;
					}
				}
				finally
				{
					Monitor.Exit(manager1);
				}
			}


			#endregion

			#region IDisposable Members

			public void Dispose()
			{
				this.UnHookMessage();
				GC.SuppressFinalize(this);
			}

			#endregion

			#region #  Properties  #

			public virtual bool DisableMessageHook
			{
				get { return this.hookDisable; }
				set
				{
					this.hookDisable = value;
					if (value)
					{
						this.UnHookMessage();

					}
				}
			}
			public virtual bool HookMessages
			{
				get
				{
					GC.KeepAlive(this);
					return (this.MessageHookHandle != IntPtr.Zero);
				}
				set
				{
					if (value && !this.hookDisable)
					{
						this.HookMessage();
						return;
					}
					this.UnHookMessage();
				}
			}


			#endregion

			#region #  Types  #

			internal class MessageHookObject
			{
				internal MouseHookManager reference;

				public MessageHookObject(MouseHookManager parent)
				{
					this.reference = parent;
				}

				public IntPtr Callback(int nCode, int wParam, int lParam)
				{
					IntPtr ptr1;
					MouseHookManager manager1;
					ptr1 = IntPtr.Zero;
					manager1 = this.reference;
					if (manager1 != null & wParam != WM_MBUTTONDOWN)
					{
						ptr1 = manager1.MessageHookProc(nCode, wParam, lParam);
					}
					return ptr1;
				}
			}

			public delegate IntPtr HookProc(int nCode, int wParam, int lParam);

			[StructLayout( LayoutKind.Sequential )]
			public class MSLLHOOKSTRUCT
			{
				public int pt_x;
				public int pt_y;
				public int mouseData;
				public int flags;
				public int time;
				public IntPtr dwExtraInfo;
			}

			[StructLayout(LayoutKind.Sequential)]
			internal class MOUSEHOOKSTRUCT_LL
			{
				public MOUSEHOOKSTRUCT_LL()
				{
					this.pt_x = 0;
					this.pt_y = 0;
					this.mouseData = ((long)0);
					this.flags = ((long)0);
					this.time = ((long)0);
					this.dwExtraInfo = 0;
				}
				public int dwExtraInfo;
				public ulong flags;
				public ulong mouseData;
				public int pt_x;
				public int pt_y;
				public ulong time;
			}
			#endregion
		}
	}
}


