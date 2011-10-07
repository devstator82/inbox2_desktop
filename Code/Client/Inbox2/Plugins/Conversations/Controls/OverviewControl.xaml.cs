using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Plugins.SharedControls;
using Inbox2.Framework.Stats;
using Inbox2.Framework.UI;
using Inbox2.Framework.UI.Controls;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.VirtualMailBox.View;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.Conversations.Helpers;
using Inbox2.Plugins.StatusUpdates.Controls;
using Label = Inbox2.Framework.VirtualMailBox.Entities.Label;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for OverviewControl.xaml
	/// </summary>
	[Export(typeof(IMainOverviewPlugin))]
	public partial class OverviewControl : UserControl, IMainOverviewPlugin
	{
	    #region Fields

	    private FoldersControl foldersControl;
	    private OverviewColumn statusUpdatesColumn;
	    private MessageDetailView messageDetailsView;
	    private StreamView streamView;

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

	    #endregion

	    #region Construction

	    public OverviewControl()
	    {
	        InitializeComponent();

	        DataContext = this;

	        EventBroker.Subscribe(AppEvents.RebuildOverview, () => Thread.CurrentThread.ExecuteOnUIThread(RebuildOverview));

	        EventBroker.Subscribe(AppEvents.ShuttingDown, () => Thread.CurrentThread.ExecuteOnUIThread(delegate
	            {
	                ClientState.Current.Context.SaveSetting("/Settings/Overview/FoldersViewWidth", foldersControl.Width);

	                if (SettingsManager.ClientSettings.AppConfiguration.PreviewPaneLocation == PreviewPaneLocation.Right)
	                    ClientState.Current.Context.SaveSetting("/Settings/Overview/PreviewPaneWidth", messageDetailsView.Width);

	                if (SettingsManager.ClientSettings.AppConfiguration.PreviewPaneLocation == PreviewPaneLocation.Bottom)
	                    ClientState.Current.Context.SaveSetting("/Settings/Overview/PreviewPaneHeight", messageDetailsView.Height);

	                if (SettingsManager.ClientSettings.AppConfiguration.ShowStreamColumn)
	                    ClientState.Current.Context.SaveSetting("/Settings/Overview/StreamColumnWidth", statusUpdatesColumn.Width);
	            }));

	        State.SelectionChanged += State_SelectionChanged;
	    }

	    #endregion

	    #region Methods

	    protected override void OnInitialized(EventArgs e)
	    {
	        base.OnInitialized(e);

	        RebuildOverview();
	    }

	    void RebuildOverview()
	    {
	        Root.Children.Clear();

	    	Message selected = null;

			if (messageDetailsView != null)
				selected = messageDetailsView.Message;

	        // These controls get rebuilt
	        statusUpdatesColumn = null;
	        messageDetailsView = null;

	        CreateFoldersView();
	        CreateStatusUpdatesColumn();
	        CreatePreviewPane();
	        CreateStreamView();

			if (selected != null && messageDetailsView != null)
				messageDetailsView.Show(selected);

            EventBroker.Publish(AppEvents.RequestFocus);
	    }

	    void CreateFoldersView()
	    {
	        var width = SettingsManager.SettingOrDefault<double>("/Settings/Overview/FoldersViewWidth", 150);

	        if (foldersControl == null)
	            foldersControl = new FoldersControl();

	        foldersControl.Width = width;

	        Root.Children.Add(foldersControl);

	        DockPanel.SetDock(foldersControl, Dock.Left);

	        CreateSplitter(Dock.Left);
	    }		

	    void CreateStatusUpdatesColumn()
	    {
	        if (SettingsManager.ClientSettings.AppConfiguration.ShowStreamColumn)
	        {
	            var width = SettingsManager.SettingOrDefault<double>("/Settings/Overview/StreamColumnWidth", 250);
	            statusUpdatesColumn = new OverviewColumn { Width = width };

	            Root.Children.Add(statusUpdatesColumn);

	            DockPanel.SetDock(statusUpdatesColumn, Dock.Right);

	            CreateSplitter(Dock.Right);
	        }
	    }

	    void CreatePreviewPane()
	    {
	        var location = SettingsManager.ClientSettings.AppConfiguration.PreviewPaneLocation;

	        if (location == PreviewPaneLocation.Hidden)
	            return;

	        messageDetailsView = new MessageDetailView();

	        Root.Children.Add(messageDetailsView);			

	        switch (location)
	        {
	            case PreviewPaneLocation.Right:
	                {
	                    messageDetailsView.Width = SettingsManager.SettingOrDefault<double>("/Settings/Overview/PreviewPaneWidth", 350);                        

	                    DockPanel.SetDock(messageDetailsView, Dock.Right);

	                    CreateSplitter(Dock.Right);

	                    break;
	                }
	            case PreviewPaneLocation.Bottom:
	                {
	                    messageDetailsView.Height = SettingsManager.SettingOrDefault<double>("/Settings/Overview/PreviewPaneHeight", 250);

	                    DockPanel.SetDock(messageDetailsView, Dock.Bottom);

	                    CreateSplitter(Dock.Bottom);

	                    break;
	                }
	            default:
	                return;
	        }

	        if (State.SelectedMessage != null)
	            messageDetailsView.Show(State.SelectedMessage);
	    }

	    void CreateStreamView()
	    {
	        // Will be the control that take sup all remaining space
	        if (streamView == null)
	            streamView = new StreamView();

	        Root.Children.Add(streamView);
	    }

	    void CreateSplitter(Dock dock)
	    {
	        var style = (Style)FindResource(dock == Dock.Left || dock == Dock.Right ? "VerticalBevelGrip" : "HorizontalBevelGrip");
	        var splitter = new DockPanelSplitter { Style = style, ProportionalResize = false };

	        Root.Children.Add(splitter);

	        DockPanel.SetDock(splitter, dock);
	    }

	    void State_SelectionChanged(object sender, EventArgs e)
	    {
	        if (State.SelectedMessage != null)
	        {
	            if (messageDetailsView != null)
	                messageDetailsView.Show(State.SelectedMessage);
	        }
	    }

	    #endregion

		#region Command handlers

	    #region Application commands

	    void New_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Compose message in stream");

	        ActionHelper.New((SourceAddress)e.Parameter);
	    }

	    void View_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("View message in stream");

	        if (e.Parameter != null)
	        {
	            State.SelectedMessages.Replace(new[] { (Message)e.Parameter });
				streamView.StreamListView.SelectedItem = e.Parameter;
	        }

	        State.View();
	    }

	    void Reply_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Reply in stream");

	        State.Reply();
	    }

	    void ReplyAll_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Reply all in stream");

	        if (e.Parameter != null)
	            State.SelectedMessages.Replace(new[] { (Message)e.Parameter });

	        State.ReplyAll();
	    }

	    void Forward_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Forward in stream");

	        if (e.Parameter != null)
	            State.SelectedMessages.Replace(new[] { (Message)e.Parameter });

	        State.Forward();
	    }

	    void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Delete");

	        streamView.DeleteStreamSelection();
	    }

	    void Star_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Star in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	        {
	            State.SelectedMessages.Replace(new[] { (Message)e.Parameter });

	            State.Star();
	        }
	    }

	    void MarkRead_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("MarkRead in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	            State.MarkRead();
	    }

	    void MarkUnread_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("MarkUnread in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	            State.MarkUnread();
	    }

	    void Archive_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Archive in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	        {
	            State.Archive();
	        }
	    }

	    void Unarchive_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Unarchive in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	        {
	            State.Unarchive();
	        }
	    }

	    void AddLabel_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Add label in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	        {
	            if (State.SelectedMessages.Count == 1)
	            {
                    var lvItem = (ListViewItem)streamView.StreamListView.ItemContainerGenerator.ContainerFromItem(State.SelectedMessage);
	                var editor = (LabelsEditorControl)VisualTreeWalker.FindName("LabelsEditor", lvItem);

	                editor.Visibility = Visibility.Visible;
                    FocusHelper.Focus(editor);
	            }
	            else
	            {
	                // Show modal labels adder
	                EventBroker.Publish(AppEvents.RequestAddLabels, State.SelectedMessages.ToList());
	            }
	        }
	    }

	    void Todo_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Make todo in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	        {
				State.AddLabel(new Label(LabelType.Todo));
	        }
	    }

	    void WaitingFor_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("WaitingFor in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	        {
				State.AddLabel(new Label(LabelType.WaitingFor));
	        }
	    }

	    void Someday_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Someday in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	        {
				State.AddLabel(new Label(LabelType.Someday));
	        }
	    }

	    void ClearAction_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Clear action in stream");

	        using (new ListViewIndexFix(streamView.StreamListView))
	        {
				State.ClearActions();
	        }
	    }

	    void OpenDocument_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Open document in stream");

	        EventBroker.Publish(AppEvents.View, (Document)e.Parameter);
	    }

	    void SaveDocument_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Save document in stream");

	        EventBroker.Publish(AppEvents.Save, (Document)e.Parameter);
	    }

	    #endregion

        #region Keyboard shortcuts

        void NewerConversation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Move to newer conversation (keyboard)");

            if (streamView.StreamListView.SelectedIndex < streamView.StreamListView.Items.Count -2)
            {
                streamView.StreamListView.SelectedIndex++;

                streamView.StreamListView.ScrollIntoView(streamView.StreamListView.SelectedItem);
            }
        }

        void OlderConversation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Move to older conversation (keyboard)");

            if (streamView.StreamListView.SelectedIndex > 0)
            {
                streamView.StreamListView.SelectedIndex--;

                streamView.StreamListView.ScrollIntoView(streamView.StreamListView.SelectedItem);
            }
        }

        void InlineReply_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Inline reply (keyboard)");

            if (messageDetailsView != null)
            {
                FocusHelper.Focus(messageDetailsView.QuickReplyAll);
            }
        }

        void ForwardInline_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Inline forward (keyboard)");

            Forward_Executed(sender, e);
        }

        void RemoveFromCurrentView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Remove from view (keyboard)");

            switch (ViewFilter.Current.Filter.CurrentView)
            {
                case ActivityView.MyInbox:
                    State.Archive();
                    break;
                
                case ActivityView.Archive:
                    State.Unarchive();
                    break;

                case ActivityView.Starred:
                    State.Star();
                    break;

                case ActivityView.Trash:
                    State.MoveToFolder(Folders.Inbox);
                    break;

                case ActivityView.Label:
                    var label = State.SelectedMessage.LabelsList.FirstOrDefault(l => l.Labelname == ViewFilter.Current.Filter.Label);

                    if (label != null)
                        State.RemoveLabel(label);

                    break;
            }
        }

        #endregion

        #region CanExecute

        void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

	    void View_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = Plugin != null;
	    }

	    void Reply_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = Plugin != null
	                       && State.CanReply;
	    }

	    void ReplyAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = Plugin != null
	                       && State.CanReplyAll;
	    }

	    void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = Plugin != null
	                       && State.CanForward;
	    }

	    void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = Plugin != null
	                       && State.CanDelete;
	    }

	    void Star_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = Plugin != null
	                       && State.CanStar;
	    }

	    void MarkRead_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = Plugin != null
	                       && State.CanMarkRead;
	    }

	    void MarkUnread_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = Plugin != null
	                       && State.CanMarkUnread;
	    }

	    void Archive_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        var message = (Message)e.Parameter;

	        e.CanExecute = !message.IsArchived && message.IsLast;
	    }

	    void Unarchive_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        var message = (Message)e.Parameter;

	        e.CanExecute = message.IsArchived && message.IsLast;
	    }

	    void Todo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        if (e.Parameter != null && streamView.StreamListView.SelectedItems.Count == 1)
	        {
	            var message = (Message)e.Parameter;

	            e.CanExecute = !(message.IsTodo || message.IsWaitingFor || message.IsSomeday);
	        }
	        else
	            e.CanExecute = true;
	    }

	    void WaitingFor_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        if (e.Parameter != null && streamView.StreamListView.SelectedItems.Count == 1)
	        {
	            var message = (Message)e.Parameter;

	            e.CanExecute = !(message.IsTodo || message.IsWaitingFor || message.IsSomeday);
	        }
	        else
	            e.CanExecute = true;
	    }

	    void Someday_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        if (e.Parameter != null && streamView.StreamListView.SelectedItems.Count == 1)
	        {
	            var message = (Message)e.Parameter;

	            e.CanExecute = !(message.IsTodo || message.IsWaitingFor || message.IsSomeday);
	        }
	        else
	            e.CanExecute = true;
	    }

	    void ClearAction_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        if (e.Parameter != null && streamView.StreamListView.SelectedItems.Count == 1)
	        {
	            var message = ((Message)e.Parameter);

	            e.CanExecute = message.IsTodo || message.IsSomeday || message.IsWaitingFor;
	        }
	        else
	            e.CanExecute = true;
	    }

	    #endregion
        
		#endregion 
	}
}