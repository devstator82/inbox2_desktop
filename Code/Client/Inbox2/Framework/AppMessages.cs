using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework
{
	public class AppMessage
	{
		public long SourceChannelId { get; set; }

		public long EntityId { get; set; }

		public EntityType EntityType { get; set; }

		public string Message { get; private set; }

		public ChannelConfiguration SourceChannel
		{
			get
			{
				var channel = ChannelsManager.GetChannelObject(SourceChannelId);

				return channel == null ? null : channel.Configuration;
			}
		}

		public AppMessage(string message)
		{
			Message = message;
		}

		public override string ToString()
		{
			return Message;
		}
	}

	public class AppMessages : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> OnSuccess;
		public event EventHandler<EventArgs> OnError;

		private AppMessage success;
		private readonly SpilledObservableCollection<AppMessage> errors;

		public AppMessage Success
		{
			get { return success; }
			set
			{
				success = value;

				OnPropertyChanged("Success");

				if (OnSuccess != null)
				{
					Thread.CurrentThread.RaiseUIEventHandler(this, OnSuccess, EventArgs.Empty);
				}
			}
		}

		public AppMessage LastError
		{
			get { return errors.LastOrDefault(); }
		}

		public SpilledObservableCollection<AppMessage> Errors
		{
			get { return errors; }
		}

		public AppMessages()
		{
			errors = new SpilledObservableCollection<AppMessage>(5);
			errors.CollectionChanged += ErrorsCollectionChanged;
		}		

		/// <summary>
		/// Called when [propertyproperty changed].
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}	
	
		void ErrorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				OnPropertyChanged("LastError");

				if (OnError != null)
				{
					Thread.CurrentThread.RaiseUIEventHandler(this, OnError, EventArgs.Empty);
				}
			}
		}
	}

	internal static class ExtensionHelpers
	{
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

		/// <summary>
		/// Executes the given action on the UI thread associated with the current dispatcher.
		/// </summary>
		/// <param name="thread">The thread.</param>
		/// <param name="target">The target.</param>
		public static void ExecuteOnUIThread<T>(this Thread thread, Action<T> target, T obj)
		{
			if (Application.Current == null || Application.Current.Dispatcher.CheckAccess())
			{
				target(obj);

				return;
			}

			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, target);
		}

		/// <summary>
		/// Executes the given action on the UI thread associated with the current dispatcher.
		/// </summary>
		/// <param name="thread">The thread.</param>
		/// <param name="target">The target.</param>
		public static void ExecuteOnUIThread<T1, T2>(this Thread thread, Action<T1, T2> target, T1 obj1, T2 obj2)
		{
			if (Application.Current == null || Application.Current.Dispatcher.CheckAccess())
			{
				target(obj1, obj2);

				return;
			}

			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, target);
		}
	}
}
