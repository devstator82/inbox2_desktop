using System;
using System.ComponentModel.Composition;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Plugins.Calendar.Helpers;

namespace Inbox2.Plugins.Calendar
{
    [Export(typeof(PluginPackage))]
    public class CalendarPlugin : PluginPackage, ISpringable
    {
        private readonly CalendarState state;
        private readonly Controller controller;

    	public override string Name
    	{
			get { return "Calendar"; }
    	}

    	public override int SortOrder
        {
            get { return 100; }
        }

        public override IStatePlugin State
        {
            get { return state; }
        }

        public override IColumnPlugin Colomn
        {
            get { return new PluginHelper(state); }
        }

        public override Type[] DataTypes
        {
            get { return new[] { typeof(Event) }; }
        }

        public CalendarPlugin()
		{
            state = new CalendarState();
			controller = new Controller();
        }

        public override void Initialize()
        {
			EventBroker.Subscribe<Event>(AppEvents.CalendarEventReceived, controller.CalendarEventReceived);
        }

        public override void LoadAsync()
        {
            var events = ClientState.Current.DataService.SelectAll<Event>();

            Thread.CurrentThread.ExecuteOnUIThread(() => state.Events.Replace(events));
        }

        public override void SearchAsync(string searchQuery)
        {
            
        }

        public string[] KeyPrefixes
        {
            get { return new[] { EntityKeyPrefixes.Event }; }
        }

        public object Spring(string prefix, long id)
        {
            switch (prefix)
            {
                case EntityKeyPrefixes.Event:
					return ClientState.Current.DataService.SelectBy<Event>(new { InternalEventId = id });
            }

            throw new NotSupportedException("The provided prefix is invalid");
        }
    }
}
