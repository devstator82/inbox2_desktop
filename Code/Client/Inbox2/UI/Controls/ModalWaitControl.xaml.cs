using System;
using System.Windows.Controls;

namespace Inbox2.UI.Controls
{
	/// <summary>
	/// Interaction logic for ModalWaitControl.xaml
	/// </summary>
	public partial class ModalWaitControl : UserControl
	{
		public string Message
		{
			get { return MessageTextBlock.Text; }
			set { MessageTextBlock.Text = value; }
		}

		public ModalWaitControl()
		{
			InitializeComponent();
		}
	}
}
