using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Core.Configuration
{
	public class AppConfiguration
	{
        public string ClientId { get; set; }

		public bool IsChannelSetupFinished { get; set; }

		public string AuthToken { get; set; }
		public string AvatarUrl { get; set; }
		public string Username { get; set; }
		public string Fullname { get; set; }

		public bool IsJustRegistered { get; set; }
		public bool IsFirstStatusUpdate { get; set; }
		public bool ShowBanner { get; set; }

        public int SurveysDone { get; set; }
        public DateTime? DateSurveyDone { get; set; }
        public DateTime? DateSurveySnoozed { get; set; }

		public bool IsStatsDisabled { get; set; }
		public bool IsDefaultMailClientCheckEnabled { get; set; }

		public bool IgnoreSslCertificateIssues { get; set; }

		public bool IsLoadingAllMessages { get; set; }

		public bool RollUpConversations { get; set; }

		public string DefaultView { get; set; }

		public PreviewPaneLocation PreviewPaneLocation { get; set; }
		public bool ShowProfileBalloons { get; set; }
		public bool ShowStreamColumn { get; set; }
        public bool ShowProductivityColumn { get; set; }
		public bool ShowLabelsColumn { get; set; }

        public string Signature { get; set; }
		public string ConfigHash { get; set; }

		public bool MarkReadWhenViewing { get; set; }
		public int? MarkReadWhenViewingAfter { get; set; }

		public bool ShowNotificationsPopup { get; set; }
		public int ShowNotificationsPopupFor { get; set; }
		public bool PlayNotificationsSound { get; set; }

		public bool MinimizeToTray { get; set; }

		public DateTime LastSyncDate { get;set; }

		public ReceiveConfiguration ReceiveConfiguration { get; set; }        

		public AppConfiguration()
		{
            ClientId = Guid.NewGuid().GetHash(12);
			IsJustRegistered = true;
			IsDefaultMailClientCheckEnabled = true;
			RollUpConversations = true;
			ShowProfileBalloons = true;
			ShowStreamColumn = true;
			PreviewPaneLocation = PreviewPaneLocation.Hidden;
			ReceiveConfiguration = new ReceiveConfiguration();
			MarkReadWhenViewing = true;
			MarkReadWhenViewingAfter = 3;
			ShowNotificationsPopup = true;
			ShowNotificationsPopupFor = 5;
			PlayNotificationsSound = true;
			IsFirstStatusUpdate = true;
			ShowLabelsColumn = true;
			DefaultView = "Single line view";
		}
	}
}
