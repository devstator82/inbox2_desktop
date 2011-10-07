using System;
using System.Collections.Generic;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_5_0_0 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.5.0.0"); }
        }

        protected override void UpgradeCore()
        {
			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE ChannelConfigs ADD COLUMN ChannelKey TEXT");
			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE ChannelConfigs ADD COLUMN ChannelConnection TEXT");
			ClientState.Current.DataService.ExecuteNonQuery("UPDATE ChannelConfigs SET ChannelConnection='Local'");

			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE Messages ADD COLUMN ContentSynced TEXT");
			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE Documents ADD COLUMN ContentSynced TEXT");

			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE CommandQueue ADD COLUMN [Value] TEXT");
			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE CommandQueue ADD COLUMN ModifyAction TEXT");

			ClientState.Current.DataService.ExecuteNonQuery("UPDATE Messages SET ContentSynced='True'");
			ClientState.Current.DataService.ExecuteNonQuery("UPDATE Documents SET ContentSynced='True'");

			ClientState.Current.DataService.ExecuteNonQuery(@"CREATE TABLE [Messages2] (
[MessageId] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[MessageKey] TEXT  NULL,
[MessageNumber] TEXT  NULL,
[MessageIdentifier] TEXT  NULL,
[InReplyTo] TEXT  NULL,
[SourceFolder] TEXT  NULL,
[SourceChannelId] INTEGER  NULL,
[TargetChannelId] INTEGER  NULL,
[Context] TEXT  NULL,
[OriginalContext] TEXT  NULL,
[BodyPreview] TEXT  NULL,
[BodyTextStreamName] TEXT  NULL,
[BodyHtmlStreamName] TEXT  NULL,
[Size] INTEGER  NULL,
[From] TEXT  NULL,
[ReturnTo] TEXT  NULL,
[To] TEXT  NULL,
[CC] TEXT  NULL,
[BCC] TEXT  NULL,
[IsRead] TEXT  NULL,
[TargetMessageState] TEXT  NULL,
[DateRead] text  NULL,
[DateAction] text  NULL,
[DateReply] text  NULL,
[DateReceived] TEXT  NULL,
[DateSent] TEXT  NULL,
[ConversationIdentifier] TEXT  NULL,
[MessageFolder] INTEGER  NULL,
[IsStarred] TEXT  NULL,
[Labels] TEXT  NULL,
[SendLabels] TEXT  NULL,
[DateCreated] TEXT  NULL,
[ContentSynced] TEXT  NULL
)");

			ClientState.Current.DataService.ExecuteNonQuery("INSERT INTO Messages2 SELECT * FROM Messages");
			ClientState.Current.DataService.ExecuteNonQuery("DROP TABLE Messages");
			ClientState.Current.DataService.ExecuteNonQuery("ALTER TABLE Messages2 RENAME TO Messages");

			// Update profile types
			ClientState.Current.DataService.ExecuteNonQuery("UPDATE Profiles SET ProfileType='Default' where ProfileType='Home'");
			ClientState.Current.DataService.ExecuteNonQuery("UPDATE Profiles SET ProfileType='Social' where ProfileType != 'Default'");
        }		 	
    }
}