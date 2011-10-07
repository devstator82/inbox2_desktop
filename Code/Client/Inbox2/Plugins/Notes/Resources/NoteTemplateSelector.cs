using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Notes.Resources
{
    class NoteTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UnreadNoteUrlTemplate { get; set; }
        public DataTemplate UnreadNoteTemplate { get; set; }
        public DataTemplate ReadNoteUrlTemplate { get; set; }
        public DataTemplate ReadNoteTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Note note = (Note)item;

            if (note == null)
                return UnreadNoteTemplate;

            if (note.ContentType == NoteTypes.Url)
                return note.IsSet(EntityStates.Read) ? ReadNoteUrlTemplate : UnreadNoteUrlTemplate;

            return note.IsSet(EntityStates.Read) ? ReadNoteTemplate : UnreadNoteTemplate;
        }
    }
}
