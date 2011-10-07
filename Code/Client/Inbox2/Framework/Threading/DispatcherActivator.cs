using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Framework.Threading
{
	public static class DispatcherActivator<T> where T : new()
	{
		public static T Create()
		{
			if (Application.Current == null || Application.Current.Dispatcher.CheckAccess())
			{
				return new T();				
			}
			else
			{
				T instance = default(T);

				Application.Current.Dispatcher.Invoke((Action)delegate { instance = new T(); }, DispatcherPriority.Normal);

				return instance;
			}			
		}

		public static T DuckCopy(object source)
		{
			if (Application.Current == null || Application.Current.Dispatcher.CheckAccess())
			{
				return new T();				
			}
			else
			{
				T instance = default(T);

				Application.Current.Dispatcher.Invoke((Action)delegate { instance = source.DuckCopy<T>(); }, DispatcherPriority.Normal);

				return instance;
			}			
		}
	}
}
