using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Inbox2.Framework.UI.Controls
{
	public static class Helper
	{		
		public static T FindAncestor<T>(this DependencyObject child) where T : DependencyObject
		{
			//get parent item
			DependencyObject parentObject = VisualTreeHelper.GetParent(child);

			//we’ve reached the end of the tree
			if (parentObject == null) return null;

			//check if the parent matches the type we’re looking for
			T parent = parentObject as T;

			if (parent != null)
			{
				return parent;
			}
			else
			{
				//use recursion to proceed with next level
				return FindAncestor<T>(parentObject);
			}
		}

		/// <summary>
		/// Find the Panel for the TabControl
		/// </summary>
		public static VirtualizingTabPanel FindVirtualizingTabPanel(Visual visual)
		{
			if (visual == null)
				return null;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
			{
				Visual child = VisualTreeHelper.GetChild(visual, i) as Visual;

				if (child != null)
				{
					if (child is VirtualizingTabPanel)
					{
						object temp = child;
						return (VirtualizingTabPanel)temp;
					}

					VirtualizingTabPanel panel = FindVirtualizingTabPanel(child);
					if (panel != null)
					{
						object temp = panel;
						return (VirtualizingTabPanel)temp; // return the panel up the call stack
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Clone an element
		/// </summary>
		/// <param name="elementToClone"></param>
		/// <returns></returns>
		public static object CloneElement(this object elementToClone)
		{
			if (elementToClone != null)
			{
				string xaml = XamlWriter.Save(elementToClone);
				return XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
			}
			else
			{
				return String.Empty;
			}
		}
	}
}
