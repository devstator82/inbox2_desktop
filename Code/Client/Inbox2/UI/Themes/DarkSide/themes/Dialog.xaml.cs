using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Inbox2.UI.Themes.DarkSide
{
	public partial class Dialog : ResourceDictionary
	{
		void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var window = (System.Windows.Window)(sender as FrameworkElement).TemplatedParent;

			window.DragMove();
		}

		void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var window = (System.Windows.Window)(sender as FrameworkElement).TemplatedParent;

			window.Close();
		}	

		void PART_CloseButton_OnClick(object sender, RoutedEventArgs e)
		{
			var window = (System.Windows.Window)(sender as FrameworkElement).TemplatedParent;
	
			window.Close();
		}
	}
}
