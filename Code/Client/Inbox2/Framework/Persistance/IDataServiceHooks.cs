using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Persistance
{
	public interface IDataServiceHooks
	{
		void BeforeSave();

		void AfterSave();

		void BeforeUpdate();

		void AfterUpdate();

		void BeforeDelete();

		void AfterDelete();
	}
}
