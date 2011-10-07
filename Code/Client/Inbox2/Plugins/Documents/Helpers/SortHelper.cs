using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inbox2.Framework;

namespace Inbox2.Plugins.Documents.Helpers
{
    public class SortHelper : SortHelperBase
    {
        private readonly string _prefix;
        private bool filename;
        private bool filetype;
        private bool sortdate;

        [SortOnProperty("Filename")]
        public bool Filename
        {
            get { return filename; }
            set
            {
                filename = value;

                PerformSort();
            }
        }

        [SortOnProperty("FilenameExtension")]
        public bool FileType
        {
            get { return filetype; }
            set
            {
                filetype = value;

                PerformSort();
            }
        }

        [SortOnProperty("SortDate")]
		public bool SortDate
        {
            get { return sortdate; }
            set
            {
                sortdate = value;

                PerformSort();
            }
        }

        protected override string SettingsKeyPrefix
        {
            get { return "/Settings/Plugins/Documents/UI/" + _prefix; }
        }

        public SortHelper(CollectionViewSource source, string prefix)
            : base(source)
        {
            _prefix = prefix;

            // default sort
            Filename = true;
            Ascending = true;
        }
    }
}
