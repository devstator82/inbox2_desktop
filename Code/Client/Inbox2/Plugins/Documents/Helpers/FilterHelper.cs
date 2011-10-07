using System;
using System.Collections.Generic;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Plugins.Documents.Helpers
{
    public class FilterHelper : FilterHelperBase
    {
        private string searchKeyword;
    	private readonly List<long> searchResults;

        public string SearchKeyword
        {
            get { return searchKeyword; }
            set
            {
                searchKeyword = value;

				searchResults.Clear();
				
				if (!String.IsNullOrEmpty(searchKeyword))					
					searchResults.AddRange(ClientState.Current.Search.PerformSearch<Document>(SearchKeyword));

                RefreshView();
            }
        }
		
        protected override string SettingsKeyPrefix
        {
            get { return "/Settings/Plugins/Contacts/UI/"; }
        }
    	
		public bool IsInSearchMode
    	{
			get { return !String.IsNullOrEmpty(SearchKeyword); }
    	}

		public bool IsSearchVisible(Document document)
		{
			return searchResults.Contains(document.DocumentId ?? 0);
		}

        public FilterHelper(CollectionViewSource view)
            : base(view)
        {
			searchResults = new List<long>();
        }
    }
}
