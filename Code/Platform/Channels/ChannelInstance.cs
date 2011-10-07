using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels
{
	public class ChannelInstance : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler<EventArgs> IsVisibleChanged;

		/// <summary>
		/// Gets or sets the configuration.
		/// </summary>
		/// <value>The configuration.</value>
		public ChannelConfiguration Configuration { get; private set; }

		/// <summary>
		/// Gets or sets the input channel instance.
		/// </summary>
		/// <value>The input channel.</value>
		public IClientInputChannel InputChannel { get; private set; }

		/// <summary>
		/// Gets or sets the output channel instance.
		/// </summary>
		/// <value>The output channel.</value>
		public IClientOutputChannel OutputChannel { get; private set; }

		/// <summary>
		/// Gets or sets the contacts channel instance.
		/// </summary>
		/// <value>The contacts channel.</value>
		public IClientContactsChannel ContactsChannel { get; private set; }

		/// <summary>
		/// Gets or sets the calendar channel.
		/// </summary>
		/// <value>The calendar channel.</value>
		public IClientCalendarChannel CalendarChannel { get; private set; }

		/// <summary>
		/// Gets or sets the status updates channel.
		/// </summary>
		public IClientStatusUpdatesChannel StatusUpdatesChannel { get; private set; }

		public bool IsVisible
		{
			get { return Configuration.DisplayEnabled; }
			set
			{
				Configuration.DisplayEnabled = value;

				OnIsVisibleChanged();

				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("IsVisible"));
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ChannelInstance"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public ChannelInstance(ChannelConfiguration configuration)
		{
			OverrideConfiguration(configuration);
		}

		public void OverrideConfiguration(ChannelConfiguration configuration)
		{
			Configuration = configuration;

			BuildChannels();
		}

		/// <summary>
		/// Builds the channels.
		/// </summary>
		protected void BuildChannels()
		{
			if (Configuration.InputChannel != null)
			{
				InputChannel = ChannelBuilder.BuildWithCredentials<IClientInputChannel>(Configuration.InputChannel);
			}

			if (Configuration.OutputChannel != null)
			{
				OutputChannel = ChannelBuilder.BuildWithCredentials<IClientOutputChannel>(Configuration.OutputChannel);
			}

			if (Configuration.ContactsChannel != null)
			{
				ContactsChannel = ChannelBuilder.BuildWithCredentials<IClientContactsChannel>(Configuration.ContactsChannel);
			}

			if (Configuration.CalendarChannel != null)
			{
				CalendarChannel = ChannelBuilder.BuildWithCredentials<IClientCalendarChannel>(Configuration.CalendarChannel);
			}

			if (Configuration.StatusUpdatesChannel != null)
				StatusUpdatesChannel = ChannelBuilder.BuildWithCredentials<IClientStatusUpdatesChannel>(Configuration.StatusUpdatesChannel);
		}

		void OnIsVisibleChanged()
		{
			if (IsVisibleChanged != null)
				IsVisibleChanged(this, EventArgs.Empty);
		}
	}

	public class ChannelInstanceComparer : IEqualityComparer<ChannelInstance>
	{
		public bool Equals(ChannelInstance x, ChannelInstance y)
		{
			return x.Configuration.ChannelId.Equals(y.Configuration.ChannelId);
		}

		public int GetHashCode(ChannelInstance obj)
		{
			return obj.GetHashCode();
		}
	}
}