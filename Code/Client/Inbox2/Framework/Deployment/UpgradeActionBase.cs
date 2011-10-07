using System;
using System.Collections.Generic;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework.Deployment
{
	public abstract class UpgradeActionBase
	{
		private static readonly List<UpgradeActionBase> upgrades = new List<UpgradeActionBase>();

		public static List<UpgradeActionBase> Upgrades
		{
			get { return upgrades; }
		}

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

		public virtual void AfterLoadUpgradeAsync()
		{
			
		}

		protected abstract void UpgradeCore();
	}
}
