using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for Column.xaml
	/// </summary>
	public partial class Column : UserControl, IFocusChild
	{
		#region Fields

		public event EventHandler<EventArgs> SelectedMessageChanged;

		#endregion

		#region Properties

		public ConversationsPlugin Plugin
		{
			get { return PluginsManager.Current.GetPlugin<ConversationsPlugin>(); }
		}

		public ConversationsState State
		{
			get { return (ConversationsState)Plugin.State; }
		}

		public UIElement FocusElement
		{
			get { return MessagesListView; }
		}

		#endregion

		#region Constructors

		public Column()
		{
			InitializeComponent();

			DataContext = this;
		}

		#endregion

		#region Command handlers

		void New_Executed(object sender, ExecutedRoutedEventArgs e)
		{
		}

		void View_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			EventBroker.Publish(AppEvents.View, (Message)e.Parameter);
		}

		void Reply_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Reply();
		}

		void ReplyAll_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.ReplyAll();
		}

		void Forward_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Forward();
		}

		void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Delete();
		}

		void Star_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			State.Star();
		}		

		void MarkRead_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.MarkRead();
		}

		void MarkUnread_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.MarkUnread();
		}

		void View_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null;
		}		

		void Reply_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanReply;
		}

		void ReplyAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanReplyAll;
		}

		void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanForward;
		}

		void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanDelete;
		}

		void Star_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && State.CanStar;
		}

		void MarkRead_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanMarkRead;
		}

		void MarkUnread_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanMarkUnread;
		}

		#endregion

		#region Methods

		public void OverrideViewSource(CollectionViewSource newSource)
		{
			MessagesListView.ItemsSource = newSource == null ? null : newSource.View;
		}

		void SelectMessages(IEnumerable<Message> messages)
		{
			State.SelectedMessages.Replace(messages);

			OnSelectedMessageChanged();
		}

		void OnSelectedMessageChanged()
		{
			if (SelectedMessageChanged != null)
			{
				SelectedMessageChanged(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Event handlers

		void MessagesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;

			SelectMessages(MessagesListView.SelectedItems.Cast<Message>());
		}

		void MessagesListView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				State.Delete();
			}
			else if (e.Key == Key.Enter)
			{
				State.View();
			}
		}

		void MessagesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var message = (e.OriginalSource as DependencyObject)
					.FindListViewItem<Message>(MessagesListView.ItemContainerGenerator);

			if (message != null)
				EventBroker.Publish(AppEvents.View, message);
		}

		#endregion
	}
}