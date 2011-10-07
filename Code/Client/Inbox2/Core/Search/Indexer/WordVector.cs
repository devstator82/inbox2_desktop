using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Core.Search.Indexer
{
	class WordVector
	{
		private string word;
		private long count;
		private bool isDirty;
		private Dictionary<string, long> entities;

		public string Word
		{
			get { return word; }
		}

		public long Count
		{
			get { return count; }
		}

		public bool IsDirty
		{
			get { return isDirty; }
		}

		public Dictionary<string, long> Entities
		{
			get { return entities; }
		}

		public WordVector(string word)
		{
			this.word = word;
			this.entities = new Dictionary<string, long>();
		}

		public void AddWordCount(string entityKey, int wordCount)
		{
			count += wordCount;
			isDirty = true;

			if (entities.ContainsKey(entityKey))
				entities.Remove(entityKey);
				
			entities.Add(entityKey, wordCount);
		}
	}
}
