using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;

namespace Inbox2.UI.Controls.Options
{
    /// <summary>
    /// Interaction logic for ViewOptionsControl.xaml
    /// </summary>
    public partial class ViewOptionsControl : UserControl
    {
    	private bool resetLayout;

        public ViewOptionsControl()
        {
            InitializeComponent();

        	DataContext = this;
        }

		public bool TrySave()
		{
			var config = SettingsManager.ClientSettings.AppConfiguration;

			switch (ShowPreviewPaneComboBox.SelectedIndex)
			{
				case 0: config.PreviewPaneLocation = PreviewPaneLocation.Hidden; break;
				case 1: config.PreviewPaneLocation = PreviewPaneLocation.Right; break;
				case 2: config.PreviewPaneLocation = PreviewPaneLocation.Bottom; break;
			}

			config.RollUpConversations = RollUpConversationsCheckBox.IsChecked ?? false;
			config.ShowProfileBalloons = ShowProfileBalloonsCheckBox.IsChecked ?? false;
			config.ShowStreamColumn = ShowStreamColumnCheckBox.IsChecked ?? false;
			config.MinimizeToTray = MinimizeToTrayCheckBox.IsChecked ?? false;

			if (resetLayout)
			{
				ClientState.Current.Context.DeleteSetting("/Settings/Overview/FoldersViewWidth");
				ClientState.Current.Context.DeleteSetting("/Settings/Overview/PreviewPaneWidth");
				ClientState.Current.Context.DeleteSetting("/Settings/Overview/PreviewPaneHeight");
				ClientState.Current.Context.DeleteSetting("/Settings/Overview/ProfileColumnWidth");
				ClientState.Current.Context.DeleteSetting("/Settings/Overview/StreamColumnWidth");
			}

			SettingsManager.Save();			

			return true;
		}

    	void ViewOptionsControl_OnLoaded(object sender, RoutedEventArgs e)
    	{
			var config = SettingsManager.ClientSettings.AppConfiguration;

			switch (config.PreviewPaneLocation)
			{
				case PreviewPaneLocation.Hidden:
					ShowPreviewPaneComboBox.SelectedIndex = 0;
					break;

				case PreviewPaneLocation.Right:
					ShowPreviewPaneComboBox.SelectedIndex = 1;
					break;

				case PreviewPaneLocation.Bottom:
					ShowPreviewPaneComboBox.SelectedIndex = 2;
					break;
			}

    		RollUpConversationsCheckBox.IsChecked = config.RollUpConversations;
			ShowProfileBalloonsCheckBox.IsChecked = config.ShowProfileBalloons;
    		ShowStreamColumnCheckBox.IsChecked = config.ShowStreamColumn;
    		MinimizeToTrayCheckBox.IsChecked = config.MinimizeToTray;
    	}

    	void ResetLayout_Click(object sender, RoutedEventArgs e)
    	{
    		if (Inbox2MessageBox.Show(Strings.ThisWillResetLayout, Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.Yes)
    		{
    			resetLayout = true;
    		}
    	}

    	void ResetDialogs_Click(object sender, RoutedEventArgs e)
    	{
			if (Inbox2MessageBox.Show(Strings.ThisWillResetDialogs, Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.Yes)
			{
				ClientState.Current.Context.DeleteSetting("/Settings/Dialogs/DeleteConversation");
			}
    	}
    }
}
