using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Core.Configuration;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;

namespace Inbox2.Core.Search
{
	public static class SearchUtil
	{
		public static void InitializeSearchStore()
		{
			string index = Path.Combine(DebugKeys.DefaultDataDirectory, "search");

			if (IndexReader.IndexExists(index) == false)
			{
				IndexWriter writer = new IndexWriter(index, new StandardAnalyzer(), true);

				writer.Close();
			}
		}
	}
}
