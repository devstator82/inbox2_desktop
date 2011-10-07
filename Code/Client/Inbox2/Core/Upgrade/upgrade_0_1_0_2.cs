using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
	public class upgrade_0_1_0_2 : UpgradeActionBase
	{
		public override Version TargetVersion
		{
			get { return new Version("0.1.0.2"); }
		}

		protected override void UpgradeCore()
		{
			ClientState.Current.DataService.ExecuteNonQuery("alter table Documents add column IsRead text");
			ClientState.Current.DataService.ExecuteNonQuery("update Documents set IsRead='True' where DocumentState = 'Read'");
			ClientState.Current.DataService.ExecuteNonQuery("update Documents set IsRead='False' where DocumentState != 'Read';");

			ClientState.Current.DataService.ExecuteNonQuery("alter table Messages add column IsRead text;");
			ClientState.Current.DataService.ExecuteNonQuery("update Messages set IsRead='True' where MessageState = 'Read'");
			ClientState.Current.DataService.ExecuteNonQuery("update Messages set IsRead='False' where MessageState != 'Read'");
		}
	}
}
