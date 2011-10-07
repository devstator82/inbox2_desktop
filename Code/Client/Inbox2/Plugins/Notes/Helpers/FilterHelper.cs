using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inbox2.Framework;

namespace Inbox2.Plugins.Notes.Helpers
{
    public class FilterHelper : FilterHelperBase
    {
        private bool received;
        private bool sent;
        private bool deleted;
        private bool concepts;

        public bool Received
        {
            get { return received; }
            set
            {
                received = value;

                RefreshView();
            }
        }

        public bool Sent
        {
            get { return sent; }
            set
            {
                sent = value;

                RefreshView();
            }
        }

        public bool Deleted
        {
            get { return deleted; }
            set
            {
                deleted = value;

                RefreshView();
            }
        }

        public bool Concepts
        {
            get { return concepts; }
            set
            {
                concepts = value;

                RefreshView();
            }
        }

        protected override string SettingsKeyPrefix
        {
            get { return "/Settings/Plugins/Notes/UI/"; }
        }

        public FilterHelper(CollectionViewSource view)
            : base(view)
        {
            received = true;
            sent = true;
            concepts = true;
        }
    }
}
