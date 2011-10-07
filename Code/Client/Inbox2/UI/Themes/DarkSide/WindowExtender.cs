using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interop;

namespace Inbox2.UI.Themes.DarkSide
{
	public static class WindowExtender
	{	
		#region Dependency properties

		public static readonly DependencyProperty HasRoundedCornersProperty =
			DependencyProperty.RegisterAttached("HasRoundedCorners", typeof(bool), typeof(WindowExtender), new UIPropertyMetadata(false, WindowExtender_HasRoundedCorners));

		public static bool GetHasRoundedCorners(DependencyObject obj)
		{
			return (bool)obj.GetValue(HasRoundedCornersProperty);
		}

		public static void SetHasRoundedCorners(DependencyObject obj, bool value)
		{
			obj.SetValue(HasRoundedCornersProperty, value);
		}

		#endregion

		public static void DragSize(this System.Windows.Window source, SizingAction direction)
		{
			IntPtr handle = new WindowInteropHelper(source).Handle;

			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				WinApi.SendMessage(handle, WinApi.WM_SYSCOMMAND, (int)WinApi.SC_SIZE + (int)direction, 0);
				WinApi.SendMessage(handle, 514, 0, 0);
			}
		}

		static void WindowExtender_HasRoundedCorners(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as System.Windows.Window).SourceInitialized += WindowExtender_SourceInitialized;
			(d as System.Windows.Window).SizeChanged += WindowExtender_SizeChanged;
		}

		static void WindowExtender_SourceInitialized(object sender, EventArgs e)
		{
			var window = (System.Windows.Window)sender;			
		
			UpdateWindowCorners(window);
		}

		static void WindowExtender_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdateWindowCorners(sender as System.Windows.Window);
		}

		static void UpdateWindowCorners(System.Windows.Window window)
		{
			if (GetHasRoundedCorners(window))
			{
				IntPtr handle = new WindowInteropHelper(window).Handle;
				HwndSource source = HwndSource.FromHwnd(handle);

				if (source != null)
				{
					Point deviceSize = source.CompositionTarget.TransformToDevice.Transform(
						new Point(window.ActualWidth, window.ActualHeight));

					IntPtr region = WinApi.CreateRoundRectRgn(0, 0, (int) deviceSize.X + 1, (int) deviceSize.Y + 1, 8, 8);

					WinApi.SetWindowRgn(handle, region, true);
				}
			}
		}		
	}
}
