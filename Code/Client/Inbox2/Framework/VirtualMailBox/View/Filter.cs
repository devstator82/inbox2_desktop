using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.VirtualMailBox.View
{
	public class Filter
	{
		public event EventHandler<EventArgs> FilterChanged;

		/// <summary>
		/// Gets or sets the currently selected activity view.
		/// </summary>
		public ActivityView CurrentView { get; set; }

		/// <summary>
		/// Gets or sets the currently selected label.
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Gets or sets a boolean indication if the activity view or one line view is visible.
		/// </summary>
		public bool IsActivityViewVisible { get; set; }	

		public string Key
		{
			get
			{
				return CurrentView + Label;
			}
		}

		public Filter()
		{
			CurrentView = ActivityView.MyInbox;
		}

		protected void OnFilterChanged()
		{
			if (FilterChanged != null)
				FilterChanged(this, EventArgs.Empty);
		}		
	}
}
