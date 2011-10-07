using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Calendar;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Web;
using Inbox2.Platform.Logging;

namespace Inbox2.Channels.GMail
{
	public class GoogleCalendarChannel : IClientCalendarChannel
	{
		protected CalendarService service = new CalendarService("Tabdeelee-Inbox2-1");

		#region Properties

		public string Hostname
		{
			get { return "http://www.google.com/calendar/feeds/"; }
			set { }
		}
		public int Port
		{
			get { return 80; }
			set { }
		}

		public bool IsSecured
		{
			get { return false; }
			set { }
		}

		public bool IsEnabled { get; set; }
		public int MaxConcurrentConnections { get; set; }
		public IChannelCredentialsProvider CredentialsProvider { get; set; }

		public string Protocol
		{
			get { return "GoogleContactsAIP"; }
		}

		#endregion

		public IEnumerable<ChannelCalendar> GetCalendars()
		{
			yield return new ChannelCalendar();
		}

		public IEnumerable<ChannelEvent> GetEvents(ChannelCalendar calendar)
		{
			var creds = CredentialsProvider.GetCredentials();
			service.setUserCredentials(creds.Claim, creds.Evidence);
			string url = String.Format("http://www.google.com/calendar/feeds/{0}/private/full", creds.Claim);

			// Check for 404 with custom httpclient on photourl since regular HttpWebClient keeps throwing exceptions
			var token = service.QueryAuthenticationToken();
			var code = HttpStatusClient.GetStatusCode(url,
				new Dictionary<string, string> { { "Authorization", "GoogleLogin auth=" + token } });

			if (!(code == 200 || code == 302))
			{
				Logger.Warn("Not downloading calendar events because of un expected HttpStatusCode. Code = [0}", LogSource.Sync, code);

				yield break;
			}

			EventQuery query = new EventQuery { Uri = new Uri(url) };
			EventFeed feed = service.Query(query);

			foreach (EventEntry item in feed.Entries)
			{
                // Create a new channelevent
				ChannelEvent evt = new ChannelEvent();

                // Get the event key
				evt.ChannelEventKey = item.Id.Uri.ToString();

                // Get subject, description and location
				evt.Subject = item.Title.Text;
				evt.Description = item.Content.Content;
			    evt.Location = item.Locations[0].ValueString;

                // Get the start, end date and date modified
				evt.StartDate = item.Times[0].StartTime;
                evt.EndDate = item.Times[0].EndTime;
			    evt.Modified = item.Updated;

                // Get the date of the event when it was initially created
			    evt.Stamp = item.Published;

                // Set the date of the event when it was created by Inbox2
                evt.DateCreated = DateTime.Now;

                // Get the status
				if (item.Status == EventEntry.EventStatus.CONFIRMED) evt.State = EventState.Confirmed;
				else if (item.Status == EventEntry.EventStatus.TENTATIVE) evt.State = EventState.Tentative;
				else if (item.Status == EventEntry.EventStatus.CANCELED) evt.State = EventState.Cancelled;
				else evt.State = EventState.Confirmed;

                // Get the visability (/class)
                if (item.EventVisibility.Value == EventEntry.Visibility.PUBLIC_VALUE) evt.Class = EventClassType.Public;
                else if (item.EventVisibility.Value == EventEntry.Visibility.PRIVATE_VALUE) evt.Class = EventClassType.Private;
                else if (item.EventVisibility.Value == EventEntry.Visibility.CONFIDENTIAL_VALUE) evt.Class = EventClassType.Confidential;
                else if (item.EventVisibility.Value == EventEntry.Visibility.DEFAULT_VALUE) evt.Class = EventClassType.Public;
                else evt.Class = EventClassType.Public;

                // Gmail hasn't a priority status, so set it to None
			    evt.Priority = EventPriority.None;

                // Return the event
				yield return evt;
			}
		}

		public void Dispose()
		{
			
		}
	}
}
