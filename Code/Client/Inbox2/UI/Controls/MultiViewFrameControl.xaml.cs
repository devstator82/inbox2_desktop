using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Inbox2.Core.Threading.Tasks;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Plugins;
using Inbox2.Framework.Stats;
using Inbox2.Framework.UI;
using Inbox2.Framework.UI.Controls;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.VirtualMailBox.View;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.UI.Windows;
using Microsoft.Win32;
using TabItem = Inbox2.Framework.UI.Controls.TabItem;
using Inbox2.Framework.Localization;

namespace Inbox2.UI.Controls
{
	/// <summary>
	/// Interaction logic for MultiViewFrameControl.xaml
	/// </summary>
	public partial class MultiViewFrameControl : UserControl
	{
	    #region Fields

	    private readonly DockControl dock;
	    private readonly Stopwatch stopwatch;

	    #endregion

	    #region Properties

	    protected ViewController ViewController
	    {
	        get { return ((ViewController)ClientState.Current.ViewController); }
	    }

	    #endregion

	    #region Construction

	    public MultiViewFrameControl()
	    {
            dock = new DockControl();
            stopwatch = new Stopwatch();
	        
            InitializeComponent();

			EventBroker.Subscribe(AppEvents.RequestStatusUpdate, () => Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					var window = new StatusUpdater();

					window.Show();
				}));

			EventBroker.Subscribe<List<Message>>(AppEvents.RequestAddLabels, messages => Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					var window = new AddLabelsWindow(messages);

					window.Show();
				}));

			EventBroker.Subscribe<List<Document>>(AppEvents.RequestAddLabels, documents => Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					var window = new AddLabelsWindow(documents);

					window.Show();
				}));

			EventBroker.Subscribe(AppEvents.RequestStatusUpdate, (string text) => Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					var window = new StatusUpdater();

					window.SetText(text);
					window.Show();
				}));

			EventBroker.Subscribe(AppEvents.RequestStatusUpdate, (string text, long channelid, UserStatus replyTo) => Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					var window = new StatusUpdater(new[] { channelid });
					window.SetReplyTo(replyTo);
					window.SetText(text);
					window.Show();
				}));

            EventBroker.Subscribe(AppEvents.RequestFocus, () => Thread.CurrentThread.ExecuteOnUIThread(
                () => ((TabItem)TabHost.SelectedItem).FocusFirstResponder()));
	    }

	    #endregion

	    #region Methods

	    protected override void OnInitialized(EventArgs e)
	    {
	        base.OnInitialized(e);			

	        // Update hotkeys during a tab change
	        TabHost.HeaderContent = dock;

	        // Create tabs for overview plugins
	        var items = 
	            PluginsManager.Current.Plugins
	                .Where(p => p.Overviews != null)
	                .SelectMany(p => p.Overviews)
	                .ToList();

	        foreach (var item in items)
	        {
	            CreateTabFor(item.Header, item.Icon, false, item, null, null, item.WellKnownView, false);
	        }

	        // Select home tab by default
	        TabHost.SelectedIndex = 0;
	    }

	    public TabItem ContainsTabFor(object dataInstance)
	    {
	        foreach (TabItem item in TabHost.Items)
	        {
				if (item.Content is IPersistableTab)
				{
					var data = (item.Content as IPersistableTab).SaveData();

					if (data != null && data.Equals(dataInstance))
						return item;
				}
	        }

	        return null;
	    }

	    public void ShowTabFor(WellKnownView view)
	    {
	        foreach (TabItem item in TabHost.Items)
	        {
	            if (item.WellKnownView == view)
	                TabHost.SelectedItem = item;
	        }
	    }	

	    public bool CanShutdown()
	    {
	        return !TabHost.Items.Cast<TabItem>().Any(t => t.Content is IVolatileTab);
	    }

	    public void CreateTabFor(string header, ImageSource icon, bool allowDelete, IViewPlugin viewPlugin, UIElement viewElement, object dataInstance, WellKnownView view, bool select)
	    {
	        // If allowdelete = false, we also hide the header
	        var newTab = new TabItem { Header = header, Icon = icon, AllowDelete = allowDelete, ShowHeader = allowDelete, Tag = viewPlugin, WellKnownView = view };

	        if (viewElement != null)
	        {
	            newTab.Content = viewElement;
	        }

	        if (newTab.Content is IControllableTab)
	        {
	            IControllableTab tab = (IControllableTab) newTab.Content;

	            // Create a textblock and bind it to the header property of the tab slave
	            TextBlock tb = new TextBlock();
	            tb.SetBinding(TextBlock.TextProperty, new Binding("Title") { Source = tab });

	            newTab.Header = tb;

	            // Attach the RequestCloseTab event
	            tab.RequestCloseTab += delegate { TabHost.RemoveItem(newTab); };

	            TabHost.HeaderContent = tab.CustomHeaderContent;
	        }			

	        if (newTab.Content is IPersistableTab)
	        {
	            Debug.Assert(dataInstance != null, "Data instance can not be null");

	            IPersistableTab tab = (IPersistableTab) newTab.Content;

	            tab.LoadData(dataInstance);
	        }

	        TabHost.Items.Add(newTab);

	        if (select)
	            TabHost.SelectedItem = newTab;
	    }

	    public void LoadMultiViewState()
	    {
	        // See if we have a stored application state
	        var state = ClientState.Current.Context.GetSettingFrom("state.xml") as PersistState;

	        if (state != null)
	        {
	            foreach (var tabState in state.Tabs)
	            {
	                var plugin = PluginsManager.Current.GetPluginBy(tabState.PluginType);

	                switch (tabState.ViewType)
	                {
	                    case ViewType.DetailsView:

	                        var view = plugin.DetailsView;
	                        CreateTabFor(view.Header, view.Icon, true, view, view.CreateView(), tabState.DataInstance, WellKnownView.DetailsView, true);

	                        break;

	                    case ViewType.NewItemView:

	                        var view1 = plugin.DetailsView;
	                        CreateTabFor(view1.Header, view1.Icon, true, view1, view1.CreateView(), tabState.DataInstance, WellKnownView.NewItemsView, true);

	                        break;
	                }
	            }

	            TabHost.SelectedIndex = state.SelectedTabIndex;
	        }
	    }

	    public void PersistChildren()
	    {
	        var persistState = new PersistState();

	        foreach (TabItem tab in TabHost.Items)
	        {
	            // Save child grid column-sizes
	            var grid = tab.GetChildrenOf<Grid>().FirstOrDefault();

	            if (grid != null && GridColumnSizeHelper.GetSaveGridColumnsSize(grid))
	                GridColumnSizeHelper.SaveColumnSizes(grid);

	            // Save open tab's state
	            if (tab.Content is IPersistableTab)
	            {
	                IPersistableTab saveTab = (IPersistableTab)tab.Content;

	                var data = saveTab.SaveData();

	                if (data != null)
	                {
	                    persistState.Tabs.Add(new TabState
	                        {
	                            PluginType = saveTab.Plugin.GetType(),
	                            ViewType = saveTab.ViewType,
	                            DataInstance = saveTab.SaveData()
	                        });
	                }
	            }
	        }

	        persistState.SelectedTabIndex = TabHost.SelectedIndex;

	        ClientState.Current.Context.SaveSettingTo("state.xml", persistState);
	    }

	    #endregion

        #region Event handlers

        void TabHost_TabItemSelected(object sender, TabItemEventArgs e)
		{
			stopwatch.Stop();

			if (e.TabItem.Content == null && e.TabItem.Tag is IViewPlugin)
			{
				// Lazy-initialize the contents of the tabitem
				var view = (IViewPlugin) e.TabItem.Tag;

				e.TabItem.Content = view.CreateView();
			}

		    string tabName;

			if (e.TabItem.Content is IControllableTab)
			{
				var tab = (IControllableTab) e.TabItem.Content;

				TabHost.HeaderContent = tab.CustomHeaderContent ?? dock;

				tabName = tab.Title;
			}
			else
			{
				TabHost.HeaderContent = dock;

				tabName = dock.GetType().ToString().Split('.').Last();
			}

            // Switch focus to first responder (if any)
            if (e.TabItem.Content as FrameworkElement != null)
                e.TabItem.FocusFirstResponder();

		    stopwatch.Start();

			ClientStats.LogEventWithTime("Trace Closed", (int)stopwatch.Elapsed.TotalMinutes);
			ClientStats.CreateNewTrace();
			ClientStats.LogEventWithSegment("Switch to tab", e.TabItem.WellKnownView.ToString());

			EventBroker.Publish(AppEvents.TabChanged, tabName);
		}

		void TabHost_TabItemClosing(object sender, TabItemCancelEventArgs e)
		{
			if (e.TabItem.Content is IDisposable)
			{
				((IDisposable)e.TabItem.Content).Dispose();
			}
		}	

		void MultiViewFrameControl_OnKeyDown(object sender, KeyEventArgs e)
		{
			// Undo/Redo
			if (e.Key == Key.Z && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
			{
				ClientState.Current.UndoManager.Undo();
			}

			if (e.Key == Key.Y && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
			{
				ClientState.Current.UndoManager.Redo();
			}
		}		

	    void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = true;
	    }

        void Escape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			if (PopupManager.ActivePopup != null && PopupManager.ActivePopup.IsOpen)
				PopupManager.ActivePopup.TryClose();

            ((TabItem)TabHost.SelectedItem).FocusFirstResponder();
        }

        void Search_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LogicalTreeWalker.Walk((FrameworkElement)TabHost.HeaderContent, delegate(UIElement element)
            {
                if (Responder.GetIsSearchResponder(element))
                {
                    FocusHelper.Focus(element);
                }
            });
        }

        void Compose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ClientStats.LogEvent("Compose");

            EventBroker.Publish<Message>(AppEvents.New, null);
        }

		void Refresh_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter == null)
			{
				ClientStats.LogEvent("SendReceive");

				EventBroker.Publish(AppEvents.RequestReceive);
				EventBroker.Publish(AppEvents.RequestSync);
				EventBroker.Publish(AppEvents.RequestSend);				
			}
			else
			{
				ClientStats.LogEvent("Receive single channel");

				EventBroker.Publish(AppEvents.RequestReceive, (ChannelInstance)e.Parameter);
				EventBroker.Publish(AppEvents.RequestSync, (ChannelInstance)e.Parameter);
			}
		}

		void UploadDocment_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Upload document");

			var dialog = new OpenFileDialog();

			dialog.Multiselect = true;

			var result = dialog.ShowDialog();

			if (result == true)
			{
				if (dialog.FileNames != null && dialog.FileNames.Length > 0)
				{
					foreach (var filename in dialog.FileNames)
					{
						var fi = new FileInfo(filename);

						if (fi.Exists)
						{
							var document = new Document
							{
								Filename = fi.Name,
								ContentType = ContentType.Attachment,
								DateCreated = DateTime.Now,
								DateSent = DateTime.Now,
								DocumentFolder = Folders.SentItems
							};

							document.StreamName = Guid.NewGuid().GetHash(12) + "_" + Path.GetExtension(document.Filename);
							document.ContentStream = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);

							EventBroker.Publish(AppEvents.DocumentReceived, document);
						}
					}
				}
			}
		}

		void UpdateStatus_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Update your status");

			EventBroker.Publish(AppEvents.RequestStatusUpdate);	
		}

	    void GotoInbox_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEventWithSegment("Goto folder (keyboard)", ActivityView.MyInbox.ToString());

            EventBroker.Publish(AppEvents.View, ActivityView.MyInbox);
	    }

	    void GotoArchive_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEventWithSegment("Goto folder (keyboard)", ActivityView.Archive.ToString());

            EventBroker.Publish(AppEvents.View, ActivityView.Archive);
	    }

	    void GotoSentItems_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEventWithSegment("Goto folder (keyboard)", ActivityView.Sent.ToString());

            EventBroker.Publish(AppEvents.View, ActivityView.Sent);
	    }

	    void GotoDrafts_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEventWithSegment("Goto folder (keyboard)", ActivityView.Drafts.ToString());

            EventBroker.Publish(AppEvents.View, ActivityView.Drafts);
	    }

	    void GotoStarred_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEventWithSegment("Goto folder (keyboard)", ActivityView.Starred.ToString());

            EventBroker.Publish(AppEvents.View, ActivityView.Starred);
	    }

	    void GotoLabel_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEventWithSegment("Goto folder (keyboard)", ActivityView.Label.ToString());

            LogicalTreeWalker.Walk((FrameworkElement)TabHost.HeaderContent, delegate(UIElement element)
            {
                if (Responder.GetIsSearchResponder(element))
                {
                    FocusHelper.Focus(element);

                    if (element is TextBox)
                    {
                        var tb = (TextBox) element;

                        tb.Text = "label: ";
                        tb.SelectionStart = tb.Text.Length;
                    }
                }
            });
	    }

	    void GotoContacts_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Goto contacts (keyboard)");

	        ClientState.Current.ViewController.MoveTo(WellKnownView.Contacts);
	    }

	    void GotoDocuments_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
			ClientStats.LogEvent("Goto documents (keyboard)");

            ClientState.Current.ViewController.MoveTo(WellKnownView.Documents);
	    }

        void GotoImages_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Goto images (keyboard)");

            ClientState.Current.ViewController.MoveTo(WellKnownView.Images);
        }     

        void StatusUpdate1_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 1);
        }

        void StatusUpdate2_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 2);
        }

        void StatusUpdate3_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 3);
        }

        void StatusUpdate4_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 4);
        }

        void StatusUpdate5_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 5);
        }

        void StatusUpdate6_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 6);
        }

        void StatusUpdate7_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 7);
        }

        void StatusUpdate8_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 8);
        }

        void StatusUpdate9_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("View status updates (keyboard)");

            EventBroker.Publish(AppEvents.RequestOpenToolbarItem, 9);
        }

        #endregion
	}
}
