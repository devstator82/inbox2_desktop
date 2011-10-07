using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using Inbox2.Core.Configuration;
using Inbox2.Core.Search.Reflection;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces;
using Inbox2.Platform.Framework.Locking;
using Inbox2.Platform.Interfaces;
using Inbox2.Platform.Logging;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace Inbox2.Core.Search
{
	[Export(typeof(ISearch))]
	public class Search : ISearch, ISynchronizedObject
	{
		private readonly string path;
		private readonly Analyzer analyzer;
		private int? maxResults = null;

		public Search()
		{
			this.path = Path.Combine(DebugKeys.DefaultDataDirectory, "search");
			analyzer = new StandardAnalyzer();
		}

		/// <summary>
		/// Stores the specified source.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		public void Store<T>(T source)
		{
			IContentMapper[] mappers = Reflector.GetMappers(source).ToArray();

			var doc = SearchHelper.CreateDocument(source, mappers);

			if (doc.GetFieldsCount() == 0)
			{
				Logger.Debug("Document did not contain any fields, ignoring add", LogSource.Search);

				return;
			}

			using (WriterLock)
			{
				// Add document to the index
				IndexWriter writer = new IndexWriter(path, analyzer, false);
				writer.AddDocument(doc);
				writer.Close();
			}
		}

		/// <summary>
		/// Delete the given item from the search index.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		public void Delete<T>(T source)
		{
			var pk = Reflector.GetPrimaryKeyInstance<T>();

			if (pk != null)
			{
				var value = pk.GetValue(source, null);

				using (WriterLock)
				{
					// Add document to the index
					IndexReader reader = IndexReader.Open(path);
					reader.DeleteDocuments(new Term(pk.Name, value.ToString()));
					reader.Close();
				}
			}
		}

		public IEnumerable<long> PerformSearch<T>(string searchQuery) where T : new()
		{
			Query query = SearchHelper.BuildQuery<T>(searchQuery, analyzer);
			string primary = Reflector.GetPrimaryKey<T>();

			using (ReaderLock)
			{
				IndexSearcher searcher = new IndexSearcher(path);

				Hits hits = searcher.Search(query);
				int count = hits.Length();

				try
				{
					for (int i = 0; i < count; i++)
					{
						if (maxResults.HasValue && i > maxResults)
							yield break;

						var doc = hits.Doc(i);

						Field primaryId = doc.GetField(primary);

						if (primaryId != null)
						{
							var value = primaryId.StringValue();

							if (!String.IsNullOrEmpty(value))
								yield return Int64.Parse(primaryId.StringValue());
						}
					}
				}
				finally
				{
					searcher.Close();	
				}				
			}
		}

		public IEnumerable<long> PerformRelatedSearch(string searchQuery)
		{
			//var b = Clustering.HCluster(MemoryIndex.Current.Words.Values);

			yield break;
		}

		#region Locking implementation

		protected ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

		public void AcquireReaderLock()
		{
			_lock.EnterReadLock();
		}

		public void ReleaseReaderLock()
		{
			_lock.ExitReadLock();
		}

		public void AcquireWriterLock()
		{
			_lock.EnterWriteLock();
		}

		public void ReleaseWriterLock()
		{
			_lock.ExitWriteLock();
		}

		public static ReaderLock ReaderLock
		{
			get
			{
				return new ReaderLock(ClientState.Current.Search as ISynchronizedObject);
			}
		}

		public static WriterLock WriterLock
		{
			get
			{
				return new WriterLock(ClientState.Current.Search as ISynchronizedObject);
			}
		}

		#endregion	
	}
}
