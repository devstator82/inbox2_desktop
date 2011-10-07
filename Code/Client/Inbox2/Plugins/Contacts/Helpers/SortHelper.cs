using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inbox2.Framework;

namespace Inbox2.Plugins.Contacts.Helpers
{
	public class SortHelper : SortHelperBase
	{
		private bool firstname;
		private bool lastname;

		[SortOnProperty("Firstname")]
		public bool Firstname
		{
			get { return firstname; }
			set
			{
				firstname = value;

				PerformSort();
			}
		}

		[SortOnProperty("Lastname")]
		public bool Lastname
		{
			get { return lastname; }
			set
			{
				lastname = value;

				PerformSort();
			}
		}

		protected override string SettingsKeyPrefix
		{
			get { return "/Settings/Plugins/Contacts/UI/"; }
		}

		public SortHelper(CollectionViewSource source) : base(source)
		{
			// default sort
			Firstname = true;
			Ascending = true;
		}
	}
}
