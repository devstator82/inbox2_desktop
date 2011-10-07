using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inbox2.Framework;

namespace Inbox2.Plugins.Notes.Helpers
{
    public class SortHelper : SortHelperBase
    {
        private bool date;
        private bool readOrUnread;
        private bool sender;

        [SortOnProperty("DateCreated")]
        public bool Date
        {
            get { return date; }
            set
            {
                date = value;

                PerformSort();
            }
        }

        [SortOnProperty("NoteState")]
        public bool ReadOrUnread
        {
            get { return readOrUnread; }
            set
            {
                readOrUnread = value;

                PerformSort();
            }
        }

        protected override string SettingsKeyPrefix
        {
            get { return "/Settings/Plugins/Notes/UI/"; }
        }

        public SortHelper(CollectionViewSource source)
            : base(source)
        {
            // Default behavior
            date = true;
            descending = true;
        }
    }
}
