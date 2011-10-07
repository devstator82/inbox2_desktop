using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_1_0_8 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.1.0.8"); }
        }

        protected override void UpgradeCore()
        {
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column Type text");
        }
    }
}
