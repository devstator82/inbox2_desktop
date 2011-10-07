using System;
using System.Web;
using Inbox2.Channels.Facebook.REST;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Framework;
using Inbox2.Platform.Framework.Extensions;
using Newtonsoft.Json.Linq;

namespace Inbox2.Channels.Facebook
{
	/// <summary>
	/// Redirect builder is only used for configuring the Facebook channel in the desktop client.
	/// </summary>
	public class FacebookRedirectBuilder : IWebRedirectBuilder
	{
		public string Token { get; set; }

		public string TokenSecret { get; set; }

		public bool IsCloudRedirect { get; set; }

		public string SuccessUri
		{
			get { return "?session="; }
		}

		public FacebookRedirectBuilder()
		{
			Token = SafeSession.Current["/Channels/Facebook/Redirect/Token"] as string;
			TokenSecret = SafeSession.Current["/Channels/Facebook/Redirect/TokenSecret"] as string;
		}		

		public Uri BuildRedirectUri()
		{
			// The API then piggy backs off the same login mechanism as used by the desktop
			// client but uses the cloud specific API keys. FB expects the domain name to be something like
			// www.<environment>.inbox2.com in that case.
			string environment = "/Settings/Application/Environment".AsKey(String.Empty);
			var cloudRedirectUrl = String.IsNullOrEmpty(environment)
				? "http://www.inbox2.com/"
				: String.Concat("http://www.", environment, ".inbox2.com");

			return new Uri(String.Format("http://www.facebook.com/login.php?api_key={0}&connect_display=popup&v=1.0&next={1}&fbconnect=true&return_session=true&skipcookie=true&req_perms=read_stream,read_mailbox,publish_stream,offline_access", 
				FacebookApiKeys.GetApiKey(), IsCloudRedirect ? cloudRedirectUrl : "http://desktop.inbox2.com/success"));
		}

		public string ParseVerifier(string returnValue)
		{
			return returnValue;
		}

		public bool ValidateReturnValue(string returnValue)
		{
			var queryParams = NameValueParser.GetCollection(new Uri(returnValue).Query, "?", "&");

			if (queryParams["session"] == null)
				return false;

			JObject result = JObject.Parse(HttpUtility.UrlDecode(queryParams["session"]));

			if (result["session_key"] == null || result["secret"] == null)
				return false;

			Token = result["session_key"].Value<string>();
			TokenSecret = result["secret"].Value<string>();

			if (ChannelContext.Current != null && ChannelContext.Current.ClientContext != null)
			{
				// 7. Save auth keys
				ChannelContext.Current.ClientContext.SaveSetting("/Channels/Facebook/SessionKey", Token);
				ChannelContext.Current.ClientContext.SaveSetting("/Channels/Facebook/SessionSecret", TokenSecret);
			}

			return true;
		}

		public string GetUsername()
		{
			FacebookRESTClient client = new FacebookRESTClient(FacebookApiKeys.GetApiKey(), TokenSecret, Token, TokenSecret);
			var user = client.GetLoggedInUser();

			return user.Name;
		}
	}
}
