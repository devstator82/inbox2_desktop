using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.UI.Controls;
using TabItem = System.Windows.Controls.TabItem;

namespace Inbox2.UI.Controls.Options
{
	/// <summary>
	/// Interaction logic for OptionsControl.xaml
	/// </summary>
	public partial class OptionsControl : UserControl
	{
		private readonly List<TabItem> visited;

		public OptionsControl()
		{
			visited = new List<TabItem>();

			InitializeComponent();			
		}

		/// <summary>
		/// Handles the Click event of the SaveButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			bool isValid = true;

			if (isValid && visited.Contains(ProfileTab)) isValid = ProfileOptionsControl.TrySave();

			if (isValid && visited.Contains(MessagesTab)) isValid = MessagesOptionsControl.TrySave();

			if (isValid && visited.Contains(ViewTab)) isValid = ViewOptionsControl.TrySave();

			if (isValid && visited.Contains(AdvancedTab)) isValid = AdvancedOptionsControl.TrySave();

			if (isValid && visited.Contains(AccountsTab)) isValid = AccountOptionsControl.TrySave();			

			// We're done, now close the window
			if (isValid)
			{
				// refreshes all views
				EventBroker.Publish(AppEvents.ReceiveFinished);
				EventBroker.Publish(AppEvents.UpdateCheckFinished);
				EventBroker.Publish(AppEvents.SyncFinished);
				EventBroker.Publish(AppEvents.SyncContactsFinished);
				EventBroker.Publish(AppEvents.SyncStatusUpdatesFinished);

				EventBroker.Publish(AppEvents.RebuildOverview);
				EventBroker.Publish(AppEvents.RebuildToolbar);

				ClientState.Current.ViewController.HidePopup();
			}
		}

		void TabControl_TabItemSelected(object sender, TabItemEventArgs e)
		{
			if (!visited.Contains(e.TabItem))
				visited.Add(e.TabItem);
		}		

		/// <summary>
		/// Handles the Click event of the CancelButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			ClientState.Current.ViewController.HidePopup();
		}		
	}
}
