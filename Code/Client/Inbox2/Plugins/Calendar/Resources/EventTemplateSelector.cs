using System.Windows;
using System.Windows.Controls;
using Inbox2.Framework.Plugins.Entities;

namespace Inbox2.Plugins.Calendar.Resources
{
    public class EventTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultEventTemplate { get; set; }
        public DataTemplate QuickEditEventTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Event calendarevent = (Event)item;

            //TODO CalendarPlugin: Switch for templates
            if (calendarevent.IsEditing) return QuickEditEventTemplate;
            return DefaultEventTemplate;
            /*
            if (conversation.ProcessingHints == ProcessingHints.QuickMessage)
            {
                return conversation.IsSet(EntityStates.Read) ? ReadQuickMessageTemplate : UnreadQuickMessageTemplate;
            }
            else
            {
                return conversation.IsSet(EntityStates.Read) ? ReadEmailMessageTemplate : UnreadEmailMessageTemplate;
            }
            */
        }
    }
}
