using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Plugins.Entities;

namespace Inbox2.Plugins.Calendar
{
    public class Controller
    {
        public CalendarState State
        {
            get { return (CalendarState)PluginsManager.Current.GetPlugin<CalendarPlugin>().State; }
        }

		public void CalendarEventReceived(Event evt)
		{			
			ClientState.Current.DataService.Save(evt);

			Thread.CurrentThread.ExecuteOnUIThread(() => State.Events.Add(evt));
		}
    }
}
