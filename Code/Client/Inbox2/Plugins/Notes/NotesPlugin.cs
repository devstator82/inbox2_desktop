using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Plugins.Notes.Helpers;

namespace Inbox2.Plugins.Notes
{
	[Export(typeof(PluginPackage))]
	public class NotesPlugin : PluginPackage, ISpringable
	{
		private readonly NotesState state;
		private readonly Controller controller;

		public override string Name
		{
			get { return "Notes"; }
		}

		public override int SortOrder
		{
			get { return 30; }
		}

		public override IStatePlugin State
		{
			get { return state; }
		}

		public override IColumnPlugin Colomn
		{
			get { return new PluginHelper(state); }
		}

		public override INewItemViewPlugin NewItemView
		{
			get { return new PluginHelper(state); }
		}

		public override Type[] DataTypes
		{
			get { return new[] { typeof(Note) }; }
		}

		public NotesPlugin()
		{
			state = new NotesState();
			controller = new Controller();
		}

		public override void Initialize()
		{
			EventBroker.Subscribe<Note>(AppEvents.NoteReceived, controller.NoteReceived);
			EventBroker.Subscribe<Message>(AppEvents.MessageReceived, controller.MessageReceived);
		}

        public override void LoadAsync()
        {
            var notes = ClientState.Current.DataService.SelectAll<Note>();

            Thread.CurrentThread.ExecuteOnUIThread(() => state.Notes.Replace(notes));
        }

		public override void SearchAsync(string searchQuery)
		{
			var notes = ClientState.Current.Search.PerformSearch<Note>(searchQuery);

			Thread.CurrentThread.ExecuteOnUIThread(() => state.Notes.Replace(notes));
		}

		public string[] KeyPrefixes
		{
			get { return new[] { EntityKeyPrefixes.Note }; }
		}

		public object Spring(string prefix, long id)
		{
			switch (prefix)
			{
				case EntityKeyPrefixes.Note:
					return ClientState.Current.DataService.SelectBy<Note>(new { InternalNoteId = id });
			}

			throw new NotSupportedException("The provided prefix is invalid");
		}
	}
}
