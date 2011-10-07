using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.ComponentModel;
using Inbox2.Platform.Framework.Composition;

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

	#region MessageOpacityConverter

	public class MessageOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return false;

			Message message = (Message)value;

			return message.IsSet(EntityStates.Read) ? 0.58 : 1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// Template selectors

	#region MessageTemplateSelector

	public class MessageTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ActivityMessageTemplate { get; set; }

		public DataTemplate OneLineMessageTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			var state = PluginsManager.Current.GetState<ConversationsState>();

			return state.IsActivityViewVisible ? ActivityMessageTemplate : OneLineMessageTemplate;
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

	#region SourceAddressToContactConverter

	public class SourceAddressToContactConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SourceAddress address = (SourceAddress)value;

			IContactsResolver resolver = ObjectComposer.GetObject<IContactsResolver>();

			return resolver.Find(address);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion
}
