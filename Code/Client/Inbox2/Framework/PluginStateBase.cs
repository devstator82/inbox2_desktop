using System;
using System.ComponentModel;
using Inbox2.Framework.Interfaces.Plugins;

namespace Inbox2.Framework
{
	public abstract class PluginStateBase : IStatePlugin, INotifyPropertyChanged
	{
		public event EventHandler<EventArgs> SelectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;		

		public virtual bool CanView
		{
			get { return false; }
		}

		public virtual bool CanNew
		{
			get { return false; }
		}

		public virtual bool CanReply
		{
			get { return false; }
		}

		public virtual bool CanReplyAll
		{
			get { return false; }
		}

		public virtual bool CanForward
		{
			get { return false; }
		}

		public virtual bool CanDelete
		{
			get { return false; }
		}

		public virtual bool CanStar
		{
			get { return false; }
		}

		public virtual bool CanMarkRead
		{
			get { return false; }
		}

		public virtual bool CanMarkUnread
		{
			get { return false; }
		}

		public void View()
		{
		    ViewCore();
		}

		public void New()
		{
		    NewCore();
		}

		public void Reply()
		{
			ReplyCore();
		}

		public void ReplyAll()
		{
			ReplyAllCore();
		}

		public void Forward()
		{
			ForwardCore();
		}

		public void Delete()
		{
			DeleteCore();
		}		

		public void MarkRead()
		{
			MarkReadCore();
		}

		public void MarkUnread()
		{
			MarkUnreadCore();
		}

		public virtual void Shutdown()
		{
		}

		protected virtual void ViewCore()
		{
		}

		protected virtual void NewCore()
		{
		}

		protected virtual void ReplyCore()
		{
		}

		protected virtual void ReplyAllCore()
		{
		}

		protected virtual void ForwardCore()
		{
		}

		protected virtual void DeleteCore()
		{
		}

		protected virtual void MarkReadCore()
		{
		}

		protected virtual void MarkUnreadCore()
		{
		}		

        protected void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }

		/// <summary>
		/// Called when [property changed].
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}		
	}
}