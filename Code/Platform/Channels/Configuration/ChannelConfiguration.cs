using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Platform.Channels.Configuration
{
	[Serializable]
	public abstract class ChannelConfiguration
	{
		private bool displayEnabled;
		private bool isEnabled;

		protected Channel InnerInputChannel { get; set; }
		protected Channel InnerOutputChannel { get; set; }
		protected Channel InnerContactsChannel { get; set; }
		protected Channel InnerCalendarChannel { get; set; }
		protected Channel InnerStatusUpdatesChannel { get; set; }

		public abstract string DisplayName { get; }
		
		public virtual bool DisplayEnabled
		{
			get { return displayEnabled; }
			set { displayEnabled = value; }
		}

		public virtual bool IsEnabled
		{
			get { return isEnabled; }
			set { isEnabled = value; }
		}

		public virtual long ChannelId { get; set; }

		public virtual string ChannelKey { get; set; }

		public virtual bool IsCustomized { get; set; }

		public virtual bool IsConnected { get; set; }

		public virtual bool IsDefault { get; set; }
	   
		public virtual ChannelCharasteristics Charasteristics
		{
			get { return ChannelCharasteristics.Default; }
		}

		[XmlIgnore]
		public virtual string DefaultDomain
		{
			get { return String.Empty; }
		}

		[XmlIgnore]
		public virtual DisplayStyle DisplayStyle
		{
			get { return DisplayStyle.Simple; }
		}

		[XmlIgnore]
		public virtual int PreferredSortOrder
		{
			get { return 100; }
		}		

		public virtual Channel InputChannel
		{
			get { return InnerInputChannel; }
			set { InnerInputChannel = value; }
		}

		public virtual Channel OutputChannel
		{
			get { return InnerOutputChannel; }
			set { InnerOutputChannel = value; }
		}

		public virtual Channel ContactsChannel
		{
			get { return InnerContactsChannel; }
			set { InnerContactsChannel = value; }
		}

		public virtual Channel CalendarChannel
		{
			get { return InnerCalendarChannel; }
			set { InnerCalendarChannel = value; }
		}

		public virtual Channel DocumentsChannel
		{
			get { return InnerCalendarChannel; }
			set { InnerCalendarChannel = value; }
		}

		public virtual Channel StatusUpdatesChannel
		{
			get { return InnerStatusUpdatesChannel; }
			set { InnerStatusUpdatesChannel = value; }
		}

		public virtual IWebRedirectBuilder RedirectBuilder
		{
			get { return null; }
		}

		public virtual ChannelProfileInfoBuilder ProfileInfoBuilder
		{
			get { return new ChannelProfileInfoBuilder(); }
		}

		protected ChannelConfiguration()
		{
			// Channels are visible per default
			displayEnabled = true;
			isEnabled = true;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(DisplayName);

			if (InnerInputChannel != null && InnerInputChannel.Authentication != null
				&& !String.IsNullOrEmpty(InnerInputChannel.Authentication.Username))
					sb.AppendFormat(" ({0})", InnerInputChannel.Authentication.Username);

			return sb.ToString();
		}		

		public abstract ChannelConfiguration Clone();
	}
}