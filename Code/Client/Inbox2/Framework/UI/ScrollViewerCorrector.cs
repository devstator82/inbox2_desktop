using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inbox2.Framework.UI
{
	/// <summary>
	/// From: http://serialseb.blogspot.com/2007/09/wpf-tips-6-preventing-scrollviewer-from.html
	/// </summary>
	public class ScrollViewerCorrector
	{
		public static readonly DependencyProperty FixScrollingProperty =
			DependencyProperty.RegisterAttached("FixScrolling", typeof(bool), typeof(ScrollViewerCorrector), new FrameworkPropertyMetadata(false, OnFixScrollingPropertyChanged));

		public static bool GetFixScrolling(DependencyObject obj)
		{
			return (bool)obj.GetValue(FixScrollingProperty);
		}

		public static void SetFixScrolling(DependencyObject obj, bool value)
		{
			obj.SetValue(FixScrollingProperty, value);
		}

		public static void OnFixScrollingPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer viewer = sender as ScrollViewer;

			if (viewer == null)
				throw new ArgumentException("The dependency property can only be attached to a ScrollViewer", "sender");

			if ((bool)e.NewValue == true)
				viewer.PreviewMouseWheel += HandlePreviewMouseWheel;
			else if ((bool)e.NewValue == false)
				viewer.PreviewMouseWheel -= HandlePreviewMouseWheel;
		}

		private static List<MouseWheelEventArgs> _reentrantList = new List<MouseWheelEventArgs>();

		private static void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			var scrollControl = sender as ScrollViewer;

			if (!e.Handled && sender != null && !_reentrantList.Contains(e))
			{
				var previewEventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
				{
					RoutedEvent = UIElement.PreviewMouseWheelEvent,
					Source = sender
				};

				var originalSource = e.OriginalSource as UIElement;

				if (originalSource != null)
				{
					_reentrantList.Add(previewEventArg);

					originalSource.RaiseEvent(previewEventArg);

					_reentrantList.Remove(previewEventArg);

					// at this point if no one else handled the event in our children, we do our job

					if (!previewEventArg.Handled && ((e.Delta > 0 && scrollControl.VerticalOffset == 0)
						|| (e.Delta <= 0 && scrollControl.VerticalOffset >= scrollControl.ExtentHeight - scrollControl.ViewportHeight)))
					{
						e.Handled = true;

						var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);

						eventArg.RoutedEvent = UIElement.MouseWheelEvent;
						eventArg.Source = sender;

						var parent = (UIElement)((FrameworkElement)sender).Parent;
						parent.RaiseEvent(eventArg);
					}
				}
			}
		}
	}
}
