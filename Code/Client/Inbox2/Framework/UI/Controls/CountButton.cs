using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Inbox2.Framework.UI.Controls
{
	public class CountButton : Button
	{
		public static readonly DependencyProperty CountProperty =
			DependencyProperty.Register("Count", typeof(int), typeof(CountButton), new UIPropertyMetadata(0));

		public int Count
		{
			get { return (int)GetValue(CountProperty); }
			set { SetValue(CountProperty, value); }
		}		
	}
}
