using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_1_0_7 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.1.0.7"); }
        }

        protected override void UpgradeCore()
        {
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column SSL text");
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column IsDefault text");
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column OutgoingHostname text");
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column OutgoingPort text");
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column OutgoingUsername text");
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column OutgoingPassword text");
            ClientState.Current.DataService.ExecuteNonQuery("alter table ChannelConfigs add column OutgoingSSL text");
        }
    }
}
