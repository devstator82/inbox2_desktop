using System;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_4_5_0 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.4.5.0"); }
        }

        protected override void UpgradeCore()
        {
			string query = @"
BEGIN TRANSACTION;

CREATE TABLE [Conversations_New] (
	[ConversationId] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
	[ConversationIdentifier] TEXT NULL,
	[Context] TEXT NULL
);

INSERT INTO Conversations_New SELECT ConversationId, ConversationIdentifier, Context FROM [Conversations];

DROP TABLE Conversations;
ALTER TABLE Conversations_New RENAME TO [Conversations];

COMMIT;";

			ClientState.Current.DataService.ExecuteNonQuery(query);



			string query2 = @"
BEGIN TRANSACTION;

CREATE TABLE [Persons_New] (
	[PersonId] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
	[PersonKey] TEXT  NULL,
	[RedirectPersonId] TEXT NULL,
	[SourceChannelId] INTEGER  NULL,
	[Firstname] TEXT  NULL,
	[Lastname] TEXT  NULL,
	[DateOfBirth] TEXT  NULL,
	[Locale] TEXT  NULL,
	[Gender] TEXT  NULL,
	[Timezone] TEXT  NULL,
	[DateCreated] TEXT  NULL
);

INSERT INTO Persons_New SELECT PersonId, PersonKey, RedirectPersonId, SourceChannelId, Firstname, Lastname, DateOfBirth, Locale, Gender, Timezone, DateCreated FROM [Persons];

DROP TABLE Persons;
ALTER TABLE Persons_New RENAME TO [Persons];

COMMIT;";

			ClientState.Current.DataService.ExecuteNonQuery(query2);
        }
    }
}