using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Plugins.Contacts.Helpers;

namespace Inbox2.Plugins.Contacts
{
	[Export(typeof(PluginPackage))]
	public class ContactsPlugin : PluginPackage
	{
		private readonly ContactsState state;
		private readonly PluginHelper overview;		

		public override string Name
		{
			get { return "Contacts"; }
		}
		
		public override IStatePlugin State
		{
			get { return state; }
		}

		public override IColumnPlugin Colomn
		{
			get { return new PluginHelper(state); }
		}

		public override IEnumerable<IOverviewPlugin> Overviews
		{
			get { return new[] { overview }; }
		}

		public ContactsPlugin()
		{
			state = new ContactsState();
			overview = new PluginHelper(state);

			EventBroker.Subscribe<Person>(AppEvents.New, delegate { State.New(); });
			EventBroker.Subscribe<Person>(AppEvents.View, delegate(Person person)
			{
				ClientState.Current.ViewController.MoveTo(WellKnownView.Contacts);

				state.IsExternalSelection = true;
				state.SelectedPersons.Replace(new[] { person });
				state.IsExternalSelection = false;
			});
		}
	}
}
