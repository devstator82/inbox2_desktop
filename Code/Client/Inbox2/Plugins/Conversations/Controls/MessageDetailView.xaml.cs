using System;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Input;
using System.Windows.Navigation;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Stats;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.Conversations.Helpers;
using Inbox2.Plugins.Conversations.Utils;
using MsHtmHstInterop;
using mshtml;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Message = Inbox2.Framework.VirtualMailBox.Entities.Message;
using UserControl = System.Windows.Controls.UserControl;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for MessageDetailView.xaml
	/// </summary>
	public partial class MessageDetailView : UserControl, INotifyPropertyChanged, IDisposable
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private Flipper flipper;
		private Message message;
		private bool initialized;

		public Message Message
		{
			get { return message; }
		}

		public MessageDetailView()
		{
			InitializeComponent();

			EventBroker.Subscribe(AppEvents.ShuttingDown, () => Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					if (message != null)
					{
						// Mark message as read if setting is enabled during shutdown
						if (SettingsManager.ClientSettings.AppConfiguration.MarkReadWhenViewing)
							message.MarkRead();
					}
				}));

			EventBroker.Subscribe(AppEvents.TabChanged, delegate(string newTab)
            	{
					// Visual fix
					QuickReplyAll.Height = 40;
            	});

			DataContext = this;
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

			HtmlView.Navigate(new Uri(Path.Combine(path, "ThreadView.html")));

			ICustomDoc doc = (ICustomDoc)HtmlView.Document;
			doc.SetUIHandler(new DocHostUIHandler());
		}

		public void Show(Message message)
		{
			// This is for our fake message
			if (message.MessageId < 0)
				return;

			this.message = message;

			QuickReplyAll.Text = String.Empty;
			
			// if document hasn't loaded (the first time), loadcomplete will take care of show for us
			if (initialized)
				BuildAndShowMessage();						

			// When message is shown due to system selection, do not track the read action
			if (!ThreadFlag.IsSet)
				message.TrackAction(ActionType.Read);

			if (flipper != null)
				flipper.Dispose();

			// Mark message read if after setting is enabled
			var markReadAFter = SettingsManager.ClientSettings.AppConfiguration.MarkReadWhenViewingAfter;			
			
			if (markReadAFter.HasValue)
			{
				flipper = new Flipper(TimeSpan.FromSeconds(markReadAFter.Value), delegate
				{
					if (!message.IsRead)
						message.MarkRead();
				});

				flipper.Delay();				
			}
			else if (SettingsManager.ClientSettings.AppConfiguration.MarkReadWhenViewing)
				message.MarkRead();

			OnPropertyChanged("Message");
		}	

		public void Dispose()
		{
			HtmlView.Dispose();
		}

		void BuildAndShowMessage()
		{
			if (message != null)
			{
				var builder = new MessageBuilder(message);

				// Inject new content with javascript (prevents the annoying IE clicking sound)
				((HTMLDocument) HtmlView.Document).parentWindow.execScript(
					String.Format("refresh({0})", builder.GetMessageHtmlView().JsEncode()), "javascript");
			}

			initialized = true;
		}

		void HtmlView_LoadCompleted(object sender, NavigationEventArgs e)
		{
			BuildAndShowMessage();			
		}		

		void View_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ActionHelper.View((SourceAddress)e.Parameter);
		}

		void Send_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Quick reply all send");

			if (String.IsNullOrEmpty(QuickReplyAll.Text.Trim()))
				return;

			var newMessage = new Message();

			#region Create message

		    var channel = message.SourceChannelId == 0 ? 
                ChannelsManager.GetDefaultChannel() :
                ChannelsManager.GetChannelObject(Message.SourceChannelId);

			var recipients = new SourceAddressCollection();
			var sourceAddress = channel.InputChannel.GetSourceAddress();

			recipients.Add(Message.From);
			recipients.AddRange(Message.To);

			// Remove our own address from recipient list
			if (recipients.Contains(sourceAddress))
				recipients.Remove(sourceAddress);

			newMessage.InReplyTo = Message.MessageIdentifier;
			newMessage.ConversationIdentifier = Message.ConversationIdentifier;
			newMessage.Context = "Re: " + Message.Context;
			newMessage.From = channel.InputChannel.GetSourceAddress();
			newMessage.TargetChannelId = channel.Configuration.ChannelId;
			newMessage.To.AddRange(recipients);
			newMessage.CC.AddRange(Message.CC);

			var access = new ClientMessageAccess(newMessage, null,
				MessageBodyGenerator.CreateBodyTextForReply(Message, QuickReplyAll.Text.Nl2Br()));

			newMessage.BodyHtmlStreamName = access.WriteBodyHtml();
			newMessage.BodyPreview = access.GetBodyPreview();
			newMessage.IsRead = true;
			newMessage.DateSent = DateTime.Now;
			newMessage.MessageFolder = Folders.SentItems;
			newMessage.DateSent = DateTime.Now;
			newMessage.DateCreated = DateTime.Now;

			#endregion

			#region Send message

			ClientState.Current.DataService.Save(newMessage);

			// Add message to mailbox
			EventBroker.Publish(AppEvents.MessageStored, newMessage);

			// Save command
			CommandQueue.Enqueue(AppCommands.SendMessage, newMessage);
			
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				ClientState.Current.ShowMessage(
					new AppMessage(Strings.MessageWillBeSentLater)
						{
							EntityId = newMessage.MessageId.Value,
							EntityType = EntityType.Message
						}, MessageType.Success);
			}

			QuickReplyAll.Text = String.Empty;

			message.TrackAction(ActionType.ReplyForward);

			#endregion
		}

		void OpenDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			EventBroker.Publish(AppEvents.View, (Document)e.Parameter);
		}

		void SaveDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			EventBroker.Publish(AppEvents.Save, (Document)e.Parameter);
		}

		void Maximize_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(QuickReplyAll.Text.Trim()))
			{
				ClientStats.LogEvent("Quick reply all maximize");

				ActionHelper.ReplyAll(Message, QuickReplyAll.Text.Nl2Br());

				QuickReplyAll.Text = String.Empty;
			}
		}

		void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}	

		void QuickReplyAll_KeyUp(object sender, KeyEventArgs e)
		{
            if (e.Key == Key.Escape)
            {
				ClientStats.LogEvent("Quick reply all cancel");

                QuickReplyAll.Text = String.Empty;

                EventBroker.Publish(AppEvents.RequestFocus);
            }
		}

		void QuickReplyAllGrid_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			QuickReplyAll.Height = 75;
		}

		void QuickReplyAllGrid_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			QuickReplyAll.Height = 40;
		}
	
		void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}		
	}
}
