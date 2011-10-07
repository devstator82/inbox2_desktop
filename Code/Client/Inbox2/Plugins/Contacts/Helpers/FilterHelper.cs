using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inbox2.Framework;

namespace Inbox2.Plugins.Contacts.Helpers
{
	public class FilterHelper : FilterHelperBase
	{
		private string searchKeyword;

		public bool ShowSoftContacts { get; set; }

		public string SearchKeyword
		{
			get { return searchKeyword; }
			set
			{
				searchKeyword = value;

				// If we are searching we will also show soft contacts
				ShowSoftContacts = !String.IsNullOrEmpty(searchKeyword);

				RefreshView();
			}
		}

		protected override string SettingsKeyPrefix
		{
			get { return "/Settings/Plugins/Contacts/UI/"; }
		}

		public FilterHelper(CollectionViewSource view)
			: base(view)
		{
			// defaults
			ShowSoftContacts = false;
		}
	}
}
