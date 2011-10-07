using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Inbox2.Core.Configuration;
using Inbox2.Framework.Collections;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Microsoft.Win32;
using mshtml;
using Label = Inbox2.Framework.VirtualMailBox.Entities.Label;

namespace Inbox2.Framework.Plugins.SharedControls
{
	/// <summary>
	/// Interaction logic for MessageEditControl.xaml
	/// </summary>
	public partial class MessageEditControl : UserControl, INotifyPropertyChanged
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> MessageSent;
		public event EventHandler<EventArgs> SubjectUpdated;	

		private readonly VirtualMailBox.VirtualMailBox mailbox = VirtualMailBox.VirtualMailBox.Current;
		private readonly CustomCompositeCollection<SourceAddress> recipients; 
		private readonly AdvancedObservableCollection<AttachmentDataHelper> attachedFiles;

		private Message sourceMessage;

		#endregion

		#region Properties

		public AdvancedObservableCollection<AttachmentDataHelper> AttachedFiles
		{
			get { return attachedFiles; }
		}

		public AdvancedObservableCollection<SourceAddress> To
		{
			get { return ToRecipients.Recipients; }
		}

		public AdvancedObservableCollection<SourceAddress> CC
		{
			get { return CcRecipients.Recipients; }
		}

		public AdvancedObservableCollection<SourceAddress> BCC
		{
			get { return BccRecipients.Recipients; }
		}

		public IEnumerable<ChannelInstance> MailChannels
		{
			get 
			{
				return ChannelsManager.Channels.Where(c => c.Configuration.Charasteristics.SupportsEmail);
			}
		}

		public IEnumerable<ChannelGroup> NonMailChannelGroups
		{
			get
			{
				var groups = new List<ChannelGroup>();

				foreach (ChannelInstance channel in ChannelsManager.Channels)
				{
					if (!channel.Configuration.Charasteristics.SupportsEmail)
					{
						ChannelInstance channel1 = channel;

						var existingGroup = groups.FirstOrDefault(g => g.ChannelGroupName == channel1.Configuration.DisplayName);

						if (existingGroup == null)
						{
							groups.Add(new ChannelGroup(recipients, new List<ChannelInstance> { channel }, channel.Configuration.DisplayName));
						}
						else
						{
							existingGroup.Channels.Add(channel);
						}
					}
				}

				return groups;
			}
		}

		public bool IsCCBCCFieldOpen { get; set; }

		public bool AddWaitingFor { get; set; }

		public Message SourceMessage
		{
			get { return sourceMessage; }
			set
			{
				sourceMessage = value;

				OnPropertyChanged("Channels");
			}
		}

		public string Context
		{
			get { return ContextTextBox.Text; }
		}

		public long SelectedChannelId
		{
			get
			{
				return ((ChannelInstance)FromAccount.SelectedItem).Configuration.ChannelId;
			}
			set
			{
				FromAccount.SelectedItem = ChannelsManager.GetChannelObject(value);
			}
		}

		public bool IsDraft
		{
			get
			{
				if (SourceMessage == null)
					return false;

				return SourceMessage.MessageFolder == Folders.Drafts;
			}
		}

		public string MessageText { get; set; }

		public HTMLDocument Document
		{
			get { return (HTMLDocument)HtmlView.Document; }
		}

		#endregion

		#region Construction

		public MessageEditControl()
		{
			InitializeComponent();

			recipients = new CustomCompositeCollection<SourceAddress>(To, CC, BCC);
			attachedFiles = new AdvancedObservableCollection<AttachmentDataHelper>();

			DataContext = this;
		}		

		#endregion

		#region Methods

		#region Recipient handling

		public void AddToRecipient(SourceAddress recipient)
		{
			ToRecipients.AddRecipient(mailbox.Find(recipient));
		}

		public void AddToRecipient(Profile profile)
		{
			ToRecipients.AddRecipient(profile);
		}

		public void AddCCRecipient(SourceAddress recipient)
		{
			CcRecipients.AddRecipient(mailbox.Find(recipient));

			IsCCBCCFieldOpen = true;
		}

		public void AddBCCRecipient(SourceAddress recipient)
		{
			BccRecipients.AddRecipient(mailbox.Find(recipient));

			IsCCBCCFieldOpen = true;
		}

		public void AddToRecipients(IEnumerable<Profile> recipients)
		{
			foreach (var recipient in recipients)
				ToRecipients.AddRecipient(recipient);
		}

		public void AddToRecipients(IEnumerable<SourceAddress> recipients)
		{
			foreach (var recipient in recipients)
				ToRecipients.AddRecipient(mailbox.Find(recipient));
		}

		public void AddCCRecipients(IEnumerable<SourceAddress> recipients)
		{
			foreach (var recipient in recipients)
				CcRecipients.AddRecipient(mailbox.Find(recipient));

			if (CcRecipients.Recipients.Count > 0)
				IsCCBCCFieldOpen = true;
		}

		public void AddBCCRecipients(IEnumerable<SourceAddress> recipients)
		{
			foreach (var recipient in recipients)
				BccRecipients.AddRecipient(mailbox.Find(recipient));

			if (BccRecipients.Recipients.Count > 0)
				IsCCBCCFieldOpen = true;
		}

		public void ClearRecipients()
		{
			ToRecipients.ClearRecipients();
			CcRecipients.ClearRecipients();
			BccRecipients.ClearRecipients();
		} 

		public void SetContext(string context)
		{
			ContextTextBox.Text = context;
		}	

		#endregion

		#region Draft handling

		public void SaveDraft()
		{
			var message = CreateMessage(Folders.Drafts);

			if (message.MessageId.HasValue)
			{
				ClientState.Current.DataService.Update(message);									
			}
			else
			{
				ClientState.Current.DataService.Save(message);

				mailbox.Messages.Add(message);
			}

			SaveDocuments(message, IsDraft);

			SourceMessage = message;

			EventBroker.Publish(AppEvents.MessageUpdated);

			ClientState.Current.ShowMessage(
				new AppMessage(Strings.DraftSavedSuccessfully)
				{
					EntityId = message.MessageId.Value,
					EntityType = EntityType.Message
				}, MessageType.Success);
		}

		public void CancelDraft()
		{			
			SourceMessage.MoveToFolder(Folders.Trash);
		}

		#endregion

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			// Load base html
			string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

			var isRtl = Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;            

			HtmlView.Navigate(new Uri(Path.Combine(path, isRtl ? "editor_rtl.html" : "editor.html")));
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (e.Key == Key.Escape)
			{
				TryCancel();
			}
		}

		public void TryCancel()
		{
			if (IsDraft)
			{
				if (Inbox2MessageBox.Show("Cancel and move draft to trash folder?", Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.No)
					return;
				else
					CancelDraft();
			}
			else if (HasMessageContent(true))
			{
				if (Inbox2MessageBox.Show("Cancel and lose all changes?", Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.No)
					return;
			}

			CloseEditor();
		}

		public bool HasMessageContent(bool deepInspection)
		{
			if (deepInspection)
			{
				return !String.IsNullOrEmpty(Context.Trim()) || !String.IsNullOrEmpty(GetBodyStream().Trim()) || To.Count > 0;
			}
			else
				return !String.IsNullOrEmpty(Context.Trim());
		}

		public string GetBodyStream()
		{
			Document.parentWindow.execScript("saveContent()", "javascript");
			
			return ((HTMLTextAreaElement)Document.getElementById("editor")).value ?? String.Empty;
		}

		public void SendMessage()
		{
			bool wasDraft = IsDraft;

			var message = CreateMessage(Folders.SentItems);
			
			if (wasDraft)
			{
				// Remove message from mailbox
				mailbox.Messages.Remove(message);
			}
			else
			{
				// Save our message
				ClientState.Current.DataService.Save(message);	
			}				

			// Attach documents
			SaveDocuments(message, wasDraft);

			// Add message to mailbox
			EventBroker.Publish(AppEvents.MessageStored, message);

			// Save command
			CommandQueue.Enqueue(AppCommands.SendMessage, message);
			
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				ClientState.Current.ShowMessage(
					new AppMessage(Strings.MessageWillBeSentLater)
						{
							EntityId = message.MessageId.Value,
							EntityType = EntityType.Message
						}, MessageType.Success);
			}
		}

		Message CreateMessage(int messageFolder)
		{
			Message message = IsDraft ? SourceMessage : new Message();
			var channel = (ChannelInstance)FromAccount.SelectedItem;

			if (IsDraft)
			{				
				if (message.BodyHtmlStreamName.HasValue)
				{
					// Delete old body stream
					ClientState.Current.Storage.Delete("m", message.BodyHtmlStreamName.ToString());
				}

				message.To.Clear();
				message.CC.Clear();
				message.BCC.Clear();				
			}

			if (channel == null)
				channel = MailChannels.First();

			if (SourceMessage != null)
			{
				message.InReplyTo = SourceMessage.MessageIdentifier;
				message.ConversationIdentifier = SourceMessage.ConversationIdentifier;
			}
			else
			{				
				message.ConversationIdentifier = Guid.NewGuid().ToConversationId();
			}			

			message.Context = ContextTextBox.Text;
			message.From = new SourceAddress(channel.InputChannel.SourceAdress, SettingsManager.ClientSettings.AppConfiguration.Fullname);
			message.TargetChannelId = channel.Configuration.ChannelId;
			message.To.AddRange(To);
			message.CC.AddRange(CC);
			message.BCC.AddRange(BCC);
			
			var access = new ClientMessageAccess(message, null, GetBodyStream());

			message.BodyHtmlStreamName = access.WriteBodyHtml();
			message.BodyPreview = access.GetBodyPreview();
			message.IsRead = true;
			message.DateSent = DateTime.Now;
			message.MessageFolder = messageFolder;
			message.DateSent = DateTime.Now;
			message.DateCreated = DateTime.Now;

			if (AddWaitingFor && !message.IsWaitingFor)
				message.AddLabel(new Label(LabelType.WaitingFor), false);
			else if (!AddWaitingFor && message.IsWaitingFor)
				message.RemoveLabel(message.LabelsList.First(l => l.LabelType == LabelType.WaitingFor), false);

			if (SourceMessage != null)
				SourceMessage.TrackAction(ActionType.ReplyForward);

			return message;
		}		

		void SaveDocuments(Message message, bool isDraft)
		{
			if (isDraft)
			{
				// Delete old attachments from message, will be re-added again below
				SourceMessage.Documents.ToList()
					.ForEach(d => d.DeleteFrom(SourceMessage));
			}

			// Add attachments to message
			foreach (var file in attachedFiles)
			{
				var document = new Document
				{
					Message = message,
					Filename = file.Filename,
					ContentType = ContentType.Attachment,
					SourceChannelId = message.SourceChannelId,
					DateCreated = DateTime.Now,
					DateSent = DateTime.Now,
					DocumentFolder = Folders.SentItems
				};

				document.StreamName = Guid.NewGuid().GetHash(12) + "_" + Path.GetExtension(document.Filename);
				document.ContentStream = new FileStream(file.Streamname, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

				EventBroker.Publish(AppEvents.DocumentReceived, document);

				// Might needs this when deleting an attachment from a draft message
				file.DocumentId = document.DocumentId;
			}
		}		

		public void CloseEditor()
		{
			if (MessageSent != null)
				MessageSent(this, EventArgs.Empty);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public void Dispose()
		{
			HtmlView.Dispose();
		}

		#endregion

		#region Event handlers

		void ContextTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (SubjectUpdated != null)
				SubjectUpdated(this, EventArgs.Empty);
		}

		void TabSink_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			// Sets focus on html editor
			Document.parentWindow.execScript("focusEditor();");
		}

		void CcRecipients_LayoutUpdated(object sender, EventArgs e)
		{
			OnPropertyChanged("IsCCBCCFieldOpen");
		}

		void HtmlView_OnLoadCompleted(object sender, NavigationEventArgs e)
		{
			if (!String.IsNullOrEmpty(MessageText))
				Document.parentWindow.execScript(
					String.Format("setContent({0})", MessageText.JsEncode()), "javascript");		    
		}
		
		void FromAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (FromAccount.SelectedItem == null)
				return;

			ToRecipients.TargetChannel = (ChannelInstance)FromAccount.SelectedItem;
			CcRecipients.TargetChannel = (ChannelInstance)FromAccount.SelectedItem;
			BccRecipients.TargetChannel = (ChannelInstance)FromAccount.SelectedItem;
		}

		#region Attachments
		
		void AddAttachment_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Attach document to new message");

			var dialog = new OpenFileDialog();

			dialog.Multiselect = true;

			var result = dialog.ShowDialog();

			if (result == true)
			{
				AttachFiles(dialog.FileNames);
			}
		}

		void ShowCCBCC_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Show CC/BCC field in new message");

			IsCCBCCFieldOpen = true;

			OnPropertyChanged("IsCCBCCFieldOpen");
		}

		void HideCCBCC_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Hide CC/BCC field in new message");

			IsCCBCCFieldOpen = false;

			OnPropertyChanged("IsCCBCCFieldOpen");
		}

		void FileDragOver(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.None;

			string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

			if (fileNames != null && fileNames.Length > 0)
				e.Effects = DragDropEffects.Copy;

			e.Handled = true;
		}

		void FileDrop(object sender, DragEventArgs e)
		{
			ClientStats.LogEvent("Attach document to new message (drop)");

			string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

			AttachFiles(fileNames);

			e.Handled = true;
		}

		void AttachFiles(string[] fileNames)
		{
			if (fileNames != null && fileNames.Length > 0)
			{
				foreach (var filename in fileNames)
				{
					FileInfo fi = new FileInfo(filename);

					if (fi.Exists)
						attachedFiles.Add(new AttachmentDataHelper(fi.Name, fi.FullName));
				}
			}
		}

		void Attachments_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				ClientStats.LogEvent("Delete attachment from new message");

				var attachment = (AttachmentDataHelper) AttachmentsListView.SelectedItem;

				AttachedFiles.Remove(attachment);	
				
				if (IsDraft && attachment.DocumentId.HasValue)
				{
					// Also remove version that is attached to our draft message
					var document = SourceMessage.Documents.FirstOrDefault(d => d.DocumentId == attachment.DocumentId);

					if (document != null)
						document.DeleteFrom(SourceMessage);
				}

				e.Handled = true;
			}
		}

		#endregion

		#endregion
	}
}
