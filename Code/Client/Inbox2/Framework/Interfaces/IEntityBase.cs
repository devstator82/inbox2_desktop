using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces
{
	public interface IEntityBase : INotifyPropertyChanged, IObjectIdentity
	{
		void UpdateProperty(string propertyName);
	}
}
