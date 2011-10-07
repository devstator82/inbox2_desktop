using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Inbox2.Framework
{
	public static class VisualTreeWalker
	{
		public static FrameworkElement FindName(string name, DependencyObject reference)
		{
			return FindName<FrameworkElement>(name, reference);
		}

		public static T FindName<T>(string name, DependencyObject reference) where T : FrameworkElement
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			if (reference == null)
			{
				throw new ArgumentNullException("reference");
			}

			return FindNameInternal<T>(name, reference);
		}

		static T FindNameInternal<T>(string name, DependencyObject reference) where T : FrameworkElement
		{
			foreach (DependencyObject obj in GetChildren(reference))
			{
				T elem = obj as T;

				if (elem != null && elem.Name == name)
				{
					return elem;
				}

				elem = FindNameInternal<T>(name, obj);

				if (elem != null)
				{
					return elem;
				}
			}

			return null;
		}

		static IEnumerable<DependencyObject> GetChildren(DependencyObject reference)
		{
			int childCount = VisualTreeHelper.GetChildrenCount(reference);

			for (int i = 0; i < childCount; i++)
			{
				yield return VisualTreeHelper.GetChild(reference, i);
			}
		}
	}
}
