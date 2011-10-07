using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inbox2.Framework;

namespace Inbox2.Plugins.Conversations.Helpers
{
	public class SortHelper : SortHelperBase
	{	
		//[SortOnProperty("SortDate")]
		//public bool Date
		//{
		//    get { return date; }
		//    set
		//    {
		//        date = value;

		//        PerformSort();
		//    }
		//}

		protected override string SettingsKeyPrefix
		{
			get { return "/Settings/Plugins/Conversations/UI/"; }
		}

		public SortHelper(CollectionViewSource source) : base(source)
		{
		}		
	}
}