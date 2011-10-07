using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Framework.UI.DragDrop;

namespace Inbox2.Framework.UI
{
	public static class ControlDragHelper
	{
		// Using a DependencyProperty as the backing store for IsDragEnabled.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsDragEnabledProperty =
			DependencyProperty.RegisterAttached("IsDragEnabled", typeof(bool), typeof(ControlDragHelper), new PropertyMetadata(false, Control_IsDragEnabledChanged));

		public static bool GetIsDragEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsDragEnabledProperty);
		}

		public static void SetIsDragEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsDragEnabledProperty, value);
		}

		static void Control_IsDragEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var element = (UIElement)sender;

			if ((bool)e.NewValue)
			{
				IDataDropObjectProvider dataProvider;

				if (element is ListView)
				{
					dataProvider = new ListViewDragDropDataProvider(element as ListView);					
				}
				else if (element is Control)
				{
					dataProvider = new GenericDragDropHelper(element as Control);
				}
				else
				{
					throw new ApplicationException(String.Format("Unsupported drag drop type {0}", element.GetType()));
				}

				DragHelper dragHelper = new DragHelper(element, dataProvider, null);
			}
		}
	}
}
