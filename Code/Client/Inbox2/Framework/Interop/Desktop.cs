using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Inbox2.Platform.Logging;
using SHDocVw;
using Shell32;

namespace Inbox2.Framework.Interop
{
	public static class Desktop
	{
		private static IntPtr progmanHandle;
		private static IntPtr listviewHandle;

		public static IntPtr GetVisibleWindowAt(System.Windows.Point location)
		{
			return WinApi.WindowFromPoint(new POINT((int)location.X, (int)location.Y));
		}

		public static void RefreshHandles()
		{
			progmanHandle = WinApi.FindWindow("Progman", "Program Manager");

			IntPtr vHandle = WinApi.FindWindowEx(progmanHandle, IntPtr.Zero, "SHELLDLL_DefView", null);

			listviewHandle = WinApi.FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", "FolderView");

			Logger.Debug("Program Manager handle is {0}", LogSource.Interop, progmanHandle);
			Logger.Debug("SysListView32 handle is {0}", LogSource.Interop, listviewHandle);			
		}

		public static List<SelectedItem> GetSelectedItems(IntPtr windowHandle)
		{
			var results = new List<SelectedItem>();

			IntPtr handle = WinApi.GetForegroundWindow();

			if (handle == progmanHandle || handle == windowHandle)
			{
				Logger.Debug("Desktop listview is active", LogSource.Interop);

				// Desktop has focus
				foreach (var file in GetSelectedItemsOnDesktop())
					results.Add(file);
			}
			else
			{
				Logger.Debug("Desktop listview was not active, expected handle for desktop {0} or for window {1}, actual handle {2}", LogSource.Interop, progmanHandle, windowHandle, handle);

				// Other window, try to determine if is a shell window
				Shell shell = new Shell();
				ShellWindows windows = (ShellWindows)shell.Windows();

				foreach (InternetExplorer window in windows)
				{
					string processName = Path.GetFileNameWithoutExtension(window.FullName).ToLower();

					if (processName.Equals("iexplore"))
					{
						if ((int)handle == window.HWND)
						{
							Logger.Debug("Active window was internet explorer instance {0}", LogSource.Interop, handle);
							Logger.Debug("   Adding {0}", LogSource.Interop, window.LocationURL);

							results.Add(new BrowserSelectedUrl() { Url = window.LocationURL });
						}
					}

					else if (processName.Equals("explorer"))
					{
						if ((int)handle == window.HWND)
						{
							Logger.Debug("Active window was windows explorer instance {0}", LogSource.Interop, handle);

							ShellFolderView view = (ShellFolderView)window.Document;
							FolderItems items = view.SelectedItems();

							foreach (FolderItem item in items)
							{
								Logger.Debug("   Adding path {0}", LogSource.Interop, item.Path);

								results.Add(new ExplorerSelectedFile() { Location = Path.GetDirectoryName(item.Path), Filename = Path.GetFileName(item.Path) });
							}
						}
					}
				}				
			}

			return results;
		}

		static IEnumerable<ExplorerSelectedFile> GetSelectedItemsOnDesktop()
		{
			//Get total count of the icons on the desktop
			int vItemCount = WinApi.SendMessage(listviewHandle, WinApi.LVM_GETITEMCOUNT, 0, 0);

			uint vProcessId;
			WinApi.GetWindowThreadProcessId(listviewHandle, out vProcessId);

			IntPtr vProcess = WinApi.OpenProcess(WinApi.PROCESS_VM_OPERATION | WinApi.PROCESS_VM_READ |
										  WinApi.PROCESS_VM_WRITE, false, vProcessId);

			IntPtr vPointer = WinApi.VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
											 WinApi.MEM_RESERVE | WinApi.MEM_COMMIT, WinApi.PAGE_READWRITE);


			var selectedItems = GetListViewSelectedItems(listviewHandle);

			try
			{
				string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

				for (int j = 0; j < vItemCount; j++)
				{
					byte[] vBuffer = new byte[256];
					LVITEM[] vItem = new LVITEM[1];
					vItem[0].mask = WinApi.LVIF_TEXT;
					vItem[0].iItem = j;
					vItem[0].iSubItem = 0;
					vItem[0].cchTextMax = vBuffer.Length;
					vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));

					uint vNumberOfBytesRead = 0;

					WinApi.WriteProcessMemory(vProcess, vPointer,
									   Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
									   Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);

					WinApi.SendMessage(listviewHandle, WinApi.LVM_GETITEMW, j, vPointer.ToInt32());

					WinApi.ReadProcessMemory(vProcess,
									  (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
									  Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
									  vBuffer.Length, ref vNumberOfBytesRead);

					string vText = Encoding.Unicode.GetString(vBuffer, 0,
															  (int)vNumberOfBytesRead);

					string iconName = vText;

					int index = vText.IndexOf('\0');
					if (index > -1)
						iconName = vText.Substring(0, index);

					//Get icon location
					WinApi.SendMessage(listviewHandle, WinApi.LVM_GETITEMPOSITION, j, vPointer.ToInt32());

					Point[] vPoint = new Point[1];
					WinApi.ReadProcessMemory(vProcess, vPointer,
									  Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0),
									  Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead);

					
					
					Point iconLocation = vPoint[0];
					bool selected = selectedItems.Contains(j);

					if (selected)
						yield return new ExplorerSelectedFile
						           	{
										Filename = iconName, 
										Location = desktopPath, 
										Coordinates = iconLocation
						           	};
				}
			}
			finally
			{
				WinApi.VirtualFreeEx(vProcess, vPointer, 0, WinApi.MEM_RELEASE);
				WinApi.CloseHandle(vProcess);
			}
		}

		static List<int> GetListViewSelectedItems(IntPtr hWnd)
		{
			List<int> list = new List<int>();

			int pos = -1;

			pos = WinApi.SendMessage(hWnd, WinApi.LVM_GETNEXTITEM, pos, WinApi.LVIS_SELECTED);

			while (pos > 0)
			{
				list.Add(pos);

				pos = WinApi.SendMessage(hWnd, WinApi.LVM_GETNEXTITEM, pos, WinApi.LVIS_SELECTED);
			}

			return list;
		}
	}
}