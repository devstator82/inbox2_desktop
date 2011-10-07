using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Framework.Threading
{
	[DataContract]
	public abstract class CommandBase
	{
		public virtual int MaxRetryCount
		{
			get { return 3; }
		}

		public virtual bool CanExecute
		{
			get { return true; }
		}

		protected abstract void ExecuteCore();

		public void Execute()
		{
			ClientState.Current.Messages.Errors.Clear();

			ExecuteCore();
		}
	}
}
