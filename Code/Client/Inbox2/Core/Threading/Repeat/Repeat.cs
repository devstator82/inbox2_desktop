using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Inbox2.Core.Threading.Repeat
{
	public class Repeat : ScheduledItem
	{
		private int repeatDelay;
		private TimeSpan repeatEvery;		
		
		public Repeat(string key) : base(key)
		{			
		}

		protected Repeat(string key, TimeSpan repeatEvery, Action action)
			: base(key)
		{
			DateScheduled = DateTime.Now.Add(repeatEvery);

			this.repeatEvery = repeatEvery;
			this.action = action;
		}

		public Repeat Every(int repeatDelay)
		{
			this.repeatDelay = repeatDelay;

			return this;
		}

		public Repeat Seconds()
		{
			repeatEvery = TimeSpan.FromSeconds(repeatDelay);

			DateScheduled = DateTime.Now.Add(repeatEvery);

			return this;
		}

		public Repeat Minutes()
		{
			repeatEvery = TimeSpan.FromMinutes(repeatDelay);

			DateScheduled = DateTime.Now.Add(repeatEvery);

			return this;
		}

		public override void OnAfterExecute()
		{
			// Re-register new instance of this repeat
			ScheduledTaskRunner.Enqueue(new Repeat(Key, repeatEvery, action));
		}
	}
}