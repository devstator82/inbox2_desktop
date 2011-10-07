using System.Windows.Data;
using Inbox2.Framework;

namespace Inbox2.Plugins.Calendar.Helpers
{
    public class FilterHelper : FilterHelperBase
    {
        private bool deleted;
        private bool received;

        public bool Deleted
        {
            get { return deleted; }
            set
            {
                deleted = value;

                RefreshView();
            }
        }
        public bool Received
        {
            get { return received; }
            set
            {
                received = value;

                RefreshView();
            }
        }		

        protected override string SettingsKeyPrefix
        {
            // TODO : SettingsKeyPrefix, is this naming right???
            get { return "/Settings/Plugins/Calendar/UI/"; }
        }

        public FilterHelper(CollectionViewSource view) : base(view)
        {
            deleted = false;
            received = true;
        }
    }
}
