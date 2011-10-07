using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Plugins.SharedControls;
using Inbox2.Framework.Stats;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for NewItemDock.xaml
	/// </summary>
	public partial class NewItemDock : UserControl
	{
		private readonly MessageEditControl control;

		public NewItemDock(MessageEditControl control)
		{
			this.control = control;

			InitializeComponent();

			DataContext = this;			
		}		

		void WaitingForCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Add waiting for to new message");

			control.AddWaitingFor = true;
		}

		void WaitingForCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Remove waiting from new message");

			control.AddWaitingFor = false;
		}

		void SaveDraft_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Save draft");

			control.SaveDraft();
		}

		void Send_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Send message");

			control.SendMessage();
			control.CloseEditor();
		}

		void Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Cancel message");

			control.TryCancel();
		}

		void Send_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = control.To.Count > 0 && control.HasMessageContent(false);
		}

		void SaveDraft_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = control.HasMessageContent(false);
		}		

		void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}		
	}
}
