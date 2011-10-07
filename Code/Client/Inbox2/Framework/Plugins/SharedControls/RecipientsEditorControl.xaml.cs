using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.UI;
using Inbox2.Framework.Utils.Text;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.Plugins.SharedControls
{
	/// <summary>
	/// Interaction logic for RecipientsEditorControl.xaml
	/// </summary>
    public partial class RecipientsEditorControl : UserControl, IFocusChild
	{
		// Using a DependencyProperty as the backing store for InnerFontSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty InnerFontSizeProperty =
			DependencyProperty.Register("InnerFontSize", typeof(int), typeof(RecipientsEditorControl), new UIPropertyMetadata(10));

	    public event EventHandler<TextChangedEventArgs> TextChanged;

		protected AdvancedObservableCollection<SourceAddress> recipients;
		protected TextPointer contentEnd;
		
		protected Flipper wordFlipper;
		protected Flipper autocompleteFlipper;

		private readonly VirtualMailBox.VirtualMailBox mailbox;

        public bool ValidationEnabled { get; set; }

		public bool SuppressListForCurrentWord { get; set; }

		public ChannelInstance TargetChannel { get; set; }

        public UIElement FocusElement
        {
            get { return Editor; }
        }

		public bool IsPopupOpen
		{
			get { return AutoCompletionPopup.IsOpen; }
		}

		public int InnerFontSize
		{
			get { return (int)GetValue(InnerFontSizeProperty); }
			set { SetValue(InnerFontSizeProperty, value); }
		}

	    public string Text
	    {
	        get
	        {
                StringBuilder sb = new StringBuilder();

                DocumentWalker walker = new DocumentWalker();                
	            walker.VisualVisited += delegate(object sender, object visitedObject, bool start)
                    {
                        if (visitedObject is Run)
                            sb.Append((visitedObject as Run).Text);
                    };

                walker.Walk(Editor.Document);
                
                return sb.ToString();
	        }
	    }

		public AdvancedObservableCollection<SourceAddress> Recipients
		{
			get { return recipients; }
		}

		public RecipientsEditorControl()
		{
			InitializeComponent();

            InputCommands.Stub(this);

			mailbox = VirtualMailBox.VirtualMailBox.Current;
			recipients = new AdvancedObservableCollection<SourceAddress>();
			
			wordFlipper = new Flipper(TimeSpan.FromMilliseconds(400), ProcessCurrentWord);
			autocompleteFlipper = new Flipper(TimeSpan.FromMilliseconds(500), SearchContacts);

            // Default the validation is enabled
		    ValidationEnabled = true;

			DataContext = this;			

			var channel = ChannelsManager.GetDefaultChannel();

			if (channel != null)
				TargetChannel = channel;
		}

		public void AddRecipient(object source)
		{
			TextRange range = WordBreaker.GetWordRange(Editor.CaretPosition);

			// Create display object
			ContentControl ctrl = new ContentControl();
			ctrl.Content = source;

			var container = new InlineUIContainer(ctrl, Editor.CaretPosition);

			contentEnd = container.ContentEnd;

			// Remove actual text
			range.Text = String.Empty;

			// Move caret to end of what was the word
			Editor.CaretPosition = contentEnd;

			// Rebuild list of recipients on insert
			RebuildRecipientsList();		
		}

		public void ClearRecipients()
		{
			Editor.Document.Blocks.Clear();
		}

		void AutoCompletionListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			var contact = (e.OriginalSource as DependencyObject)
				.FindListViewItem<Profile>(AutoCompletionListBox.ItemContainerGenerator);

			if (contact != null)
			{
				AddRecipient(contact);
			}
		}		

		void AutoCompletionListBox_LostFocus(object sender, RoutedEventArgs e)
		{
			// Only allow completion list to be visible if InputTextBox 
			// or AutoCompletionListBox have the focus.
			HideList();
		}

		void AutoCompletionListBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter || e.Key == Key.Tab)
			{
				InsertSelectedContact();
				e.Handled = true;
			}
		}

		void Editor_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			wordFlipper.Delay();

			switch (e.Key)
			{
				case Key.Up:
					// Move selection up
					if (AutoCompletionListBox.SelectedIndex > 0)
						AutoCompletionListBox.SelectedIndex--;

					e.Handled = true;

					break;

				case Key.Down:
					// Move selection down
					if (AutoCompletionListBox.SelectedIndex < AutoCompletionListBox.Items.Count)
						AutoCompletionListBox.SelectedIndex++;

					e.Handled = true;

					break;

				case Key.Enter:
					// Accept selection
					if (IsPopupOpen)
						InsertSelectedContact();

					break;

				case Key.Escape:
					// Restore state before we opened the list
					HideList();

					e.Handled = true;
					break;
				
				case Key.Tab:
					if (IsPopupOpen)
						InsertSelectedContact();
					else
					{
						TextRange range = WordBreaker.GetWordRange(Editor.CaretPosition);

						string text = range.Text.Trim();

						// Break out when use tabs and nothing has been entered
						if (String.IsNullOrEmpty(text.Trim()))
							return;

						ProcessCurrentWord();
					}						

					e.Handled = true;

					break;
				case Key.Space:
				case Key.OemComma:
				case Key.OemSemicolon:
					// Add word being typed in by user
					ProcessCurrentWord();

					break;
				default:
					// Clear color of range
					TextRange currentRange = WordBreaker.GetWordRange(Editor.CaretPosition);
					currentRange.ApplyPropertyValue(TextBlock.ForegroundProperty, Brushes.Black);

					break;
			}
		}

		void ProcessCurrentWord()
		{
			TextRange range = WordBreaker.GetWordRange(Editor.CaretPosition);

			string text = range.Text.Trim();

			// Validate if the email address is valid
			if (!SourceAddress.IsValidEmail(text))
			{
				if (ValidationEnabled)
				{
                    range.ApplyPropertyValue(TextBlock.ForegroundProperty, FindResource("TabAndLightButtonText"));

					SuppressListForCurrentWord = false;
				}

				return;
			}

			SuppressListForCurrentWord = false;

			SourceAddress address = new SourceAddress(text);
			AddRecipient(address);

			// Notify listeners of new entry
			RebuildRecipientsList();
		}

		void Editor_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Back:
				case Key.Delete:
					// Rebuild list of recipients on delete, we don't do this in the PreviewKeyDown
					// because this doesn't work correctly with listeners on the recipients collection
					RebuildRecipientsList();

					break;

				case Key.C:
				case Key.V:
				case Key.X:
					if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
						RebuildRecipientsList();

					break;
			}
		}

		void Editor_KeyUp(object sender, KeyEventArgs e)
		{
			autocompleteFlipper.Delay();
		}
		
		void SearchContacts()
		{
			TextRange tr = WordBreaker.GetWordRange(Editor.CaretPosition);

			// Make sure we have at least two chars
			if (tr.Text.Length >= 3)
			{
				var prevResults = AutoCompletionListBox.ItemsSource as List<Profile>;

				List<Profile> results;

				var q = mailbox.Profiles.Where(
						p => p.SourceAddress.ToString().IndexOf(Text, StringComparison.InvariantCultureIgnoreCase) > -1)
					.Where(p => p.SourceChannelId == 0 || p.SourceChannel.Charasteristics.SupportsPrivateMessage)
					.Union(mailbox.Persons
						.Where(r => r.Name.IndexOf(Text, StringComparison.InvariantCultureIgnoreCase) > -1)
						.SelectMany(r => r.Profiles
							.Where(p => p.SourceChannelId == 0 || p.SourceChannel.Charasteristics.SupportsPrivateMessage)))
					.OrderByDescending(p => p.Messages.Count)
					.Distinct()
					.Take(10);

				using (mailbox.Profiles.ReaderLock)
					results = q.ToList();

				if (prevResults != null && prevResults.Count == results.Count)
					return;

				if (results.Count > 0)
				{
					ShowList(results);

					return;
				}				
			}

			HideList();
		}

		void InsertSelectedContact()
		{
			// Get selected contact
			var contact = AutoCompletionListBox.SelectedItem as Profile;

			if (contact == null)
				return;

			AddRecipient(contact);

			HideList();
		}

		/// <summary>
		/// Hides the list.
		/// </summary>
		public void HideList()
		{
			AutoCompletionListBox.ItemsSource = null;
			AutoCompletionPopup.IsOpen = false;
		}

		/// <summary>
		/// Shows the list.
		/// </summary>
		/// <param name="results">The results.</param>
		/// <param name="pos">The pos.</param>
		void ShowList(IEnumerable<Profile> results)
		{
			if (SuppressListForCurrentWord)
				return;

			var screenPos = Editor.TranslatePoint(new Point(), this);

			Canvas.SetTop(AutoCompletionListBox, screenPos.Y + 20);

			AutoCompletionListBox.ItemsSource = results;
			AutoCompletionPopup.IsOpen = true;
		}

		/// <summary>
		/// Rebuilds the recipients list.
		/// </summary>
		void RebuildRecipientsList()
		{
			SuppressListForCurrentWord = false;

			recipients.Clear();

			DocumentWalker walker = new DocumentWalker();
			walker.VisualVisited += delegate(object sender, object visitedObject, bool start)
			{
				if (visitedObject is ContentControl)
				{
					ContentControl contentControl = (ContentControl)visitedObject;
					object content = contentControl.Content;

					if (content is SourceAddress)
						recipients.Add(content as SourceAddress);

					if (content is Profile)
					{
						var profile = (content as Profile);
						var address = profile.SourceAddress;

						address.SetChannel(profile.SourceChannel);

						recipients.Add(address);
					}
				}
			};
			walker.Walk(Editor.Document);
		}

		void UserControl_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			Keyboard.Focus(Editor);
		}

		void UserControl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ProcessCurrentWord();

			HideList();
		}

        void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

		void AutoCompletionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//if (e.AddedItems != null)
			//    if (e.AddedItems.Count == 0)
			//        System.Diagnostics.Debugger.Break();
		}	    
	}
}
