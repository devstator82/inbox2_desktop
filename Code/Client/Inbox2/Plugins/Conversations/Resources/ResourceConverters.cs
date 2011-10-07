using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.VirtualMailBox.View;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.ComponentModel;

namespace Inbox2.Plugins.Conversations.Resources
{
	// Message converters

	#region MessageExpandedConverter

	public class MessageExpandedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return false;

			Message message = (Message)value;

			return true;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion	

	#region MessageMarginConverter

	public class MessageMarginConverter : IValueConverter
	{
		private int i = 0;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return i%2 == 0 ? new Thickness() : new Thickness(50, 0, 0, 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new ApplicationException();
		}
	}

	#endregion

	// Template selectors

	#region MessageTemplateSelector

	public class MessageTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ActivityMessageTemplate { get; set; }

		public DataTemplate OneLineMessageTemplate { get; set; }

		public DataTemplate FakeMessageTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item == null)
				return FakeMessageTemplate;

			var message = (Message) item;

			if (message.MessageId == -1)
				return FakeMessageTemplate;

			return ViewFilter.Current.Filter.IsActivityViewVisible ? ActivityMessageTemplate : OneLineMessageTemplate;
		}
	}

	public class DocumentTemplateSelector : DataTemplateSelector
	{
		public DataTemplate CollapsedTemplate { get; set; }

		public DataTemplate ExpandedTemplate { get; set; }

		public bool IsExpanded { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			return IsExpanded ? ExpandedTemplate : CollapsedTemplate;
		}
	}

	#endregion

	// MailAddress converters

	#region MailAddressConverter

	public class MailAddressConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SourceAddress address = (SourceAddress)value;

			if (String.IsNullOrEmpty(address.DisplayName))
				return address.Address;

			return String.Format("{0} <{1}>", address.DisplayName, address.Address);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SourceAddress address = new SourceAddress(value.ToString());

			return address;
		}
	}

	#endregion

	#region MailAddressCollectionConverter

	public class MailAddressCollectionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return String.Empty;

			SourceAddressCollection collection = (SourceAddressCollection)value;

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < collection.Count; i++)
			{
				sb.Append(collection[i].ToString());

				if (i < collection.Count - 1)
					sb.Append("; ");
			}

			return sb.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var converter = new SourceAddressCollectionConverter();

			return converter.ConvertFrom(value);
		}
	}

	#endregion	
}
