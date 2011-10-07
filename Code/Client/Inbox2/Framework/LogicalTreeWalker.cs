using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Inbox2.Framework
{
	public static class LogicalTreeWalker
	{
		public static void Walk<T>(FrameworkElement source, Action<T> action)
		{
			WalkTree(source, delegate(object s)
        	{
				if (s is T) action((T)s);
        	});
		}

		public static T FindName<T>(FrameworkElement source, string name) where T : FrameworkElement
		{
			T result = default(T);

			Walk(source, delegate(T instance) { if (instance.Name == name) result = instance; });

			return result;
		}

		static void WalkTree(FrameworkElement source, Action<object> action)
		{
			foreach (object child in LogicalTreeHelper.GetChildren(source))
			{
				if (child is FrameworkElement)
				{
					action(child);

					WalkTree((FrameworkElement)child, action);
				}
			}			
		}
	}
}
