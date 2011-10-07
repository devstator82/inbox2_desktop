using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.VirtualMailBox
{
	public static class EntityFactory<T>
	{
		private static Func<long, T> factoryFunc;

		public static void RegisterFactory(Func<long, T> func)
		{
			factoryFunc = func;
		}

		public static T Build(long key)
		{
			if (factoryFunc == null)
				throw new ApplicationException(String.Format("Factory function for type {0} has not been registered", typeof(T)));

			return factoryFunc(key);
		}
	}
}
