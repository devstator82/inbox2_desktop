using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Inbox2.Framework.Extensions
{
	/// <summary>
	/// Borrowed from http://www.hardcodet.net/2008/02/find-wpf-parent
	/// </summary>
	public static class DependencyObjectExtensions
	{
		/// <summary>
		/// Finds a parent of a given item on the visual tree.
		/// </summary>
		/// <typeparam name="T">The type of the queried item.</typeparam>
		/// <param name="child">A direct or indirect child of the
		/// queried item.</param>
		/// <returns>The first parent item that matches the submitted
		/// type parameter. If not matching item can be found, a null
		/// reference is being returned.</returns>
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
		/// Tries to locate a given item within the visual tree,
		/// starting with the dependency object at a given position. 
		/// </summary>
		/// <typeparam name="T">The type of the element to be found
		/// on the visual tree of the element at the given location.</typeparam>
		/// <param name="reference">The main element which is used to perform
		/// hit testing.</param>
		/// <param name="point">The position to be evaluated on the origin.</param>
		public static T FindFromPoint<T>(this UIElement reference, Point point)
		  where T : DependencyObject
		{
			DependencyObject element = reference.InputHitTest(point)
										 as DependencyObject;
			if (element == null) return null;
			else if (element is T) return (T)element;
			else return FindAncestor<T>(element);
		}

		public static IEnumerable<T> GetChildrenOf<T>(this DependencyObject obj)
		{
			foreach (object child in LogicalTreeHelper.GetChildren(obj))
			{
				if (child is T)
					yield return (T) child;
				else if (child is DependencyObject)
					foreach (var subChild in GetChildrenOf<T>((DependencyObject)child))
						yield return subChild;
			}
		}

		/// <summary>
		/// Finds the list view item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="generator">The generator.</param>
		/// <returns></returns>
		public static T FindListViewItem<T>(this DependencyObject source, ItemContainerGenerator generator)
		{
			DependencyObject dep = source;

			if (dep is Hyperlink)
				return FindListViewItem<T>((dep as Hyperlink).Parent, generator);

			if (dep is Run)
				return FindListViewItem<T>((dep as Run).Parent, generator);

			while ((dep != null) && !(dep is ListBoxItem))
			{
				dep = VisualTreeHelper.GetParent(dep);
			}

			if (dep == null)
				return default(T);

			ListBoxItem selectedItem = (ListBoxItem)dep;

			var document = (T)generator.ItemFromContainer(selectedItem);

			return document;
		}
	}
}
