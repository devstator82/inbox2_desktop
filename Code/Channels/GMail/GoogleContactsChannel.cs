using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Contacts;
using Google.GData.Client;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Channels.GMail
{
	public class GoogleContactsChannel : IClientContactsChannel
	{
		#region Properties

		public string Hostname
		{
			get { return "http://www.google.com/m8/feeds/contacts/"; }
			set { }
		}
		public int Port
		{
			get { return 80; }
			set { }
		}

		public bool IsSecured
		{
			get { return false;  }
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
		
		public IEnumerable<ChannelContact> GetContacts()
		{
			var cred = CredentialsProvider.GetCredentials();
			var rs = new RequestSettings("Tabdeelee-Inbox2-1", cred.Claim, cred.Evidence) { AutoPaging = true };			
			var cr = new ContactsRequest(rs);

			var feed = cr.GetContacts();

			foreach (Contact entry in feed.Entries)
			{
				ChannelContact contact = new ChannelContact();

				contact.Person.Name = entry.Title;
				contact.Profile.ChannelProfileKey = entry.Id;

				if (entry.Phonenumbers.Count > 0)
				{
					var phone = entry.Phonenumbers.First();
					contact.Profile.PhoneNr = phone.Value;
				}

				if (entry.PrimaryEmail != null)
					contact.Profile.SourceAddress = new SourceAddress(entry.PrimaryEmail.Address, contact.Person.Name);

				try
				{
					// Check for 404 with custom httpclient on photourl since regular HttpWebClient keeps throwing exceptions
					//var token = cr.Service.QueryAuthenticationToken();
					//var code = HttpStatusClient.GetStatusCode(entry.PhotoUri.ToString(), 
					//    new Dictionary<string, string> { { "Authorization", "GoogleLogin auth=" + token }});

					//if (code.HasValue && code == 200)
					//{									
						IGDataRequest request = cr.Service.RequestFactory.CreateRequest(GDataRequestType.Query, entry.PhotoUri);					
						request.Execute();

						using (var avatarstream = request.GetResponseStream())
						{
							if (avatarstream != null)
							{
								ChannelAvatar avatar = new ChannelAvatar();

								// Copy avatarstream to a new memorystream because the source
								// stream does not support seek operations.
								MemoryStream ms = new MemoryStream();
								avatarstream.CopyTo(ms);

								avatar.Url = entry.PhotoUri.ToString();
								avatar.ContentStream = ms;

								contact.Profile.ChannelAvatar = avatar;
							}
						}
					//}
				}
				catch (CaptchaRequiredException)
				{
					// Since GMail will keep on raising CaptchaRequiredException, break out here
					// todo let the user know in some way or another?
					yield break;	
				}
				catch (Exception)
				{
					
				}				
				
				yield return contact;
			}			
		}

		public IClientContactsChannel Clone()
		{
			return new GoogleContactsChannel
				{
					Hostname = Hostname,
					Port = Port,
					IsSecured = IsSecured,
					MaxConcurrentConnections = MaxConcurrentConnections,
					CredentialsProvider = CredentialsProvider
				};
		}

		public void Dispose()
		{
			
		}
	}
}