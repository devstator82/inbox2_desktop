using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Collections
{
	/// <summary>
	/// From: http://www.lexparse.com/2009/11/02/c-lambdas-never-implement-icomparer-and-iequalitycomparer-again/
	/// </summary>
	public class Comparer<T> : IComparer<T>
	{
		private Func<T, T, int> _compareFn;

		public Comparer(Func<T, T, int> fn)
		{
			_compareFn = fn;
		}

		public int Compare(T x, T y)
		{
			return _compareFn(x, y);
		}
	}

	/// <summary>
	/// From: http://www.lexparse.com/2009/11/02/c-lambdas-never-implement-icomparer-and-iequalitycomparer-again/
	/// </summary>
	public class EqualityComparer<T> : IEqualityComparer<T>
	{
		private Func<T, T, bool> _equalsFn;
		private Func<T, int> _getHashCodefn;

		public EqualityComparer(Func<T, T, bool> equalsFn, Func<T, int> getHashCodefn)
		{
			_equalsFn = equalsFn;
			_getHashCodefn = getHashCodefn;
		}

		public bool Equals(T x, T y)
		{
			return _equalsFn(x, y);
		}

		public int GetHashCode(T obj)
		{
			return _getHashCodefn(obj);
		}
	}

}
