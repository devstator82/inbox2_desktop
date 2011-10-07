using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Upgrade
{
	public abstract class UpgradeActionBase
	{
		public abstract Version TargetVersion { get; }

		public void Upgrade()
		{
			try
			{
				Logger.Debug("Running upgrade {0}", LogSource.Startup, GetType());

				UpgradeCore();

				Logger.Debug("Upgrade {0} finished successfully", LogSource.Startup, GetType());
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while running upgrade {0}, Exception = {1}", LogSource.Startup, GetType(), ex);
			}
		}

		protected abstract void UpgradeCore();
	}
}
