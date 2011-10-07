using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Inbox2.Framework.Extensions
{
	public static class ThreadExtensions
	{
		/// <summary>
		/// Raises the sepcified property changed event on the UI thread (if needed).
		/// </summary>
		/// <param name="thread">The thread.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="eventHandler">The event handler.</param>
		public static void RaiseUIPropertyChanged(this Thread thread, object sender, string propertyName, PropertyChangedEventHandler eventHandler)
		{
			if (Application.Current == null || Application.Current.Dispatcher.CheckAccess())
			{
				if (eventHandler != null)
				{
					eventHandler(sender, new PropertyChangedEventArgs(propertyName));
				}

				return;
			}

			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
				(Action)delegate
				{
					if (eventHandler != null)
					{
						eventHandler(sender, new PropertyChangedEventArgs(propertyName));
					}
				});
		}

		/// <summary>
		/// Raises the sepcified event on the UI thread (if needed).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="thread">The thread.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="eventHandler">The event handler.</param>
		public static void RaiseUIEventHandler<T>(this Thread thread, object sender, EventHandler<T> eventHandler) where T : EventArgs, new()
		{
			RaiseUIEventHandler(thread, sender, eventHandler, new T());
		}

		/// <summary>
		/// Raises the sepcified event on the UI thread (if needed).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="thread">The thread.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="eventHandler">The event handler.</param>
		/// <param name="eventArgs">The event args.</param>
		public static void RaiseUIEventHandler<T>(this Thread thread, object sender, EventHandler<T> eventHandler, T eventArgs) where T : EventArgs
		{
			if (Application.Current == null || Application.Current.Dispatcher.CheckAccess())
			{
				if (eventHandler != null)
				{
					eventHandler(sender, eventArgs);
				}

				return;
			}

			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
				(Action)delegate
				{
					if (eventHandler != null)
					{
						eventHandler(sender, eventArgs);
					}
				});
		}

		/// <summary>
		/// Executes the given action on the UI thread associated with the current dispatcher.
		/// </summary>
		/// <param name="thread">The thread.</param>
		/// <param name="target">The target.</param>
		public static void ExecuteOnUIThread(this Thread thread, Action target)
		{
			if (Application.Current == null || Application.Current.Dispatcher.CheckAccess())
			{
				target();

				return;
			}

			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, target);
		}
	}
}
