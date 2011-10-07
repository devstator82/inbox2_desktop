using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Inbox2.UI.Extensions
{
	public static class CollectionExtensions
	{
		public static T FindElementOfType<T>(this UIElementCollection source) where T : UIElement
		{
			foreach (var o in source)
			{
				if (o is T) return (T)o;
			}

			return default(T);
		}

		public static void RemoveElementOfType<T>(this UIElementCollection source) where T : UIElement
		{
			T ctrl = FindElementOfType<T>(source);

			if (ctrl != null)
			{
				source.Remove(ctrl);
			}
		}
	}
}