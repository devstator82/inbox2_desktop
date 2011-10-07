using System;
using System.ComponentModel;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Platform.Channels.Configuration;

namespace Inbox2.Framework.Threading.Progress
{
	public class ProgressGroup : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<EventArgs> Completed;

		private long sourceChannelId;
        private string status;
        private bool isIndeterminate;
    	private bool isCompleted;
		private bool isRegistered;
		
		private long progress;
		private long maximum;

		public long SourceChannelId
    	{
			get { return sourceChannelId; }
    		set
    		{
				sourceChannelId = value;

				OnPropertyChanged("SourceChannelId");
				OnPropertyChanged("Channel");
    		}
    	}

		public ChannelConfiguration SourceChannel
		{
			get
			{
				var channel = ChannelsManager.GetChannelObject(SourceChannelId);

				return channel == null ? null : channel.Configuration;
			}
		}

		public long Progress
        {
            get { return progress; }
        }

		public long Maximum
		{
			get { return maximum; }
		}

        public bool IsIndeterminate
        {
            get { return isIndeterminate; }
        }

    	public bool IsCompleted
    	{
    		get { return isCompleted; }
			set
			{
				isCompleted = value;

				if (isCompleted)
				{
					OnGroupCompleted();
				}
			}
    	}

        public string Status
        {
            get { return status; }
            set
            {
                status = value;

                OnPropertyChanged("Status");
            }
        }
    	
    	public bool IsRegistered
    	{
    		get { return isRegistered; }
    		set
    		{
    			isRegistered = value;

				OnPropertyChanged("IsRegistered");
    		}
    	}

    	public ProgressGroup()
    	{
			this.isIndeterminate = true;
    	}

		public void SetMaximum(int max)
		{
			maximum = max;
			isIndeterminate = false;

			OnPropertyChanged("IsIndeterminate");			
			OnPropertyChanged("Maximum");
			OnPropertyChanged("Progress");
		}

		public void IncrementProgress()
		{
			progress++;

			OnPropertyChanged("Progress");
		}

		public void SetProgress(int progress)
		{
			this.progress = progress;

			OnPropertyChanged("Progress");

			if (progress >= 100)
				IsCompleted = true;
		}

		void OnGroupCompleted()
        {
			if (Completed != null)
				Thread.CurrentThread.RaiseUIEventHandler(this, Completed);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
        }
    }
}