using System;
using System.ComponentModel;
using System.Windows;

namespace Inbox2.UI.Windows
{
	/// <summary>
	/// Interaction logic for PopupWindow.xaml
	/// </summary>
	public partial class PopupWindow : Window
	{
		private bool forceClose;

		public PopupWindow(UIElement child)
		{
			InitializeComponent();

			Content = child;
		}

		public void ForceClose()
		{
			forceClose = true;

			Close();
		}

		void Window_Closing(object sender, CancelEventArgs e)
		{
			// Prevents people from closing window with alt+f4 and such
			if (!forceClose)
				e.Cancel = true;
		}
	}
}
