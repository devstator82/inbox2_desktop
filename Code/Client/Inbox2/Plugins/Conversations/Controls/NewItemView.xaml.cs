using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Stats;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Plugins.Conversations.Helpers;
using Inbox2.Framework.Localization;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for NewItemView.xaml
	/// </summary>
	public partial class NewItemView : UserControl, IVolatileTab, INotifyPropertyChanged, IDisposable
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> RequestCloseTab;

		private readonly NewItemDock dockControl;

		public string Title
		{
			get
			{
				var sb = new StringBuilder();

				sb.Append(Strings.NewMessage);
				sb.Append(" - ");

				if (String.IsNullOrEmpty(MessageEditControl.Context))
					sb.Append(Strings.Untitled);
				else
					sb.Append(MessageEditControl.Context);

				return sb.ToString();
			}
		}

		public Control CustomHeaderContent
		{
			get { return dockControl; }
		}

		public PluginPackage Plugin
		{
			get { return PluginsManager.Current.GetPlugin<ConversationsPlugin>(); }
		}

		public ViewType ViewType
		{
			get { return ViewType.NewItemView; }
		}
	
		public NewItemView()
		{
			InitializeComponent();

			dockControl = new NewItemDock(MessageEditControl);
		}

		public void LoadData(object data)
		{
			NewMessageDataHelper helper = (NewMessageDataHelper) data;

			// New message, select the default channel
			MessageEditControl.SelectedChannelId = ChannelsManager.GetDefaultChannel().Configuration.ChannelId;	

			if (helper != null)
			{
				if (helper.SourceMessageId.HasValue)
					using (VirtualMailBox.Current.Messages.ReaderLock)
						MessageEditControl.SourceMessage = VirtualMailBox.Current.Messages
							.First(m => m.MessageId == helper.SourceMessageId);
							
				if (helper.To != null) MessageEditControl.AddToRecipients(helper.To);
				if (helper.Cc != null) MessageEditControl.AddCCRecipients(helper.Cc);
				if (helper.Bcc != null) MessageEditControl.AddBCCRecipients(helper.Bcc);
				if (helper.AttachedFiles != null)
				{
					foreach (var attachedFile in helper.AttachedFiles)
					{
						var info = new FileInfo(attachedFile.Streamname);

						if (!info.Exists)
						{
							ClientState.Current.ShowMessage(
								new AppMessage(String.Format(Strings.FileNotFound, info.Name)), MessageType.Error);

							continue;			
						}

						MessageEditControl.AttachedFiles.Add(attachedFile);
					}
				}

				MessageEditControl.SetContext(helper.Context);
				MessageEditControl.MessageText = helper.Body;

				if (helper.SelectedChannelId > 0)
					MessageEditControl.SelectedChannelId = helper.SelectedChannelId;
			}					
		}

		public object SaveData()
		{
			return null;
		}

		public void Dispose()
		{
			MessageEditControl.Dispose();
		}

		void SaveDraft_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		void SaveDraft_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (MessageEditControl.HasMessageContent(false))
			{
				ClientStats.LogEvent("Save draft (keyboard)");

				MessageEditControl.SaveDraft();
			}
		}

		void MessageEditControl_OnSubjectUpdated(object sender, EventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs("Title"));
		}

		void MessageEditControl_MessageSent(object sender, EventArgs e)
		{
			if (RequestCloseTab != null)
			{
				RequestCloseTab(this, EventArgs.Empty);
			}
		}
	}
}
