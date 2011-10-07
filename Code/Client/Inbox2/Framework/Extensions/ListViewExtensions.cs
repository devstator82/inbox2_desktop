using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using Inbox2.Framework.UI;

namespace Inbox2.Framework.Extensions
{
	public static class ListViewExtensions
	{
		public static ScrollViewer GetScrollViewer(this ListView source)
		{
			var firstChild = VisualTreeHelper.GetChild(source, 0);
			var secondChild = VisualTreeHelper.GetChild(firstChild, 0);

			return secondChild as ScrollViewer;
		}
	}
}
