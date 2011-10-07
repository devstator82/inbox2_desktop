using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_4_0_0 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.4.0.0"); }
        }

        protected override void UpgradeCore()
        {
			ClientState.Current.DataService.ExecuteNonQuery("alter table Persons add column [RedirectPersonId] integer");
        }
    }
}