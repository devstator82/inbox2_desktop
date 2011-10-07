using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Inbox2.Framework.UI
{
	/// <summary>
	/// From: http://fortes.com/2007/03/20/bindablerun/
	/// </summary>
	public class BindableRun : Run
	{
		public static readonly DependencyProperty BoundTextProperty =
			DependencyProperty.Register("BoundText", typeof(string),
			typeof(BindableRun),
			new PropertyMetadata(
				new PropertyChangedCallback(OnBoundTextChanged)));

		private static void OnBoundTextChanged(DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			((Run)d).Text = e.NewValue as string;
		}

		public string BoundText
		{
			get { return GetValue(BoundTextProperty) as string; }
			set { SetValue(BoundTextProperty, value); }
		}
	}
}
