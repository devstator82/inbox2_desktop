using System;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_4_3_0 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.4.3.0"); }
        }

        protected override void UpgradeCore()
        {
			ClientState.Current.DataService.ExecuteNonQuery("update ChannelConfigs set IsVisible='True'");
        }
    }
}