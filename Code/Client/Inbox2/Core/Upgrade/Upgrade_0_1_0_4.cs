using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
	public class upgrade_0_1_0_4 : UpgradeActionBase
	{
		public override Version TargetVersion
		{
			get { return new Version("0.1.0.4"); }
		}

		protected override void UpgradeCore()
		{
			ClientState.Current.DataService.ExecuteNonQuery("alter table Persons add column DateCreated text");
			ClientState.Current.DataService.ExecuteNonQuery("alter table Profiles add column DateCreated text");
		}
	}
}
