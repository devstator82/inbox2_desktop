using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Inbox2.Framework
{
	public abstract class LauncherCommand : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string description;

		public string Description
		{
			get { return description; }
			set
			{
				description = value;

				OnPropertyChanged("Description");
			}
		}

		protected LauncherCommand(string description)
		{
			this.description = description;

			UpdateDescription(String.Empty);
		}

		public abstract void Execute(string query);

		public virtual void UpdateDescription(string query)
		{
		}

		public virtual bool CanExecute(string query)
		{
			return true;
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
