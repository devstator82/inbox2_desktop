using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Inbox2.Framework.UI
{
	public class Flipper
	{
		protected readonly TimeSpan flipIn;
		protected readonly Action action;
		protected readonly DispatcherTimer timer;

		public Flipper(TimeSpan flipIn, Action action)
		{
			this.flipIn = flipIn;
			this.action = action;

			timer = new DispatcherTimer(flipIn, DispatcherPriority.Background, Timer_Tick, Dispatcher.CurrentDispatcher);
			timer.Stop();
		}

		public bool IsRunning
		{
			get
			{
				return timer.IsEnabled;
			}
		}

		public void Dispose()
		{
			timer.Stop();
		}

		public void Delay()
		{
			if (timer.IsEnabled)
			{
				// Reset flipper
				timer.Stop();
				timer.Start();
			}
			else
			{
				timer.Start();
			}
		}

		protected void Timer_Tick(object sender, EventArgs e)
		{
			timer.Stop();

			// Execute code
			action.Invoke();
		}
	}
}
