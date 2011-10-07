using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Inbox2.Framework.Interfaces
{
	public interface ISearch
	{
		void Store<T>(T source);

		void Delete<T>(T source);

		IEnumerable<long> PerformSearch<T>(string searchQuery) where T : new();

		IEnumerable<long> PerformRelatedSearch(string searchQuery);
	}
}
