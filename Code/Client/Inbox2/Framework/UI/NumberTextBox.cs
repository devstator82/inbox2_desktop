using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inbox2.Framework.UI
{
	/// <summary>
	/// Based on this article: http://dotnetus.spaces.live.com/Blog/cns!4E39ECD492E4EEC1!550.entry
	/// </summary>
	public class NumberTextBox : TextBox
	{
		public bool AllowSeperators { get; set; }

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			e.Handled = !AreAllValidNumericChars(e.Text);

			base.OnPreviewTextInput(e);
		} 

		bool AreAllValidNumericChars(string str)
		{
			if (AllowSeperators)
			{
				if (str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentSymbol |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.PerMilleSymbol |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol |
				    str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
						return true;
			}

			bool ret = true;

			for (int i = 0; i < str.Length; i++)
				ret &= Char.IsDigit(str[i]);

			return ret; 
		}
	}
}
