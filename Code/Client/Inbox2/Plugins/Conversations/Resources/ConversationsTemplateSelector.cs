using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins.Entities;

namespace Inbox2.Plugins.Conversations.Resources
{
	public class ConversationsTemplateSelector : DataTemplateSelector
	{
		public DataTemplate QuickMessageTemplate { get; set; }
		public DataTemplate EmailMessageTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			Conversation conversation = (Conversation)item;

			return conversation.ProcessingHints == ProcessingHints.QuickMessage ? QuickMessageTemplate : EmailMessageTemplate;
		}
	}
}
