using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
	public class upgrade_0_1_0_0 : UpgradeActionBase
	{
		public override Version TargetVersion
		{
			get { return new Version("0.1.0.1"); }
		}

		protected override void UpgradeCore()
		{
			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE Messages ADD COLUMN IsStarred TEXT");
			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE UserStatys ADD COLUMN TargetChannelId TEXT");
		}
	}
}
