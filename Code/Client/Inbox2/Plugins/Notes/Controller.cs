using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Notes
{
	public class Controller
	{
        public NotesState State
        {
            get { return (NotesState)PluginsManager.Current.GetPlugin<NotesPlugin>().State; }
        }

		public void NoteReceived(Note note)
		{
			// todo implemented reader/writer lock on notestate
			ClientState.Current.DataService.Save(note);

			Thread.CurrentThread.ExecuteOnUIThread(() => State.Notes.Add(note));			
		}

		public void MessageReceived(Message message)
		{
			if (message.Metadata.i2mpFlow == i2mpFlows.Note || IsMostlyUrlContent(message))
			{
				message.ProcessingHints = ProcessingHints.Ignore;

				int version = 0;

				Int32.TryParse(message.Metadata.i2mpSequence, out version);

				Note note = new Note();
				note.InternalMessageId = message.InternalMessageId;
				note.ConversationId = message.Metadata.i2mpReference;
				note.Version = version;
				note.ContentType = IsMostlyUrlContent(message) ? NoteTypes.Url : NoteTypes.Note;
				note.Context = message.Context;
				note.Content = message.GetBestBodyMatch(TextConversion.ToText);
				note.DownloadState = DownloadStates.Downloaded;
				note.SendState = SendStates.DoNotSend;
				note.NoteFolder = Folders.Inbox;
				note.NoteState = EntityStates.Unread;
				note.SourceChannelId = message.SourceChannelId;
				note.DateExpires = message.DateReceived.Value;
				note.DateCreated = DateTime.Now;

				ClientState.Current.DataService.Save(note);

				Thread.CurrentThread.ExecuteOnUIThread(() => State.Notes.Add(note));
			}
		}

		bool IsMostlyUrlContent(Message message)
		{
			Regex regex = new Regex(@"(?i:http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=~]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			string source = message.GetBestBodyMatch();
			MatchCollection mc = regex.Matches(source);

			if (mc.Count == 0)
				return false;

			// Get total length of all entries
			int total = 0;
			foreach (Match match in mc)
				total += match.Length;

			// See if our url(s) occupies at least 30% of the the text
			int perc = 100 * total / source.Length;

			return perc > 30;
		}
	}
}
