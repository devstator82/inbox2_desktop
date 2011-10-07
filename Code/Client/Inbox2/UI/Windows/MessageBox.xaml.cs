using System;
using System.Windows;
using Inbox2.Framework;
using Inbox2.Framework.Localization;

namespace Inbox2.UI.Windows
{
	/// <summary>
	/// Interaction logic for MessageBox.xaml
	/// </summary>
	public partial class MessageBox : Window
	{
		private readonly MessageBoxEventArgs args;

		public MessageBox(MessageBoxEventArgs args)
		{
			this.args = args;

			InitializeComponent();
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			MessageTextBlock.Text = args.Message;

			if (args.DoNotAskAgain)
			{
				DoNotAskAgainCheckBox.Visibility = Visibility.Visible;
				DoNotAskAgainCheckBox.IsChecked = args.DoNotAskAgainChecked;
				DoNotAskAgainCheckBox.Content = args.DoNotAskAgainMessage;
			}

			switch (args.Buttons)
			{
				case Inbox2MessageBoxButton.OK:
					OkButton.Visibility = Visibility.Visible;
					break;
				case Inbox2MessageBoxButton.OKCancel:
					OkButton.Visibility = Visibility.Visible;
					CancelButton.Visibility = Visibility.Visible;
					break;
				case Inbox2MessageBoxButton.YesNo:
					YesButton.Visibility = Visibility.Visible;
					NoButton.Visibility = Visibility.Visible;
					break;
				case Inbox2MessageBoxButton.YesNoCancel:
					YesButton.Visibility = Visibility.Visible;
					NoButton.Visibility = Visibility.Visible;
					CancelButton.Visibility = Visibility.Visible;
					break;
				case Inbox2MessageBoxButton.Conversation:
					ConversationButton.Visibility = Visibility.Visible;
					MessageButton.Visibility = Visibility.Visible;
					CancelButton.Visibility = Visibility.Visible;
					break;
			}
		}

		void OK_Click(object sender, RoutedEventArgs e)
		{
			args.Result = Inbox2MessageBoxResult.OK;
			args.DoNotAskAgainResult = DoNotAskAgainCheckBox.IsChecked ?? false;

			Close();
		}

		void Yes_Click(object sender, RoutedEventArgs e)
		{
			args.Result = Inbox2MessageBoxResult.Yes;
			args.DoNotAskAgainResult = DoNotAskAgainCheckBox.IsChecked ?? false;

			Close();
		}

		void No_Click(object sender, RoutedEventArgs e)
		{
			args.Result = Inbox2MessageBoxResult.No;
			args.DoNotAskAgainResult = DoNotAskAgainCheckBox.IsChecked ?? false;

			Close();
		}

		void Conversation_Click(object sender, RoutedEventArgs e)
		{
			args.Result = Inbox2MessageBoxResult.Conversation;
			args.DoNotAskAgainResult = DoNotAskAgainCheckBox.IsChecked ?? false;

			Close();
		}

		void Message_Click(object sender, RoutedEventArgs e)
		{
			args.Result = Inbox2MessageBoxResult.Message;
			args.DoNotAskAgainResult = DoNotAskAgainCheckBox.IsChecked ?? false;

			Close();
		}

		void Cancel_Click(object sender, RoutedEventArgs e)
		{
			args.Result = Inbox2MessageBoxResult.Cancel;
			args.DoNotAskAgainResult = false;

			Close();
		}
	}
}
