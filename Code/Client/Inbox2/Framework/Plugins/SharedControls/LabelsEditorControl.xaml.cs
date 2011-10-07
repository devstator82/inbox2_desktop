using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.UI;
using Inbox2.Framework.UI.Controls;
using Inbox2.Framework.Utils.Text;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Keyboard = System.Windows.Input.Keyboard;
using Label = Inbox2.Framework.VirtualMailBox.Entities.Label;

namespace Inbox2.Framework.Plugins.SharedControls
{
    public class LabelsTextBox : TextOnlyRichTextBox
	{
		public static readonly DependencyProperty ShowWatermarkProperty = DependencyProperty.Register("ShowWatermark", typeof(bool), typeof(LabelsTextBox), new UIPropertyMetadata(true));
		public static readonly DependencyProperty WatermarkStyleProperty = DependencyProperty.Register("WatermarkStyle", typeof(Style), typeof(LabelsTextBox));

		public Style WatermarkStyle
		{
			get { return (Style)GetValue(WatermarkStyleProperty); }
			set { SetValue(WatermarkStyleProperty, value); }
		}

		public bool ShowWatermark
		{
			get { return (bool)GetValue(ShowWatermarkProperty); }
			set { SetValue(ShowWatermarkProperty, value); }
		}
	}

	/// <summary>
	/// Interaction logic for LabelsEditorControl.xaml
	/// </summary>
    public partial class LabelsEditorControl : UserControl, IFocusChild
	{
		#region Dependency Properties

		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(Message), typeof(LabelsEditorControl), new UIPropertyMetadata(LabelsEditorControl_MessageChanged));
		public static readonly DependencyProperty HideOnEmptyProperty = DependencyProperty.Register("HideOnEmpty", typeof(bool), typeof(LabelsEditorControl), new UIPropertyMetadata(false, LabelsEditorControl_HideOnEmptyChanged));

		public Message Message
		{
			get { return (Message)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		public bool HideOnEmpty
		{
			get { return (bool)GetValue(HideOnEmptyProperty); }
			set { SetValue(HideOnEmptyProperty, value); }
		}

		static void LabelsEditorControl_MessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			var control = (LabelsEditorControl) d;
			
			control.Reset();
		}

		static void LabelsEditorControl_HideOnEmptyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			var control = (LabelsEditorControl)d;
			var hide = (bool) args.NewValue;

			if (hide)
			{
				if (control.Message == null || control.Message.LabelsList.Count(l => l.LabelType == LabelType.Custom) == 0)
					control.Visibility = Visibility.Collapsed;
			}
		}		

		#endregion

		#region Fields

		protected Message _message;		
		protected Flipper _flipper;		
		protected TextPointer contentEnd;
		private IEventReg subscription;

		#endregion

		#region Properties

        public UIElement FocusElement
        {
            get { return Editor; }
        }

		public bool IsPopupOpen
		{
			get { return AutoCompletionPopup.IsOpen; }
		}

		public bool HasLightBackground
		{
			set
			{
				if (value)
				{
					Editor.WatermarkStyle = (Style)FindResource("LabelsDarkWaterMarkLabel");
					Editor.Background = Brushes.White;
					Editor.Foreground = Brushes.Black;					
				}
			}
		}

		#endregion

		#region Construction

		public LabelsEditorControl()
		{
			InitializeComponent();

		    InputCommands.Stub(this);

			_flipper = new Flipper(TimeSpan.FromMilliseconds(400), CheckExistingLabel);

			// If labels are updated from elsewhere in the message we are currently viewing
			// (and we are not the one causing the updated), rebuild the labels
			subscription = EventBroker.Subscribe(AppEvents.MessageLabelsUpdated,
				(Message message) => Thread.CurrentThread.ExecuteOnUIThread(delegate
	              	{
	              		if (message == Message && !Editor.IsFocused)
	              			Reset();
	              	}));
		}

		#endregion

		#region Methods

		public List<LabelsContainer> GetVisualLabels()
		{
			var walker = new DocumentWalker();
			var labels = new List<LabelsContainer>();

			walker.VisualVisited += delegate(object sender, object visitedObject, bool start)
			{
				if (visitedObject is ContentControl)
				{
					var contentControl = (ContentControl)visitedObject;

					if (contentControl.Content is LabelsContainer)
						labels.Add((LabelsContainer) contentControl.Content);
				}
			};

			walker.Walk(Editor.Document);

			return labels;
		}

		internal void Reset()
		{
			// Clear textbox contents
			Editor.Document.Blocks.Clear();
			Editor.ShowWatermark = true;

			if (Message != null &&
				Message.LabelsList.Any(l => l.LabelType == LabelType.Custom))
			{
				Message.LabelsList
					.Where(l => l.LabelType == LabelType.Custom)
					.Select(s => new LabelsContainer(s.Labelname))
					.ForEach(s => AddVisualLabel(s, false));

				if (Visibility == Visibility.Collapsed)
					Visibility = Visibility.Visible;
			}
		}

		void CheckExistingLabel()
		{
			TextRange range = WordBreaker.GetWordRange(Editor.CaretPosition);

			string text = range.Text.Trim();

			if (VirtualMailBox.VirtualMailBox.Current.Labels.ContainsKey(text))
				AddVisualLabel(new LabelsContainer(text), true);
		}

		void ProcessCurrentWord()
		{
			TextRange range = WordBreaker.GetWordRange(Editor.CaretPosition);

			string text = range.Text.Trim();

			if (!String.IsNullOrEmpty(text))
				AddVisualLabel(new LabelsContainer(text), true);
		}		

		bool CanInsertSelectedVisualLabel()
		{
			var label = AutoCompletionListBox.SelectedItem as LabelsContainer;

			return (label != null);
		}

		void InsertSelectedVisualLabel()
		{
			// Get selected contact
			var label = AutoCompletionListBox.SelectedItem as LabelsContainer;

			if (label == null) return;

			AddVisualLabel(label, true);
		}

		public void AddVisualLabel(LabelsContainer source, bool addToMessage)
		{			
			// Create display object
			var ctrl = new ContentControl { Content = source };
			var container = new InlineUIContainer(ctrl, Editor.CaretPosition);

			contentEnd = container.ContentEnd;

			if (addToMessage && Message != null)
			{
				Message.AddLabel(new Label(source.Labelname));
			}

			// Remove any typed text
			WordBreaker.GetWordRange(Editor.CaretPosition).Text = String.Empty;

			// Move caret to end of what was the word
			Editor.CaretPosition = contentEnd;
			Editor.ShowWatermark = false;

			// Hide dropdown list if it is visible
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
		void ShowList(IEnumerable<LabelsContainer> results, Rect pos)
		{
			//var screenPos = Editor.TranslatePoint(new Point(), this);

			//Canvas.SetLeft(AutoCompletionListBox, pos.X);
			//Canvas.SetTop(AutoCompletionListBox, screenPos.Y + 20);

			AutoCompletionListBox.ItemsSource = results;
			AutoCompletionPopup.IsOpen = true;
		}

		/// <summary>
		/// Rebuilds the recipients list.
		/// </summary>
		public void DeleteMissingVisualLabels()
		{
			if (Message == null) return;

			var simpleLabels = new List<LabelsContainer>();
			var walker = new DocumentWalker();

			walker.VisualVisited += delegate(object sender, object visitedObject, bool start)
		    	{
		    		if (visitedObject is ContentControl)
		    		{
		    			var contentControl = (ContentControl)visitedObject;

						if (contentControl.Content is LabelsContainer)
							simpleLabels.Add((LabelsContainer)contentControl.Content);
		    		}
		    	};

			walker.Walk(Editor.Document);

			// Check if there are any labels that need to be removed
			var remove = (from label in Message.LabelsList
			              where !simpleLabels.Any(l => l.Labelname == label.Labelname)
			              select label).ToList();

			foreach (var label in remove)
				Message.RemoveLabel(label, true);

			Editor.ShowWatermark = Message.LabelsList.Count == 0;
		}

		#endregion

		#region Event handlers

		void AutoCompletionListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			var label = (e.OriginalSource as DependencyObject)
				.FindListViewItem<LabelsContainer>(AutoCompletionListBox.ItemContainerGenerator);

			if (label != null)
				AddVisualLabel(label, true);
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
				InsertSelectedVisualLabel();
				e.Handled = true;
			}
		}

		void Editor_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			_flipper.Delay();

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

				case Key.Escape:
					// Restore state before we opened the list
                    if (AutoCompletionPopup.IsOpen)
					    HideList();
                    else
                        EventBroker.Publish(AppEvents.RequestFocus);

					break;

				case Key.Tab:
				case Key.Enter:
				case Key.Space:
					if (IsPopupOpen && CanInsertSelectedVisualLabel())
						InsertSelectedVisualLabel();
					else
						ProcessCurrentWord();

					e.Handled = true;

					break;
			}			
		}		

		void Editor_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Back:
				case Key.Delete:
					DeleteMissingVisualLabels();

					break;

				case Key.C:
				case Key.V:
				case Key.X:
					if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
						DeleteMissingVisualLabels();

					break;
			}
		}

		void Editor_KeyUp(object sender, KeyEventArgs e)
		{
			TextRange tr = WordBreaker.GetWordRange(Editor.CaretPosition);

			// Make sure we have at least two chars
			if (tr.Text.Length >= 2)
			{
				var results = VirtualMailBox.VirtualMailBox.Current.Labels.Keys
					.Where(k => k.IndexOf(tr.Text, StringComparison.InvariantCultureIgnoreCase) > -1)
					.Select(k => new LabelsContainer(k))
					.ToList();

				var prevResults = AutoCompletionListBox.ItemsSource as List<LabelsContainer>;

				if (prevResults != null && prevResults.Count == results.Count)
					return;

				// No result, hide list (if it was allready shown)
				if (results.Count > 0)
				{
					// Get position of caret
					var pos = tr.Start.GetCharacterRect(LogicalDirection.Forward);

					ShowList(results, pos);

					return;
				}
			}

			// Nothing to show, hide the list
			HideList();
		}

		void LabelsEditorControl_OnUnloaded(object sender, RoutedEventArgs e)
		{
			EventBroker.Unregister(subscription);
		}

		void UserControl_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
		    FocusHelper.Focus(Editor);

			// Move caret to end of editor
			Editor.CaretPosition = Editor.Document.ContentEnd;
		}

		void UserControl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			TextRange tr = WordBreaker.GetWordRange(Editor.CaretPosition);

			// Clear any text which might have not been processed yet
			tr.Text = String.Empty;

			HideList();

			if (HideOnEmpty && Message.LabelsList.Count(l => l.LabelType == LabelType.Custom) == 0)
				Visibility = Visibility.Collapsed;
		}      

		#endregion		
	}
}
