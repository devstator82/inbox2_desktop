using Inbox2.Framework;
using System.Windows.Data;

namespace Inbox2.Plugins.Calendar.Helpers
{
    public class SortHelper : SortHelperBase
    {
        private bool startdate;

        //TODO CalendarPlugin: Is this the right naming for the property?
        [SortOnProperty("StartDate")]
        public bool StartDate
        {
            get { return startdate; }
            set
            {
                startdate = value;

                PerformSort();
            }
        }

        protected override string SettingsKeyPrefix
        {
            //TODO CalendarPlugin: SettingsKeyPrefix, is this naming right?
            get { return "/Settings/Plugins/Calendar/UI/"; }
        }

        public SortHelper(CollectionViewSource source) : base(source)
        {
            startdate = true;
            descending = true;
        }
    }
}
