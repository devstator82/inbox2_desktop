using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework.Interfaces;

namespace Inbox2.Framework.UI
{
	public static class FocusHelper
	{
		public static void Focus(UIElement element)
		{
			if (element is IFocusChild)
			{
				Focus(((IFocusChild)element).FocusElement);

				return;
			}

			//Focus in a callback to run on another thread, ensuring the main UI thread is initialized by the
			//time focus is set
			ThreadPool.QueueUserWorkItem(delegate(Object foo)
			{
				UIElement elem = (UIElement)foo;
				elem.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Input,
					(Action)delegate
					{
						elem.Focus();
						Keyboard.Focus(elem);

						if (elem is ListView)
						{
							// Listview is a special case and we need to set focus
							// to the selected item and not the listview itself
							var lv = (ListView)elem;
							if (lv.Items.Count > 0)
							{
								var index = lv.SelectedIndex;

								if (index < 0)
									index = 0;

								var item = lv.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;

								if (item != null)
									item.Focus();
							}
						}
					});
			}, element);
		}

		public static void FocusFirstResponder(this FrameworkElement root)
		{
			LogicalTreeWalker.Walk(root, delegate(UIElement element)
			{
				if (Responder.GetIsFirstResponder(element))
				{
					Focus(element);
				}
			});
		}
	}
}