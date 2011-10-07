using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Inbox2.Core.Search
{
	static class IndexHelper
	{
		public static Dictionary<string, int> GetWordsCount(string source)
		{
			var words = new Dictionary<string, int>();

			foreach (var word in GetWords(source))
			{
				if (!words.ContainsKey(word))				
					words.Add(word, 0);

				words[word]++;
			}

			return words;
		}

		static IEnumerable<string> GetWords(string source)
		{
			MatchCollection mc = Regex.Matches(source, @"\b[A-Za-z]+\b", RegexOptions.None);

			foreach (Match ma in mc)
				yield return ma.Value.ToLower();
		}
	}
}