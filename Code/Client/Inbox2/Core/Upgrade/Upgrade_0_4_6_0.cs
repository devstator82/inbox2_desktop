using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using Inbox2.Core.Upgrade.Windows;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_4_6_0 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.4.6.0"); }
        }

        protected override void UpgradeCore()
        {
			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE Messages ADD COLUMN OriginalContext TEXT");
			ClientState.Current.DataService.ExecuteNonQuery("UPDATE Messages SET OriginalContext=Context");
        }

		public override void AfterLoadUpgradeAsync()
		{
			UpgradeWindow window = null;

			Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					window = new UpgradeWindow { Owner = Application.Current.MainWindow };					
					window.Show();
				});

			try
			{
				var messages = new List<Message>(VirtualMailBox.Current.Messages);

				Thread.CurrentThread.ExecuteOnUIThread(() => window.SetMaximum(messages.Count));

				var sb = new StringBuilder();

				for (int i = 0; i < messages.Count; i++)
				{
					var message = messages[i];

					message.OriginalContext = message.Context;
					message.Context = message.Context.ToClearSubject();

					sb.AppendFormat("update Messages set OriginalContext='{0}', Context='{1}' where MessageId='{2}'; ",
						message.OriginalContext.AddSQLiteSlashes(), message.Context.AddSQLiteSlashes(), message.MessageId);
					
					if (i % 100 == 0)
					{
						try
						{
							ClientState.Current.DataService.ExecuteNonQuery(sb.ToString());
						}
						catch (Exception ex)
						{
							Logger.Error("An error occured while trying to run Upgrade_0_4_6_0/AfterLoadUpgradeAsync. Exception = {0}", LogSource.BackgroundTask, ex);
						}

						sb.Clear();
					}

					var progress = i + 1;

					Thread.CurrentThread.ExecuteOnUIThread(() => window.SetProgress(progress));
				}

				if (sb.Length > 0)
					ClientState.Current.DataService.ExecuteNonQuery(sb.ToString());
			}
			finally
			{
				Thread.CurrentThread.ExecuteOnUIThread(() => window.Close());
			}			
		}    	
    }
}