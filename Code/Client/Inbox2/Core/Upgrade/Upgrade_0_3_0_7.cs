using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_3_0_7 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.3.0.7"); }
        }

        protected override void UpgradeCore()
        {
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column [IsManuallyCustomized] text");
			ClientState.Current.DataService.ExecuteNonQuery("alter table Messages add column [SendLabels] text");
			ClientState.Current.DataService.ExecuteNonQuery("update Messages set [SendLabels]=[Labels] where [SourceFolder]='INBOX' and [Labels] != ''");			
        }
    }
}