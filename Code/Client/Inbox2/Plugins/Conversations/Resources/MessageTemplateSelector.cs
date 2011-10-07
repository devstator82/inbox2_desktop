using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Plugins.Conversations.Resources
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UnreadQuickMessageTemplate { get; set; }
        public DataTemplate UnreadEmailMessageTemplate { get; set; }
        public DataTemplate ReadQuickMessageTemplate { get; set; }
        public DataTemplate ReadEmailMessageTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
            Message conversation = (Message)item;

            if (conversation.ProcessingHints == ProcessingHints.QuickMessage)
            {
                return conversation.IsSet(EntityStates.Read) ? ReadQuickMessageTemplate : UnreadQuickMessageTemplate;
            }
            else
            {
                return conversation.IsSet(EntityStates.Read) ? ReadEmailMessageTemplate : UnreadEmailMessageTemplate;
            }
		}
	}
}
