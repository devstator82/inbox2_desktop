using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.VirtualMailBox.View;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for MessageActionsBox.xaml
	/// </summary>
	public partial class MessageActionsBox : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<EventArgs> AfterAction;

		public Message SelectedMessage { get; private set; }

		public MessageActionsBox()
		{
			InitializeComponent();

			DataContext = this;
		}

		public void Show(Message message)
		{
			SelectedMessage = message;

			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs("SelectedMessage"));
		}
		
		void RemoveTodo_Click(object sender, RoutedEventArgs e)
		{
			SelectedMessage.RemoveLabel(SelectedMessage.LabelsList.FirstOrDefault(l => l.LabelType == LabelType.Todo));

            OnAfterAction();
		}

		void RemoveWaitingFor_Click(object sender, RoutedEventArgs e)
		{
			SelectedMessage.RemoveLabel(SelectedMessage.LabelsList.FirstOrDefault(l => l.LabelType == LabelType.WaitingFor));

            OnAfterAction();
		}

		void RemoveSomeday_Click(object sender, RoutedEventArgs e)
		{
			SelectedMessage.RemoveLabel(SelectedMessage.LabelsList.FirstOrDefault(l => l.LabelType == LabelType.Someday));

		    OnAfterAction();
		}

        void ViewTodoList_Click(object sender, RoutedEventArgs e)
        {
            EventBroker.Publish(AppEvents.View, ActivityView.Todo);
        }

        void ViewWaitingForList_Click(object sender, RoutedEventArgs e)
        {
            EventBroker.Publish(AppEvents.View, ActivityView.WaitingFor);
        }

        void ViewReadSomedayList_Click(object sender, RoutedEventArgs e)
        {
            EventBroker.Publish(AppEvents.View, ActivityView.Someday);
        }

        void OnAfterAction()
        {
            if (AfterAction != null)
            {
                AfterAction(this, EventArgs.Empty);
            }
        }
	}
}
